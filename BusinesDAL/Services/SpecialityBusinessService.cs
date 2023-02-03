using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using BusinesDAL.Abstract;
using Microsoft.Extensions.Logging;

namespace BusinesDAL.Services
{
    public class SpecialityBusinessService : ISpecialityBusinessService
    {
        private readonly ILogger<SpecialityBusinessService> logger;
        private readonly ISpecialityRepository repository;

        public SpecialityBusinessService(
            ILogger<SpecialityBusinessService> logger,
            ISpecialityRepository repository
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

        public Task<SpecialtyDTO> GetByIdAsync(long id)
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

        public Task<PageableData<SpecialtyDTO>> GetPage(PageableData<SpecialtyDTO> data)
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

        public Task<PageableData<SpecialtyDTO>> GetPageSpecialityOfFacultyAsync(PageableData<SpecialtyDTO> data, long facultyId)
        {
            try
            {
                return this.repository.GetPageSpecialityOfFacultyAsync(data, facultyId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public Task<SpecialtyDTO> SaveAsync(SpecialtyDTO item)
        {
            try
            {
                return this.repository.SaveAsync(item);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}