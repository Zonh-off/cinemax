namespace API.DTOs;

public class TicketScanResponse
{
    public bool IsValid { get; set; }
    public string Code { get; set; } = "";
    public string? Message { get; set; }

    public TicketResponse? Ticket { get; set; }
}