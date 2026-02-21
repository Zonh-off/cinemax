using System.Security.Claims;
using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[Authorize]
public class OrdersController(IOrderService orders, ISeatHoldService holds, IMapper mapper, IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderListItemResponse>>> GetMyOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var spec = new OrdersForUserSpec(userId);

        var orders = await unit.Repository<Order>().GetAllEntityWithSpec(spec);

        var result = mapper.Map<List<OrderListItemResponse>>(orders);
        return Ok(result);
    }
    
    [HttpGet("{orderId:int}/tickets")]
    public async Task<ActionResult<IReadOnlyList<TicketResponse>>> GetOrderTickets(int orderId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var spec = new TicketsForUserOrderSpec(orderId, userId);
        var tickets = await unit.Repository<OrderTicket>().GetAllEntityWithSpec(spec);
        
        if (tickets.IsNullOrEmpty()) return NotFound();

        return Ok(mapper.Map<List<TicketResponse>>(tickets));
    }
    
    [HttpPost("{showtimeId:int}/hold")]
    public async Task<ActionResult> HoldSeat(int showtimeId, [FromBody] SeatHoldRequest dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var (ok, reason) = await holds.HoldSeatAsync(
            showtimeId,
            dto.SeatId,
            userId,
            TimeSpan.FromMinutes(5)); // dev TTL

        return ok ? Ok(new { showtimeId, dto.SeatId }) : BadRequest(new { reason });
    }

    [HttpPost("{showtimeId:int}/release")]
    public async Task<ActionResult> ReleaseSeat(int showtimeId, [FromBody] SeatHoldRequest dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        await holds.ReleaseSeatAsync(showtimeId, dto.SeatId, userId);

        return Ok(new { showtimeId, dto.SeatId });
    }
    
    [HttpPost("{showtimeId:int}/hold-many")]
    public async Task<ActionResult> HoldMany(int showtimeId, [FromBody] SeatsHoldRequest dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var results = new List<object>();
        foreach (var seatId in dto.SeatIds.Distinct())
        {
            var (ok, reason) = await holds.HoldSeatAsync(showtimeId, seatId, userId, TimeSpan.FromMinutes(5));
            results.Add(new { seatId, ok, reason });
        }

        return Ok(results);
    }
    
    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] CreateOrderRequest dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var model = new CreateOrderModel
        {
            ShowtimeId = dto.ShowtimeId,
            SeatIds = dto.SeatIds
        };

        var result = await orders.CreatePendingOrderAsync(userId, model);
        return Ok(result);
    }

    [HttpPost("{orderId:int}/confirm")]
    public async Task<ActionResult> Confirm(int orderId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await orders.ConfirmOrderAsync(userId, orderId);
        return Ok();
    }
}