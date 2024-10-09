using Payment.BusinessLayer.Abstract;
using Payment.DataAccessLayer.Abstract;
using Payment.EntityLayer.Concrete;

namespace Payment.BusinessLayer.Concrete;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _refreshTokenRepository.AddAsync(refreshToken);
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        return await _refreshTokenRepository.GetByTokenAsync(token);
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _refreshTokenRepository.UpdateAsync(refreshToken);
    }
}
