using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.common.DTO;
using app.domain.Abstract;
using app.test.core.Utils;

namespace app.domain.test.Services
{
    public class FacultyRepository : ABaseTest<IFacultyRepository>
    {
        public override IFacultyRepository GetCurrentService()
        {
            return this.GetFacultyRepository();
        }

        [Fact]
        public async void GetAsyncTest()
        {
            IFacultyRepository service = this.GetCurrentService();
            IEnumerable<FacultyDTO> result = await service.GetAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }

        [Fact]
        public async void GetAsyncWithIdTest()
        {
            IFacultyRepository service = this.GetCurrentService();
            long facultyId = 1;
            FacultyDTO result = await service.GetAsync(facultyId);
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(result.Id, facultyId);
        }
    }
}