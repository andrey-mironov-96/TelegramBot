using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using app.business.Abstract;
using Microsoft.Extensions.Logging;

namespace app.business.Services
{
    public class TestScoreBusinessService : ITestScoreBusinessService
    {
        private readonly ILogger<TestScoreBusinessService> logger;
        private readonly ITestScoreRepository repository;

        public TestScoreBusinessService(
            ILogger<TestScoreBusinessService> logger,
            ITestScoreRepository repository
            )
        {
            this.logger = logger;
            this.repository = repository;
        }
         public Task<bool> DeleteAsync(long id)
        {
            try
            {
                return this.repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public  Task<TestScoreDTO> GetByIdAsync(long id)
        {
            try
            {
                return this.repository.GetAsync(id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public Task<PageableData<TestScoreDTO>> GetPage(PageableData<TestScoreDTO> data)
        {
            try
            {
                return this.repository.GetPage(data);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<TestScoreDTO> SaveAsync(TestScoreDTO item)
        {
            try
            {
                TestScoreDTO specialty = await this.repository.SaveAsync(item);
                return specialty;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}