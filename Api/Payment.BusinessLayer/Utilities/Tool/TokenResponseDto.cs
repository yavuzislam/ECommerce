namespace Payment.BusinessLayer.Utilities.Tool;
public class TokenResponseDto
{
    public TokenResponseDto(string token, DateTime expireDate, string provider)
    {
        Token = token;
        ExpireDate = expireDate;
        Provider = provider;
    }

    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public string Provider { get; set; }
}

