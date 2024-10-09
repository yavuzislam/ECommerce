using Microsoft.AspNetCore.Http;
using Payment.DataAccessLayer.Abstract;
using Payment.DataAccessLayer.Concrete;
using Payment.DataAccessLayer.Repositories;
using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.EntityFramework;

public class EfAddressDal : GenericRepository<Address>, IAddressDal
{
    public EfAddressDal(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
    {
    }
}
