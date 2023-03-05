using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.common.DTO;
using app.domain.Abstract;
using app.test.core.Utils;

namespace app.domain.test.Services
{
    public class BotServiceTest : ABaseTest<IBotService>
    {
        public override IBotService GetCurrentService()
        {
            return this.GetBotService();
        }

        [Fact]
        public async Task InitFacultyTest()
        {
            IBotService service = GetCurrentService();
            long userId = 111111111;
            await service.InitFacultyAsync(userId);
        }

        [Fact]
        public async Task DoWork_ChooseFaculty_Test()
        {
            IBotService service = GetCurrentService();
            long userId = 111111111;
            string message = "Факультет математики, информационных и авиационных технологий";
            MessageDTO messageDTO = await service.DoWork(userId, message);
            
            Assert.NotNull(messageDTO);
            Assert.True(messageDTO.Message.Count() > 0);
        }

        [Fact]
        public async Task DoWork_ChooseSpeciality_Test()
        {
            IBotService service = GetCurrentService();
            long userId = 111111111;
            string message = "01.03.02 Прикладная математика и информатика";
            MessageDTO messageDTO = await service.DoWork(userId, message);
            
            Assert.NotNull(messageDTO);
            Assert.True(messageDTO.Message.Count() > 0);
        }

        [Fact]
        public async Task InitProfTest_Test()
        {
            IBotService service = GetCurrentService();
            long userId = 111111111;
            await service.InitProfAsync(userId);
        }

    }
}