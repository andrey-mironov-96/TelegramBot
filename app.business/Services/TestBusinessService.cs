using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using app.business.Abstract;
using Microsoft.Extensions.Logging;

namespace app.business.Services
{
    public class TestBusinessService : ITestBusinessService
    {
        private readonly ILogger<TestBusinessService> logger;
        private readonly ITestRepository repository;

        public TestBusinessService(ILogger<TestBusinessService> logger, ITestRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }
        public Task<bool> DeleteAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException($"Doesn't delete test becouse id <= 0 {id}");
                }
                return repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public Task<TestDTO> GetByIdAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException($"Doesn't getting test by id becouse id <= 0 {id}");
                }
                return repository.GetAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public Task<PageableData<TestDTO>> GetPage(PageableData<TestDTO> data)
        {
            try
            {
                if (data == null)
                {
                    throw new ArgumentException("Doesn't getting page with tests becouse pageabeleData equal null");
                }
                return repository.GetPageableDataAsync(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public Task<TestDTO> SaveAsync(TestDTO item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentException("Doesn't saving test becouse test equal null");
                }
                if (item.Id < 0)
                {
                    throw new ArgumentException($"Doesn't saving test becouse id < 0 {item.Id}");
                }
                return repository.SaveAsync(item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
