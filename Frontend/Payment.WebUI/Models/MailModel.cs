namespace Payment.WebUI.Models
{
    public class MailModel
    {
        public string Message { get; set; }
        public string ConfirmationToken { get; set; }
        public string ConfirmationLink { get; set; }
    }
}
