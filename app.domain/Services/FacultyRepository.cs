using app.common.DTO;
using app.domain.Abstract;
using app.domain.Data.Configuration;
using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace app.domain.Services
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly ILogger<FacultyRepository> _logger;
        private readonly AppDbContext _dbContext;

        public FacultyRepository(ILogger<FacultyRepository> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public Task<IEnumerable<FacultyDTO>> GetAsync()
            => Task.FromResult(ToDTO(_dbContext.Faculties
                    .AsNoTracking()
                    .Include(fac => fac.Specialities)
                    .ToList()
                ));

        public Task<FacultyDTO> GetAsync(long id)
        => Task.FromResult(ToDTO(_dbContext.Faculties
                    .AsNoTracking()
                    .Include(fac => fac.Specialities)
                    .Single(fac => fac.Id == id)
                ));

        public static FacultyDTO ToDTO(Faculty domain)
        {
            return new FacultyDTO
            {
                Id = domain.Id,
                Name = domain.Name
            };
        }

        public static IEnumerable<FacultyDTO> ToDTO(IEnumerable<Faculty> domains)
            => domains.Select(dto =>
                new FacultyDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Specialities = dto.Specialities.Select(s =>
                    new SpecialtyDTO
                    {
                        CreateAt = s.CreateAt,
                        EducationType = s.EducationType,
                        ExtrabudgetaryPlaces = s.ExtrabudgetaryPlaces,
                        FacultyId = s.FacultyId,
                        GeneralCompetition = s.GeneralCompetition,
                        Id = s.Id,
                        Name = s.Name,
                        QuotaLOP = s.QuotaLOP,
                        SpecialQuota = s.SpecialQuota,
                        TargetAdmissionQuota = s.TargetAdmissionQuota
                    }).ToList()
                });

        public static Faculty ToDomain(FacultyDTO dto)
        {
            return new Faculty
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public static IEnumerable<Faculty> ToDomain(IEnumerable<FacultyDTO> dtos)
            => dtos.Select(domain =>
                new Faculty
                {
                    Id = domain.Id,
                    Name = domain.Name,
                    Specialities = domain.Specialities.Select(s =>
                    new Specialty
                    {
                        CreateAt = s.CreateAt,
                        EducationType = s.EducationType,
                        ExtrabudgetaryPlaces = s.ExtrabudgetaryPlaces,
                        FacultyId = s.FacultyId,
                        GeneralCompetition = s.GeneralCompetition,
                        Id = s.Id,
                        Name = s.Name,
                        QuotaLOP = s.QuotaLOP,
                        SpecialQuota = s.SpecialQuota,
                        TargetAdmissionQuota = s.TargetAdmissionQuota
                    }).ToList()
                });

        public async Task<FacultyDTO> SaveAsync(FacultyDTO value)
        {
            Faculty faculty = ToDomain(value); ;
            faculty.Specialities = null;
            if (value.Id > 0)
            {
                _dbContext.Entry<Faculty>(faculty).State = EntityState.Modified;
            }
            else
            {
                _dbContext.Faculties.Add(faculty);
            }
            await _dbContext.SaveChangesAsync();
            return ToDTO(faculty);
        }
    }
}