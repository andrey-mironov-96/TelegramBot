using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using app.domain.Data.Configuration;
using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace app.domain.Services
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ILogger<QuestionRepository> logger;
        private readonly AppDbContext database;

        public QuestionRepository(
            ILogger<QuestionRepository> logger,
            AppDbContext database)
        {
            this.logger = logger;
            this.database = database;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await database.Questions.Where(q => q.Id == id)
                .ExecuteUpdateAsync(_ => _
                    .SetProperty(prop => prop.IsDeleted, true));
            return true;
        }

        public async Task<IEnumerable<QuestionDTO>> GetAsync()
        {
            return ToDTO(await database.Questions.AsNoTracking().ToListAsync());
        }

        public async Task<QuestionDTO> GetAsync(long id)
        {
            return ToDTO(await database.Questions.AsNoTracking().SingleOrDefaultAsync(spec => spec.Id == id));
        }

        public async Task<QuestionDTO> SaveAsync(QuestionDTO value)
        {
            Question question = ToDomain(value);
            if (value.Id == 0)
            {
                value.CreateAt = DateTime.Now;
                this.database.Questions.Add(question);
            }
            else
            {
                value.ChangeAt = DateTime.Now;
                this.database.Entry(question).State = EntityState.Modified;
                if (question.Answers != null)
                {
                    var answersToRemove = this.database.Answers.Where(w => w.QuestionId == question.Id && w.IsDeleted == false && !question.Answers.Select(a => a.Id).Contains(w.Id))
                        .ExecuteUpdate(_ => _
                            .SetProperty(prop => prop.IsDeleted, true));
                    foreach (Answer answer in question.Answers)
                    {
                        answer.ChangeAt = DateTime.Now;
                        if (answer.Id == 0)
                        {
                            answer.CreateAt = DateTime.Now;
                            answer.IsDeleted = false;
                            this.database.Answers.Entry(answer).State = EntityState.Added;
                        }
                        else
                        {
                            this.database.Answers.Entry(answer).State = EntityState.Modified;
                        }
                    }
                }
            }
            await this.database.SaveChangesAsync();
            return ToDTO(question);
        }

        public int GetNextQuestionPosition(long testId)
        {
            Question[] questions = this.database.Questions.AsNoTracking().Where(q => q.IsDeleted == false && q.TestId == testId).ToArray();
            int currentPosition = questions.Count();
            if (currentPosition == 0)
            {
                return 0;
            }
            return currentPosition++;
        }

        private QuestionDTO ToDTO(Question domain)
        {
            if (domain == null)
            {
                return null;
            }
            return new QuestionDTO()
            {
                Position = domain.Position,
                Title = domain.Title,
                Answers = domain.Answers != null ? domain.Answers.Select(a => new AnswerDTO
                {
                    ChangeAt = a.ChangeAt,
                    CreateAt = a.CreateAt,
                    IsDeleted = a.IsDeleted,
                    Id = a.Id,
                    Point = a.Point,
                    QuestionId = a.QuestionId,
                    Text = a.Text,
                }).ToList() : new List<AnswerDTO>(),
                TestId = domain.TestId,
                ChangeAt = domain.ChangeAt,
                CreateAt = domain.CreateAt,
                Id = domain.Id
            };
        }

        private Question ToDomain(QuestionDTO dto)
        {
            if (dto == null)
            {
                return null;
            }
            return new Question()
            {
                Position = dto.Position,
                Title = dto.Title,
                Answers = dto.Answers?.Select(a => new Answer
                {
                    ChangeAt = a.ChangeAt,
                    CreateAt = a.CreateAt,
                    IsDeleted = a.IsDeleted,
                    Id = a.Id,
                    Point = a.Point,
                    QuestionId = a.QuestionId,
                    Text = a.Text,
                }).ToList(),
                TestId = dto.TestId,
                ChangeAt = dto.ChangeAt,
                CreateAt = dto.CreateAt,
                Id = dto.Id
            };
        }

        private IEnumerable<Question> ToDomain(IEnumerable<QuestionDTO> dtos)
        {
            return dtos.Select(q => ToDomain(q));
        }

        private IEnumerable<QuestionDTO> ToDTO(IEnumerable<Question> dtos)
        {
            return dtos.Select(q => ToDTO(q));
        }

        public async Task<PageableData<QuestionDTO>> GetPage(PageableData<QuestionDTO> data)
        {
            int offset = (data.Page - 1) * data.PageSize;
            IQueryable<Question> query = database.Questions
                .Include(q => q.Answers.Where(a => !a.IsDeleted).OrderBy(a => a.Point))
                .Where(q => !q.IsDeleted)
                .OrderBy(q => q.Position);
            query = AddFilters(query, data.Filters);
            query = query.Skip(offset)
                .Take(data.PageSize);
            List<Question> result = await query.ToListAsync();
            IQueryable<Question> queryTotal = database.Questions.AsNoTracking().Where(speciality => !speciality.IsDeleted);
            queryTotal = AddFilters(queryTotal, data.Filters);
            data.Total = await queryTotal.CountAsync();
            data.Data = ToDTO(result);
            return data;
        }

        public IEnumerable<QuestionDTO> GetQuestionsOfTest(long testId)
        {
            List<Question> questions = this.database.Questions
            .Include(question => question.Answers.Where(a => !a.IsDeleted).OrderBy(a => a.Point))
            .AsNoTracking().Where(question => question.IsDeleted == false && question.TestId == testId).ToList();
            return ToDTO(questions);
        }

        private IQueryable<Question> AddFilters(IQueryable<Question> query, Filter[] filters)
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