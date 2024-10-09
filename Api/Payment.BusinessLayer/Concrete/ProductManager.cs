using Payment.BusinessLayer.Abstract;
using Payment.DataAccessLayer.Abstract;
using Payment.DtoLayer.Dtos.ProductDtos;
using Payment.EntityLayer.Concrete;
using Serilog;

namespace Payment.BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly ILogger _logger;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
            _logger = Log.ForContext<ProductManager>();
        }

        public async Task InsertAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    var argumentNullException = new ArgumentNullException(nameof(product));
                    _logger.Warning(argumentNullException, "Null ürün nesnesi sağlandı.");
                    throw argumentNullException;
                }

                await _productDal.InsertAsync(product);
                _logger.Information("Ürün başarıyla eklendi. Ürün ID: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ürün ekleme sırasında bir hata oluştu.");
                throw;
            }
        }

        public async Task UpdateAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    var argumentNullException = new ArgumentNullException(nameof(product));
                    _logger.Warning(argumentNullException, "Null Ürün nesnesi sağlandı.");
                    throw argumentNullException;
                }

                await _productDal.UpdateAsync(product);
                _logger.Information("Ürün başarıyla güncellendi. Ürün ID: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ürün güncelleme sırasında bir hata oluştu. Ürün ID: {ProductId}", product.Id);
                throw;
            }
        }

        public async Task DeleteAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    var argumentNullException = new ArgumentNullException(nameof(product));
                    _logger.Warning(argumentNullException, "Null ürün nesnesi sağlandı.");
                    throw argumentNullException;
                }

                await _productDal.DeleteAsync(product);
                _logger.Information("Ürün başarıyla silindi. Ürün ID: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ürün silme sırasında bir hata oluştu. Ürün ID: {ProductId}", product.Id);
                throw;
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    var argumentException = new ArgumentException("Geçersiz ürün ID.", nameof(id));
                    _logger.Warning(argumentException, "Geçersiz ürün ID sağlandı. ID: {ProductId}", id);
                    throw argumentException;
                }

                var Product = await _productDal.GetByIdAsync(id);

                if (Product == null)
                {
                    _logger.Information("Ürün bulunamadı. Ürün ID: {ProductId}", id);
                }
                else
                {
                    _logger.Information("Ürün başarıyla getirildi. Ürün ID: {ProductId}", id);
                }

                return Product;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ürün getirme sırasında bir hata oluştu. Ürün ID: {ProductId}", id);
                throw;
            }
        }

        public async Task<List<Product>> GetListAsync()
        {
            try
            {
                var products = await _productDal.GetListAsync();
                _logger.Information("Ürün listesi başarıyla getirildi. Toplam Ürün Sayısı: {Count}", products.Count);
                return products;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ürün listesi getirme sırasında bir hata oluştu.");
                throw;
            }
        }

        public async Task<List<ResultProductByUserEmailDto>> TGetAllByUserEmail()
        {
            try
            {
                var products = await _productDal.GetAllByUserEmail();
                _logger.Information("Ürün listesi başarıyla getirildi. Toplam Ürün Sayısı: {Count}", products.Count);
                return products;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ürün listesi getirme sırasında bir hata oluştu.");
                throw;
            }
        }

        public async Task<List<ResultProductByUserEmailWithCategoryNameDto>> TGetAllByUserEmailWithCategoryName()
        {
            try
            {
                var products = await _productDal.GetAllByUserEmailWithCategoryName();
                _logger.Information("Ürün listesi başarıyla getirildi. Toplam Ürün Sayısı: {Count}", products.Count);
                return products;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ürün listesi getirme sırasında bir hata oluştu.");
                throw;
            }
        }
    }
}
