using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using app.business.Abstract;
using Microsoft.Extensions.Logging;

namespace app.business.Services
{
    public class SpecialityBusinessService : ISpecialityBusinessService
    {
        private readonly ILogger<SpecialityBusinessService> logger;
        private readonly ISpecialityRepository repository;
        private readonly IStateService stateService;

        public SpecialityBusinessService(
            ILogger<SpecialityBusinessService> logger,
            ISpecialityRepository repository,
            IStateService stateService
            )
        {
            this.logger = logger;
            this.repository = repository;
            this.stateService = stateService;
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

        public async Task<SpecialtyDTO> SaveAsync(SpecialtyDTO item)
        {
            try
            {
                SpecialtyDTO specialty = await this.repository.SaveAsync(item);
                await stateService.RemoveKey(stateService.facultyKey);
                return specialty;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
        public Task<IEnumerable<SpecialtyDTO>> GetSpecialtiesAsync()
        {
            try
            {
                return this.repository.GetAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}