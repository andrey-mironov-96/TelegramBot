using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using app.business.Abstract;
using Microsoft.Extensions.Logging;

namespace app.business.Services
{
    public class FacultyBusinessService : IFacultyBusinessService
    {
        private readonly ILogger<FacultyBusinessService> logger;
        private readonly IFacultyRepository fRepository;
        private readonly IStateService stateService;

        public FacultyBusinessService(
            ILogger<FacultyBusinessService> logger, 
            IFacultyRepository fRepository,
            IStateService stateService)
        {
            this.logger = logger;
            this.fRepository = fRepository;
            this.stateService = stateService;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                await this.fRepository.DeleteAsync(id);
                await this.stateService.RemoveKey(this.stateService.facultyKey);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public Task<FacultyDTO> GetByIdAsync(long id)
        {
            try
            {
                return this.fRepository.GetAsync(id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public Task<IEnumerable<FacultyDTO>> GetFacultiesAsync()
        {
            try
            {
                return fRepository.GetAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public Task<FacultyDTO> GetFacultyAsync(long id)
        {
            return this.GetByIdAsync(id);
        }

        public Task<PageableData<FacultyDTO>> GetPage(PageableData<FacultyDTO> data)
        {
            try
            {
                return fRepository.GetPage(data);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<FacultyDTO> SaveAsync(FacultyDTO item)
        {
            try
            {
                FacultyDTO faculty = await this.fRepository.SaveAsync(item);
                await this.stateService.RemoveKey(this.stateService.facultyKey);
                return faculty;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

    }
}