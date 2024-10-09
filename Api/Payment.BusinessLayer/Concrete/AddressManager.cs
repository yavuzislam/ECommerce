using Payment.BusinessLayer.Abstract;
using Payment.DataAccessLayer.Abstract;
using Payment.EntityLayer.Concrete;
using Serilog;

namespace Payment.BusinessLayer.Concrete;

public class AddressManager : IAddressService
{
    private readonly IAddressDal _addressDal;
    private readonly ILogger _logger;

    public AddressManager(IAddressDal addressDal)
    {
        _addressDal = addressDal;
        _logger = Log.ForContext<AddressManager>();
    }

    public async Task DeleteAsync(Address t)
    {
        if (t == null)
        {
            _logger.Warning("Adres nesnesi null olamaz.");
            throw new ArgumentNullException(nameof(t), "Adres nesnesi null olamaz.");
        }

        await _addressDal.DeleteAsync(t);
        _logger.Information("Adres başarıyla silindi. Adres ID: {AddressId}", t.Id);
    }

    public async Task<Address> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.Warning("Geçersiz adres ID sağlandı. ID: {AddressId}", id);
            throw new ArgumentOutOfRangeException(nameof(id), "Geçersiz adres ID.");
        }

        var address = await _addressDal.GetByIdAsync(id);
        if (address == null)
        {
            _logger.Warning("Adres bulunamadı. ID: {AddressId}", id);
            throw new NullReferenceException("Adres bulunamadı.");
        }

        _logger.Information("Adres başarıyla getirildi. Adres ID: {AddressId}", id);
        return address;
    }

    public async Task<List<Address>> GetListAsync()
    {
        //var addresses = await _addressDal.GetListAsync();
        var addresses = await _addressDal.GetListAsync();
        _logger.Information("Adresler başarıyla getirildi. Toplam adres sayısı: {AddressCount}", addresses.Count);
        return addresses;
    }

    public async Task InsertAsync(Address t)
    {
        if (t == null)
        {
            _logger.Warning("Adres nesnesi null olamaz.");
            throw new ArgumentNullException(nameof(t), "Adres nesnesi null olamaz.");
        }
        await _addressDal.InsertAsync(t);
    }

    public async Task UpdateAsync(Address t)
    {
        if (t == null)
        {
            _logger.Warning("Adres nesnesi null olamaz.");
            throw new ArgumentNullException(nameof(t), "Adres nesnesi null olamaz.");
        }

        await _addressDal.UpdateAsync(t);
        _logger.Information("Adres başarıyla güncellendi. Adres ID: {AddressId}", t.Id);
    }
}
