using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.Abstract;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByTokenAsync(string token);
    Task AddAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
}
