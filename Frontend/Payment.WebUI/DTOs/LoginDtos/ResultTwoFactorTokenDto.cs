namespace Payment.WebUI.DTOs.LoginDtos
{
    public class ResultTwoFactorTokenDto
    {
        public string? AuthenticatorKey { get; set; }
        public string? QrCodeUrl { get; set; }
    }
}
