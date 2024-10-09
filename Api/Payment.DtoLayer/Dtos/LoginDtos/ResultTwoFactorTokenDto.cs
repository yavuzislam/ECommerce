namespace Payment.DtoLayer.Dtos.LoginDtos
{
    public class ResultTwoFactorTokenDto
    {
        public string? AuthenticatorKey { get; set; }
        public string? QrCodeUrl { get; set; }
    }
}
