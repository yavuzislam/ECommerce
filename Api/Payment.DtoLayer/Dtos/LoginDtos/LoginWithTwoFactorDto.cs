namespace Payment.DtoLayer.Dtos.LoginDtos;

public class LoginWithTwoFactorDto
{
    public string UserId { get; set; } 
    public string Token { get; set; } 
    public string Password { get; set; } 
}