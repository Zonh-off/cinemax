using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class TicketsController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet("/api/tickets/{ticketCode}")]
    public async Task<ActionResult<TicketScanResponse>> Scan(string ticketCode)
    {
        var spec = new TicketByCodeSpec(ticketCode);
        var ticket = await unit.Repository<OrderTicket>().GetEntityWithSpec(spec);

        if (ticket == null)
            return NotFound(new TicketScanResponse { IsValid = false, Code = "NotFound", Message = "Ticket not found" });

        if (ticket.Order.Status != OrderStatus.Confirmed)
            return Conflict(new TicketScanResponse { IsValid = false, Code = "NotConfirmed", Message = "Order is not confirmed" });

        var now = DateTime.UtcNow;
        var start = ticket.Showtime.StartTime;

        if (now > start.AddMinutes(30))
            return Conflict(new TicketScanResponse { IsValid = false, Code = "ShowtimePassed", Message = "Showtime already passed" });

        var dto = mapper.Map<TicketResponse>(ticket);

        return Ok(new TicketScanResponse
        {
            IsValid = true,
            Code = "OK",
            Ticket = dto
        });
    }
}