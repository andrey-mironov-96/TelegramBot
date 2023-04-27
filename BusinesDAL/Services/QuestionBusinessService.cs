using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using BusinesDAL.Abstract;
using Microsoft.Extensions.Logging;

namespace BusinesDAL.Services
{
    public class QuestionBusinessService : IQuestionBusinessService
    {

        private readonly ILogger<QuestionBusinessService> logger;
        private readonly IQuestionRepository repository;

        public QuestionBusinessService(
            ILogger<QuestionBusinessService> logger,
            IQuestionRepository repository
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

        public Task<QuestionDTO> GetByIdAsync(long id)
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

        public Task<PageableData<QuestionDTO>> GetPage(PageableData<QuestionDTO> data)
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

        public int GetNextQuestionPosition(long testId)
        {
            try
            {
                return this.repository.GetNextQuestionPosition(testId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<QuestionDTO> SaveAsync(QuestionDTO item)
        {
             try
            {
                QuestionDTO question = await this.repository.SaveAsync(item);
                return question;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}