using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using BusinesDAL.Abstract;
using Microsoft.Extensions.Logging;

namespace BusinesDAL.Services
{
    public class FacultyBusinessService : IFacultyBusinessService
    {
        private readonly ILogger<FacultyBusinessService> logger;
        private readonly IFacultyRepository fRepository;

        public FacultyBusinessService(ILogger<FacultyBusinessService> logger, IFacultyRepository fRepository)
        {
            this.logger = logger;
            this.fRepository = fRepository;
        }

        public Task<bool> DeleteAsync(long id)
        {
            try
            {
                return this.fRepository.DeleteAsync(id);
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

        public Task<FacultyDTO> SaveAsync(FacultyDTO item)
        {
            try
            {
                return this.fRepository.SaveAsync(item);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

    }
}