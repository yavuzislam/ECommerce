using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Payment.DataAccessLayer.Abstract;
using Payment.DataAccessLayer.Concrete;
using Payment.DataAccessLayer.Repositories;
using Payment.DtoLayer.Dtos.ProductDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.EntityFramework
{
    public class EfProductDal : GenericRepository<Product>, IProductDal
    {
        private readonly Context _context;
        public EfProductDal(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
            _context = context;
        }

        public async Task<List<ResultProductByUserEmailDto>> GetAllByUserEmail()
        {
            var values = await _context.Products
                .Include(x => x.CreateUser)
                .Include(x => x.UpdateUser)
                .Select(x => new ResultProductByUserEmailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImagePath = x.ImagePath,
                    Price = x.Price,
                    DiscountRate = x.DiscountRate,
                    Stock = x.Stock,
                    CategoryId = x.CategoryId,
                    IsActive = x.IsActive,
                    CreateUserEmail = x.CreateUser.Email,
                    UpdateUserEmail = x.UpdateUser.Email,
                    CreateTime = x.CreateTime,
                    UpdateTime = x.UpdateTime
                }).ToListAsync();
            return values;
        }

        public async Task<List<ResultProductByUserEmailWithCategoryNameDto>> GetAllByUserEmailWithCategoryName()
        {
            var values = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.CreateUser)
                .Include(x => x.UpdateUser)
                .Select(x => new ResultProductByUserEmailWithCategoryNameDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImagePath = x.ImagePath,
                    Price = x.Price,
                    DiscountRate = x.DiscountRate,
                    Stock = x.Stock,
                    CategoryName = x.Category.Name,
                    IsActive = x.IsActive,
                    CreateUserEmail = x.CreateUser.Email,
                    UpdateUserEmail = x.UpdateUser.Email,
                    CreateTime = x.CreateTime,
                    UpdateTime = x.UpdateTime
                }).ToListAsync();
            return values;
        }
    }
}
