namespace app.domain.Abstract
{
    public interface IRepository<TEntity, DTOEntity>
    {
        public Task<IEnumerable<DTOEntity>> GetAsync();

        public Task<DTOEntity> GetAsync(long id);

        public Task<DTOEntity> SaveAsync(DTOEntity value);

        public Task<bool> DeleteAsync(long id);
        
    }
}