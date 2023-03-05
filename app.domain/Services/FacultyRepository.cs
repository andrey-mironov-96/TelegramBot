using app.common.DTO;
using app.common.Utils;
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
                    CreateAt = dto.CreateAt,
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
                    CreateAt = domain.CreateAt,
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
                faculty.ChangeAt = DateTime.Now;
                _dbContext.Entry<Faculty>(faculty).State = EntityState.Modified;
                if (faculty.Specialities != null && faculty.Specialities.Count > 0)
                {
                    foreach (Specialty specialty in faculty.Specialities)
                    {
                        _dbContext.Entry(specialty).State = EntityState.Modified;
                    }
                }
            }
            else
            {
                faculty.CreateAt = DateTime.Now;
                _dbContext.Faculties.Add(faculty);
            }
            await _dbContext.SaveChangesAsync();
            return ToDTO(faculty);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _dbContext.Faculties.Where(faculty => faculty.Id == id)
                .ExecuteUpdateAsync(_ => _
                    .SetProperty(prop => prop.IsDeleted, true));

            await _dbContext.Specialies.Where(speciality => speciality.FacultyId == id)
                .ExecuteUpdateAsync(_ => _
                    .SetProperty(prop => prop.IsDeleted, true));


            return true;
        }

        public async Task<PageableData<FacultyDTO>> GetPage(PageableData<FacultyDTO> data)
        {
            int offset = (data.Page - 1) * data.PageSize;
            List<Faculty> result = await _dbContext.Faculties
                .Include(faculty => faculty.Specialities)
                .Where(faculty => !faculty.IsDeleted)
                .Skip(offset)
                .Take(data.PageSize)
                .ToListAsync();
            data.Total = await _dbContext.Faculties
                .Include(faculty => faculty.Specialities)
                .Where(faculty => !faculty.IsDeleted)
                .CountAsync();
            data.Data = ToDTO(result);
            return data;
        }
    }
}