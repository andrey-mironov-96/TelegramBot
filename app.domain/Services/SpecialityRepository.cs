using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using app.domain.Data.Configuration;
using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace app.domain.Services
{
    public class SpecialityRepository : ISpecialityRepository
    {
        private readonly ILogger<SpecialityRepository> logger;
        private readonly AppDbContext database;

        public SpecialityRepository(
            ILogger<SpecialityRepository> logger,
            AppDbContext database)
        {
            this.logger = logger;
            this.database = database;
        }
        public async Task<bool> DeleteAsync(long id)
        {
            await database.Specialies.Where(spec => spec.Id == id)
                .ExecuteUpdateAsync(_ => _
                    .SetProperty(prop => prop.IsDeleted, true));
            return true;
        }

        public async Task<IEnumerable<SpecialtyDTO>> GetAsync()
        {
            return ToDTO(await database.Specialies.AsNoTracking().ToListAsync());
        }

        public async Task<SpecialtyDTO> GetAsync(long id)
        {
            return ToDTO(await database.Specialies.AsNoTracking().SingleOrDefaultAsync(spec => spec.Id == id));
        }

        public async Task<SpecialtyDTO> SaveAsync(SpecialtyDTO value)
        {
            Specialty specialty = ToDomain(value);
            if (value.Id == 0)
            {
                value.CreateAt = DateTime.Now;
                this.database.Specialies.Add(specialty);
            }
            else
            {
                value.ChangeAt = DateTime.Now;
                this.database.Entry(specialty).State = EntityState.Modified;
            }
            await this.database.SaveChangesAsync();
            return ToDTO(specialty);
        }

        public async Task<PageableData<SpecialtyDTO>> GetPage(PageableData<SpecialtyDTO> data)
        {
            int offset = (data.Page - 1) * data.PageSize;
            List<Specialty> result = await database.Specialies
                .Where(speciality => !speciality.IsDeleted)
                .Skip(offset)
                .Take(data.PageSize)
                .ToListAsync();
            data.Total = await database.Specialies
            .Where(speciality => !speciality.IsDeleted)
            .CountAsync();
            data.Data = ToDTO(result);
            return data;
        }

        public async Task<PageableData<SpecialtyDTO>> GetPageSpecialityOfFacultyAsync(PageableData<SpecialtyDTO> data, long facultyId)
        {
            int offset = (data.Page - 1) * data.PageSize;
            List<Specialty> result = await database.Specialies.AsNoTracking()
                .Where(spec => spec.FacultyId == facultyId && !spec.IsDeleted)
                .Skip(offset)
                .Take(data.PageSize)
                .ToListAsync();
            data.Total = await database.Specialies.Where(spec => spec.FacultyId == facultyId).CountAsync();
            data.Data = ToDTO(result);
            return data;
        }

        private SpecialtyDTO ToDTO(Specialty domain)
        {
            if (domain == null)
            {
                return null;
            }
            return new SpecialtyDTO()
            {
                ChangeAt = domain.ChangeAt,
                CreateAt = domain.CreateAt,
                EducationType = domain.EducationType,
                ExtrabudgetaryPlaces = domain.ExtrabudgetaryPlaces,
                FacultyId = domain.FacultyId,
                GeneralCompetition = domain.GeneralCompetition,
                Id = domain.Id,
                IsDeleted = domain.IsDeleted,
                Name = domain.Name,
                QuotaLOP = domain.QuotaLOP,
                SpecialQuota = domain.SpecialQuota,
                TargetAdmissionQuota = domain.TargetAdmissionQuota
            };
        }

        private Specialty ToDomain(SpecialtyDTO dto)
        {
            if (dto == null)
            {
                return null;
            }
            return new Specialty()
            {
                ChangeAt = dto.ChangeAt,
                CreateAt = dto.CreateAt,
                EducationType = dto.EducationType,
                ExtrabudgetaryPlaces = dto.ExtrabudgetaryPlaces,
                FacultyId = dto.FacultyId,
                GeneralCompetition = dto.GeneralCompetition,
                Id = dto.Id,
                IsDeleted = dto.IsDeleted,
                Name = dto.Name,
                QuotaLOP = dto.QuotaLOP,
                SpecialQuota = dto.SpecialQuota,
                TargetAdmissionQuota = dto.TargetAdmissionQuota
            };
        }

        private IEnumerable<Specialty> ToDomain(IEnumerable<SpecialtyDTO> dtos)
        {
            return dtos.Select(spec => ToDomain(spec));
        }

        private IEnumerable<SpecialtyDTO> ToDTO(IEnumerable<Specialty> dtos)
        {
            return dtos.Select(spec => ToDTO(spec));
        }
    }
}