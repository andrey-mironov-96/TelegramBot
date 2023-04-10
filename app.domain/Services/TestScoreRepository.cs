using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using app.domain.Data.Configuration;
using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace app.domain.Services
{
    public class TestScoreRepository : ITestScoreRepository
    {
        private readonly ILogger<TestScoreRepository> logger;
        private readonly AppDbContext database;

        public TestScoreRepository(
            ILogger<TestScoreRepository> logger,
            AppDbContext database)
        {
            this.logger = logger;
            this.database = database;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await database.TestScores.Where(test => test.Id == id)
                .ExecuteUpdateAsync(_ => _
                    .SetProperty(prop => prop.IsDeleted, true));
            return true;
        }

        public async Task<IEnumerable<TestScoreDTO>> GetAsync()
        {
            return ToDTO(await database.TestScores.AsNoTracking().ToListAsync());
        }

        public async Task<TestScoreDTO> GetAsync(long id)
        {
            return ToDTO(await database.TestScores.AsNoTracking().SingleOrDefaultAsync(spec => spec.Id == id));
        }

        public IEnumerable<TestScoreDTO> GetByTestId(long testId)
        {
            List<TestScore> tests = database.TestScores.AsNoTracking().Where(test => test.TestId == testId && test.IsDeleted == false).ToList();
            return ToDTO(tests);
        }

        public async Task<TestScoreDTO> SaveAsync(TestScoreDTO value)
        {
            TestScore testScore = ToDomain(value);
            if (value.Id == 0)
            {
                value.CreateAt = DateTime.Now;
                this.database.TestScores.Add(testScore);
            }
            else
            {
                value.ChangeAt = DateTime.Now;
                this.database.Entry(testScore).State = EntityState.Modified;
            }
            await this.database.SaveChangesAsync();
            return ToDTO(testScore);
        }

        public async Task<PageableData<TestScoreDTO>> GetPage(PageableData<TestScoreDTO> data)
        {
            int offset = (data.Page - 1) * data.PageSize;
            IQueryable<TestScore> query = database.TestScores
                .Where(test => !test.IsDeleted);
            query = AddFilters(query, data.Filters);
            List<TestScore> result = await query.Skip(offset)
                .Take(data.PageSize)
                .ToListAsync();

            
            IQueryable<TestScore> queryTotal = database.TestScores
                .Where(speciality => !speciality.IsDeleted);
            queryTotal = AddFilters(queryTotal, data.Filters);
            data.Total = await queryTotal.CountAsync();
            data.Data = ToDTO(result);
            return data;
        }

        private TestScoreDTO ToDTO(TestScore domain)
        {
            if (domain == null)
            {
                return null;
            }
            return new TestScoreDTO()
            {
                From = domain.From,
                TargetId = domain.TargetId,
                TargetType = domain.TargetType,
                TestId = domain.TestId,
                Text = domain.Text,
                To = domain.To,
                ChangeAt = domain.ChangeAt,
                CreateAt = domain.CreateAt,
                Id = domain.Id
            };
        }

        private TestScore ToDomain(TestScoreDTO dto)
        {
            if (dto == null)
            {
                return null;
            }
            return new TestScore()
            {
                From = dto.From,
                TargetId = dto.TargetId,
                TargetType = dto.TargetType,
                TestId = dto.TestId,
                Text = dto.Text,
                To = dto.To,
                ChangeAt = dto.ChangeAt,
                CreateAt = dto.CreateAt,
                Id = dto.Id
            };
        }

        private IEnumerable<TestScore> ToDomain(IEnumerable<TestScoreDTO> dtos)
        {
            return dtos.Select(test => ToDomain(test));
        }

        private IEnumerable<TestScoreDTO> ToDTO(IEnumerable<TestScore> dtos)
        {
            return dtos.Select(test => ToDTO(test));
        }

        private IQueryable<TestScore> AddFilters(IQueryable<TestScore> query, Filter[] filters)
        {
            if (filters != null)
            {
                foreach (Filter filter in filters)
                {
                    switch (filter.Field)
                    {
                        case "testId":
                            query = query.Where(q => q.TestId == Int32.Parse(filter.Value));
                            break;
                    }
                }
            }
            return query;
        }
    }
}