using Core.Entities;
using Infrastucture.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin;

public class UpdateOrderStatusRequest
{
    public OrderStatus Status { get; set; }
}

[Authorize(Roles="Admin")]
[Route("api/admin/orders")]
public class AdminOrdersController(StoreContext context) : ControllerBase
{
    [HttpPut("{orderId:int}/status")]
    public async Task<ActionResult> UpdateStatus(int orderId, [FromBody] UpdateOrderStatusRequest req)
    {
        var order = await context.Orders.FindAsync(orderId);
        if (order == null) return NotFound();

        order.Status = req.Status;
        await context.SaveChangesAsync();

        return Ok(new { orderId, status = order.Status.ToString() });
    }
}