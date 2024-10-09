namespace Payment.WebUI.Services.MailServices;

public interface IEmailSenderService
{
    Task SendEmailAsync(string email, string subject, string message);
}
