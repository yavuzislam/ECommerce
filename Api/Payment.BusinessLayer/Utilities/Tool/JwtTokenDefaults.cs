namespace Payment.BusinessLayer.Utilities.Tool;

public class JwtTokenDefaults
{
    public const string ValidAudience = "PaymentApi";
    public const string ValidIssuer = "https://localhost:7276";
    public const int Expire = 60;

    public static string GoogleValidIssuer = "https://accounts.google.com";
    public static int GoogleTokenExpire = 10;

    //public const int RefreshTokenExpireDays = 7;
}
