using Core.Interfaces;
using FluentEmail.Core;

namespace Infrastucture.Services.EmailService;

public class EmailService(IFluentEmail mail) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var response = await mail
           .To(to)
           .Subject(subject)
           .Body(body, isHtml: true)
           .SendAsync();

        if (!response.Successful)
        {
            throw new Exception($"Failed to send email: {string.Join(", ", response.ErrorMessages)}");
        }
    }
}