using Payment.EntityLayer.Concrete;

namespace Payment.BusinessLayer.Abstract;

public interface IRefreshTokenService
{
    Task<RefreshToken> GetByTokenAsync(string token);
    Task CreateRefreshTokenAsync(RefreshToken refreshToken);
    Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
}
