namespace PartyRoom.Domain.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<bool> CreateAsync(T createModel);
        Task<bool> UpdateAsync(T updateModel);
        Task<bool> DeleteAsync(T deleteModel);
        Task<T> GetByIdAsync(Guid id);
        Task<IQueryable<T>> GetAllAsync();
        /// <summary>
        /// Асинхронно сохраняет изменения в базу данных через контекст DbContext.
        /// </summary>
        Task<bool> SaveAsync();
        Task<bool> ExistsIdAsync(Guid id);
    }
}
