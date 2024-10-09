using Payment.WebUI.Utilities.Tool;

namespace Payment.WebUI.DTOs.LoginDtos
{
    public class LoginResponseDto
    {
        public bool RequiresTwoFactor { get; set; }
        public string UserId { get; set; }
        public TokenResponseDto TokenResponse { get; set; }
    }

}
