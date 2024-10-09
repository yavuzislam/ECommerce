using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Payment.DataAccessLayer.Abstract;
using Payment.DataAccessLayer.Concrete;
using Payment.DataAccessLayer.Repositories;
using Payment.DtoLayer.Dtos.CategoryDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.EntityFramework
{
    public class EfCategoryDal : GenericRepository<Category>, ICategoryDal
    {
        private readonly Context _context;
        public EfCategoryDal(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
            _context = context;
        }

        public async Task<List<ResultCategoryByUserEmailDto>> GetAllByUserEmail()
        {
            var values = await _context.Categories
                .Include(x => x.CreateUser)
                .Include(x => x.UpdateUser)
                .Select(x => new ResultCategoryByUserEmailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImagePath = x.ImagePath,
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