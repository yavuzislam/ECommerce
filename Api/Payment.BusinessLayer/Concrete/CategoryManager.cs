using Payment.BusinessLayer.Abstract;
using Payment.DataAccessLayer.Abstract;
using Payment.DtoLayer.Dtos.CategoryDtos;
using Payment.EntityLayer.Concrete;
using Serilog;

namespace Payment.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        private readonly ILogger _logger;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
            _logger = Log.ForContext<CategoryManager>();
        }

        public async Task InsertAsync(Category category)
        {
            try
            {
                if (category == null)
                {
                    var argumentNullException = new ArgumentNullException(nameof(category));
                    _logger.Warning(argumentNullException, "Null kategori nesnesi sağlandı.");
                    throw argumentNullException;
                }

                await _categoryDal.InsertAsync(category);
                _logger.Information("Kategori başarıyla eklendi. Kategori ID: {CategoryId}", category.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Kategori ekleme sırasında bir hata oluştu.");
                throw;
            }
        }

        public async Task UpdateAsync(Category category)
        {
            try
            {
                if (category == null)
                {
                    var argumentNullException = new ArgumentNullException(nameof(category));
                    _logger.Warning(argumentNullException, "Null kategori nesnesi sağlandı.");
                    throw argumentNullException;
                }

                await _categoryDal.UpdateAsync(category);
                _logger.Information("Kategori başarıyla güncellendi. Kategori ID: {CategoryId}", category.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Kategori güncelleme sırasında bir hata oluştu. Kategori ID: {CategoryId}", category.Id);
                throw;
            }
        }

        public async Task DeleteAsync(Category category)
        {
            try
            {
                if (category == null)
                {
                    var argumentNullException = new ArgumentNullException(nameof(category));
                    _logger.Warning(argumentNullException, "Null kategori nesnesi sağlandı.");
                    throw argumentNullException;
                }

                await _categoryDal.DeleteAsync(category);
                _logger.Information("Kategori başarıyla silindi. Kategori ID: {CategoryId}", category.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Kategori silme sırasında bir hata oluştu. Kategori ID: {CategoryId}", category.Id);
                throw;
            }
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    var argumentException = new ArgumentException("Geçersiz kategori ID.", nameof(id));
                    _logger.Warning(argumentException, "Geçersiz kategori ID sağlandı. ID: {CategoryId}", id);
                    throw argumentException;
                }

                var category = await _categoryDal.GetByIdAsync(id);

                if (category == null)
                {
                    _logger.Information("Kategori bulunamadı. Kategori ID: {CategoryId}", id);
                }
                else
                {
                    _logger.Information("Kategori başarıyla getirildi. Kategori ID: {CategoryId}", id);
                }

                return category;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Kategori getirme sırasında bir hata oluştu. Kategori ID: {CategoryId}", id);
                throw;
            }
        }

        public async Task<List<Category>> GetListAsync()
        {
            try
            {
                var categories = await _categoryDal.GetListAsync();
                _logger.Information("Kategori listesi başarıyla getirildi. Toplam Kategori Sayısı: {Count}", categories.Count);
                return categories;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Kategori listesi getirme sırasında bir hata oluştu.");
                throw;
            }
        }

        public async Task<List<ResultCategoryByUserEmailDto>> TGetAllByUserEmail()
        {
            try
            {
                var categories = await _categoryDal.GetAllByUserEmail();
                _logger.Information("Kategori listesi başarıyla getirildi. Toplam Kategori Sayısı: {Count}", categories.Count);
                return categories;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Kategori listesi getirme sırasında bir hata oluştu.");
                throw;
            }
        }
    }
}
