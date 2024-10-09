namespace Payment.DataAccessLayer.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        Task InsertAsync(T t);
        Task DeleteAsync(T t);
        Task UpdateAsync(T t);
        Task<List<T>> GetListAsync();
        Task<T> GetByIdAsync(int id);
    }
}
