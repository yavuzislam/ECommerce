namespace Payment.WebUI.Utilities.Tool;

public class TokenResponseDto
{
    public TokenResponseDto(string token, DateTime expireDate, string provider)
    {
        Token = token;
        ExpireDate = expireDate;
        Provider = provider;
    }

    public string Token { get; set; }           // JWT Token
    public DateTime ExpireDate { get; set; }    // Token'ın son kullanma tarihi
    public string Provider { get; set; }        // Giriş sağlayıcısı (Google)
}
