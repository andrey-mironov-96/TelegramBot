using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using app.domain.Data.Configuration;
using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace app.domain.Services
{
    public class TestRepository : ITestRepository
    {

        private readonly AppDbContext dbContext;

        public TestRepository(
            AppDbContext dbContext
            )
        {

            this.dbContext = dbContext;
        }
        public async Task<bool> DeleteAsync(long id)
        {
            await this.dbContext.Tests
                 .Where(test => test.Id == id && test.IsDeleted == false)
                 .ExecuteUpdateAsync(_ =>
                     _.SetProperty(prop => prop.IsDeleted, true)
                  );

            await this.dbContext.Questions
                .Where(question => question.TestId == id)
                .ExecuteUpdateAsync(_ =>
                _.SetProperty(prop => prop.IsDeleted, true)
                );

            await this.dbContext.Answers
                .Where(answer => answer.Question.TestId == id)
                .ExecuteUpdateAsync(_ =>
                    _.SetProperty(prop => prop.IsDeleted, true)
                );
            await this.dbContext.TestScores
                .Where(ts => ts.IsDeleted == false && ts.Test.Id == id)
                .ExecuteUpdateAsync(_ => _
                    .SetProperty(prop => prop.IsDeleted, true)
                );
            return true;
        }

        public async Task<IEnumerable<TestDTO>> GetAsync()
        {
            List<Test> tests = await this.dbContext.Tests
                .Include(test => test.Questions).ThenInclude(question => question.Answers)
                .Include(test => test.TestScores)
                .AsNoTracking()
                .Where(test => test.IsDeleted == false)
                .ToListAsync();
            return ToDTO(tests);
        }

        public async Task<TestDTO> GetAsync(long id)
        {
            Test test = await this.dbContext.Tests
                .Include(test => test.Questions).ThenInclude(question => question.Answers)
                .Include(test => test.TestScores)
                .AsNoTracking()
                .SingleAsync(test => test.Id == id && test.IsDeleted == false);
            return ToDTO(test);

        }

        public async Task<PageableData<TestDTO>> GetPageableDataAsync(PageableData<TestDTO> data)
        {
            int offset = (data.Page - 1) * data.PageSize;
            List<Test> result = await this.dbContext.Tests
                .Include(test => test.Questions).ThenInclude(question => question.Answers)
                .Include(test => test.TestScores)
                .AsNoTracking()
                .Where(test => !test.IsDeleted)
                .Skip(offset)
                .Take(data.PageSize)
                .ToListAsync();
            data.Total = await this.dbContext.Tests
                .Where(tests => !tests.IsDeleted)
                .CountAsync();
            data.Data = ToDTO(result);
            return data;
        }

        public async Task<List<TestDTO>> Get()
        {
            List<Test> tests = await this.dbContext.Tests.AsNoTracking().ToListAsync();
            return ToDTO(tests);
        }

        public async Task<TestDTO> SaveAsync(TestDTO value)
        {
            Test test = ToDomain(value);
            test.ChangeAt = DateTime.Now;
            if (test.Id == 0)
            {
                test.CreateAt = DateTime.Now;
                test.IsDeleted = false;
                this.dbContext.Tests.Add(test);
            }
            else
            {
                this.dbContext.Tests.Entry(test).State = EntityState.Modified;
                if (test.Questions != null)
                {
                    foreach (Question question in test.Questions)
                    {
                        question.ChangeAt = DateTime.Now;
                        if (question.Id == 0)
                        {
                            question.CreateAt = DateTime.Now;
                            question.IsDeleted = false;
                            this.dbContext.Questions.Entry(question).State = EntityState.Added;
                        }
                        else
                        {
                            this.dbContext.Questions.Entry(question).State = EntityState.Modified;
                        }
                        if (question.Answers != null)
                        {
                            foreach (Answer answer in question.Answers)
                            {
                                answer.ChangeAt = DateTime.Now;
                                if (answer.Id == 0)
                                {
                                    answer.CreateAt = DateTime.Now;
                                    answer.IsDeleted = false;
                                    this.dbContext.Answers.Entry(answer).State = EntityState.Added;
                                }
                                else
                                {
                                    this.dbContext.Answers.Entry(answer).State = EntityState.Modified;
                                }
                            }
                        }
                    }
                }
                if (test.TestScores != null)
                {
                    foreach (TestScore testScore in test.TestScores)
                    {
                        testScore.ChangeAt = DateTime.Now;
                        if (testScore.Id == 0)
                        {
                            testScore.CreateAt = DateTime.Now;
                            testScore.IsDeleted = false;
                            this.dbContext.TestScores.Entry(testScore).State = EntityState.Added;
                        }
                        else
                        {
                            this.dbContext.TestScores.Entry(testScore).State = EntityState.Modified;
                        }
                    }
                }
            }
            await this.dbContext.SaveChangesAsync();
            return ToDTO(test);
        }

        private Test ToDomain(TestDTO testDTO)
        {
            return new Test
            {
                Id = testDTO.Id,
                ChangeAt = testDTO.ChangeAt,
                CreateAt = testDTO.CreateAt,
                IsDeleted = testDTO.IsDeleted,
                TestScores = testDTO.TestScores?.Select(t => new TestScore
                {
                    ChangeAt = t.ChangeAt,
                    CreateAt = t.CreateAt,
                    From = t.From,
                    To = t.To,
                    Id = t.Id,
                    IsDeleted = t.IsDeleted,
                    TargetId = t.TargetId,
                    TargetType = t.TargetType,
                    TestId = t.TestId,
                    Text = t.Text
                }).ToList(),
                Questions = testDTO.Questions?.Select(q => new Question
                {
                    Id = q.Id,
                    ChangeAt = q.ChangeAt,
                    CreateAt = q.CreateAt,
                    IsDeleted = q.IsDeleted,
                    TestId = q.TestId,
                    Title = q.Title,
                    Position = q.Position,
                    Answers = q.Answers?.Select(a => new Answer
                    {
                        ChangeAt = a.ChangeAt,
                        CreateAt = a.CreateAt,
                        IsDeleted = a.IsDeleted,
                        Id = a.Id,
                        Point = a.Point,
                        QuestionId = a.QuestionId,
                        Text = a.Text,
                    }).ToList()
                }).ToList(),
                Title = testDTO.Title
            };
        }

        private List<Test> ToDomain(List<TestDTO> testDTOs)
        {
            return testDTOs.Select(s => ToDomain(s)).ToList();
        }

        private TestDTO ToDTO(Test test)
        {
            return new TestDTO
            {
                Id = test.Id,
                ChangeAt = test.ChangeAt,
                CreateAt = test.CreateAt,
                IsDeleted = test.IsDeleted,
                TestScores = test.TestScores?.Select(t => new TestScoreDTO
                {
                    ChangeAt = t.ChangeAt,
                    CreateAt = t.CreateAt,
                    From = t.From,
                    To = t.To,
                    Id = t.Id,
                    IsDeleted = t.IsDeleted,
                    TargetType = t.TargetType,
                    TargetId = t.TargetId,
                    TestId = t.TestId,
                    Text = t.Text,

                }).ToList(),
                Questions = test.Questions?.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    ChangeAt = q.ChangeAt,
                    CreateAt = q.CreateAt,
                    IsDeleted = q.IsDeleted,
                    TestId = q.TestId,
                    Position = q.Position,
                    Title = q.Title,
                    Answers = q.Answers?.Select(a => new AnswerDTO
                    {
                        ChangeAt = a.ChangeAt,
                        CreateAt = a.CreateAt,
                        IsDeleted = a.IsDeleted,
                        Id = a.Id,
                        Point = a.Point,
                        QuestionId = a.QuestionId,
                        Text = a.Text,
                    }).ToList()
                }).ToList(),
                Title = test.Title
            };
        }

        private List<TestDTO> ToDTO(List<Test> tests)
        {
            return tests.Select(s => ToDTO(s)).ToList();
        }
    }
}
