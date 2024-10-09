namespace Payment.BusinessLayer.Utilities.Services.MailServices;

public interface IEmailSenderService
{
    Task SendEmailAsync(string email, string subject, string message);
}
