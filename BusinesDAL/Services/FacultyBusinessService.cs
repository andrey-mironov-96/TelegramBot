using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.common.DTO;
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
        public Task<IEnumerable<FacultyDTO>> GetFacultiesAsync()
        {
            try
            {
               return fRepository.GetAsync();
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public Task<FacultyDTO> GetFacultyAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<FacultyDTO> SaveAsync(FacultyDTO faculty)
        {
            throw new NotImplementedException();
        }
    }
}