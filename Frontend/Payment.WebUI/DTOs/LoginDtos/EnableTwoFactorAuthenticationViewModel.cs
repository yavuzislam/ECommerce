namespace Payment.WebUI.DTOs.LoginDtos
{
    public class EnableTwoFactorAuthenticationViewModel
    {
        public string SharedKey { get; set; }
        public string AuthenticatorUri { get; set; }
        public string Code { get; set; }
    }

}
