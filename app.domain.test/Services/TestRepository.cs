using app.common.DTO;
using app.common.Utils;
using app.domain.Abstract;
using app.test.core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace app.domain.test.Services
{
    public class TestRepository : ABaseTest<ITestRepository>
    {
        public override ITestRepository GetCurrentService()
        {
            return this.GetTestRepository();
        }

        [Fact]
        public async void SaveAsync_Test_Create()
        {
            ITestRepository testRepository = this.GetCurrentService();
            TestDTO value = new TestDTO
            {
                Id = 0,
                Title = "Тест на проф. ориентацию",
                Questions = new List<QuestionDTO>
                {
                    new QuestionDTO() {
                        Id = 0,
                        TestId = 0,
                        Title =  "Я предпочту заниматься финансовыми операциями, а не, например, музыкой.",
                        Position = 1,
                        Answers = new List<AnswerDTO>
                        {
                            new AnswerDTO{Id = 0, QuestionId = 0, Text = "Да", Point = 1},
                            new AnswerDTO{Id = 0, QuestionId = 0, Text = "Затрудняюсь ответить", Point = 2},
                            new AnswerDTO{Id = 0, QuestionId = 0, Text = "Нет", Point = 3},
                        }

                    }
                }

            };
            TestDTO value2 = await testRepository.SaveAsync(value);
            Assert.NotNull(value2);
            Assert.True(value2.Id > 0);
            Assert.True(value2.Questions.Count > 0);
            Assert.True(value2.Questions[0].Answers.Count > 0);
        }

        [Fact]
        public async void GetAsync_Test_ById()
        {
            ITestRepository testRepository = this.GetCurrentService();
            long id = 1;
            TestDTO value = await testRepository.GetAsync(id);

            Assert.NotNull(value);
            Assert.True(value.Id > 0);
            Assert.True(value.Questions.Count > 0);
            Assert.True(value.Questions[0].Answers.Count > 0);
        }

        [Fact]
        public async void GetAsync_Test_All()
        {
            ITestRepository testRepository = this.GetCurrentService();
            IEnumerable<TestDTO> values = await testRepository.GetAsync();

            Assert.NotNull(values);
            Assert.True(values.Count() > 0);
            Assert.Contains(values, f => f.Id > 0);

        }

        [Fact]
        public async void SaveAsync_Test_Update()
        {
            ITestRepository testRepository = this.GetCurrentService();
            long id = 1;
            string phrase = "_change";
            TestDTO value = await testRepository.GetAsync(id);
            value.Title += phrase;
            value.Questions.ForEach(q =>
            {
                q.Title += phrase;
                q.Answers.ForEach(a =>
                {
                    a.Text += phrase;
                });
            });
            value.TestScores = new List<TestScoreDTO>()
            {
                new TestScoreDTO(){From = 0, To = 20, Id = 0, TargetId = 0, TargetType = common.Utils.Enums.TargetType.None, TestId = id, Text = "Вам больше подходит это"}
            };
            TestDTO value2 = await testRepository.SaveAsync(value);
            Assert.NotNull(value2);
            Assert.True(value2.Id > 0);
            Assert.True(value2.Questions.Count > 0);
            Assert.True(value2.Questions[0].Answers.Count > 0);
            Assert.True(value2.Title.EndsWith(phrase) == true);
            Assert.Contains(value2.Questions, f => f.Title.EndsWith(phrase));
            Assert.Contains(value2.Questions[0].Answers, f => f.Text.EndsWith(phrase));
        }

        [Fact]
        public async void DeleteAsync_Test()
        {
            ITestRepository testRepository = this.GetCurrentService();
            long id = 1;
            bool result = await testRepository.DeleteAsync(id);

            Assert.True(result);
        }

        [Fact]
        public async void GetPageableDataAsync_Test()
        {
            ITestRepository testRepository = this.GetCurrentService();
            PageableData<TestDTO> pageableData = new PageableData<TestDTO>(1, 10, 20);
            PageableData<TestDTO> result = await testRepository.GetPageableDataAsync(pageableData);

            Assert.NotNull(result);
        }
    }
}
