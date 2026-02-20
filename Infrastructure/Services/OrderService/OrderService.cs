using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Orders;

public class OrderService : IOrderService
{
    private readonly StoreContext _context;
    private readonly ISeatHoldService _holds;
    private readonly IShowtimeNotifier _notifier;

    public OrderService(StoreContext context, ISeatHoldService holds, IShowtimeNotifier notifier)
    {
        _context = context;
        _holds = holds;
        _notifier = notifier;
    }

    public async Task<OrderSummaryModel> CreatePendingOrderAsync(string userId, CreateOrderModel req)
    {
        if (req.SeatIds == null || req.SeatIds.Count == 0)
            throw new Exception("No seats selected");

        // Load showtime + hall + seats
        var showtime = await _context.Showtimes
            .Include(s => s.Hall)
                .ThenInclude(h => h.Seats)
            .FirstOrDefaultAsync(s => s.Id == req.ShowtimeId);

        if (showtime == null) throw new Exception("Showtime not found");

        // Already sold seats check
        var soldSeatIds = await _context.Set<OrderTicket>()
            .Where(t => t.ShowtimeId == req.ShowtimeId && req.SeatIds.Contains(t.SeatId))
            .Select(t => t.SeatId)
            .ToListAsync();

        if (soldSeatIds.Count > 0)
            throw new Exception($"Some seats already sold: {string.Join(",", soldSeatIds)}");

        // Holds check (must be held by this user)
        foreach (var seatId in req.SeatIds)
        {
            if (!await _holds.IsHeldByUserAsync(req.ShowtimeId, seatId, userId))
                throw new Exception($"Seat {seatId} is not held by you");
        }

        // Load prices for showtime
        var priceMap = await _context.ShowtimeSeatPrices
            .Where(p => p.ShowtimeId == req.ShowtimeId)
            .ToDictionaryAsync(p => p.SeatTypeId, p => p.Price);

        var seatTypeNames = await _context.SeatTypes
            .ToDictionaryAsync(x => x.Id, x => x.Name);

        var selectedSeats = showtime.Hall.Seats
            .Where(s => req.SeatIds.Contains(s.Id))
            .ToList();

        if (selectedSeats.Count != req.SeatIds.Count)
            throw new Exception("Some seats not found in hall");

        var expiresAt = DateTime.UtcNow.AddMinutes(10);

        decimal total = 0;
        var summaries = new List<OrderTicketSummaryModel>();
        var reservedSeats = new List<OrderReservedSeat>();

        foreach (var seat in selectedSeats)
        {
            if (!priceMap.TryGetValue(seat.SeatTypeId, out var unitPrice))
                throw new Exception($"No price for seat type {seat.SeatTypeId}");

            total += unitPrice;

            summaries.Add(new OrderTicketSummaryModel
            {
                SeatId = seat.Id,
                Row = seat.Row,
                Number = seat.Number,
                SeatType = seatTypeNames.TryGetValue(seat.SeatTypeId, out var n) ? n : "Unknown",
                UnitPrice = unitPrice
            });

            reservedSeats.Add(new OrderReservedSeat
            {
                OrderId = 0, // set after order saved
                ShowtimeId = req.ShowtimeId,
                SeatId = seat.Id,
                SeatTypeId = seat.SeatTypeId,
                UnitPrice = unitPrice
            });
        }

        // Create order + reserved seats in transaction
        await using var tx = await _context.Database.BeginTransactionAsync();

        var order = new Order
        {
            UserId = userId,
            ShowtimeId = req.ShowtimeId,
            Status = OrderStatus.Pending,
            TotalPrice = total,
            ExpiresAt = expiresAt
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var rs in reservedSeats)
            rs.OrderId = order.Id;

        _context.Set<OrderReservedSeat>().AddRange(reservedSeats);
        await _context.SaveChangesAsync();

        await tx.CommitAsync();

        return new OrderSummaryModel
        {
            OrderId = order.Id,
            ShowtimeId = req.ShowtimeId,
            ExpiresAt = expiresAt,
            TotalPrice = total,
            Status = order.Status.ToString(),
            Tickets = summaries
        };
    }

    public async Task ConfirmOrderAsync(string userId, int orderId)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null) throw new Exception("Order not found");
        if (order.Status != OrderStatus.Pending) throw new Exception("Order is not pending");
        if (order.ExpiresAt.HasValue && order.ExpiresAt.Value < DateTime.UtcNow)
            throw new Exception("Order expired");

        // TODO: check payment success before confirm

        var reserved = await _context.Set<OrderReservedSeat>()
            .Where(x => x.OrderId == orderId)
            .ToListAsync();

        if (reserved.Count == 0)
            throw new Exception("No reserved seats for this order");

        // Ensure holds still belong to user (optional but recommended)
        foreach (var rs in reserved)
        {
            if (!await _holds.IsHeldByUserAsync(order.ShowtimeId, rs.SeatId, userId))
                throw new Exception($"Seat {rs.SeatId} is not held by you anymore");
        }

        await using var tx = await _context.Database.BeginTransactionAsync();

        // Insert final tickets (DB unique constraint on (ShowtimeId, SeatId) protects double sell)
        var tickets = reserved.Select(rs => new OrderTicket
        {
            OrderId = order.Id,
            ShowtimeId = rs.ShowtimeId,
            SeatId = rs.SeatId,
            SeatTypeId = rs.SeatTypeId,
            UnitPrice = rs.UnitPrice
        }).ToList();

        _context.Set<OrderTicket>().AddRange(tickets);

        order.Status = OrderStatus.Confirmed;
        order.ExpiresAt = null;

        // Remove reserved seats after confirm
        _context.Set<OrderReservedSeat>().RemoveRange(reserved);

        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        // notify clients that seats are confirmed (sold)
        await _notifier.SeatsConfirmedAsync(order.ShowtimeId, tickets.Select(t => t.SeatId).ToList());

        // optional: release redis holds for these seats
        foreach (var seatId in tickets.Select(t => t.SeatId))
            await _holds.ReleaseSeatAsync(order.ShowtimeId, seatId, userId);
    }
}