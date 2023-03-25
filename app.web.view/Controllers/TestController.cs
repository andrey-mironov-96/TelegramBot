using app.common.DTO;
using app.common.Utils;
using BusinesDAL.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app.web.view.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ABaseController<TestController, ITestBusinessService, TestDTO>
    {
        public TestController(ILogger<TestController> logger, ITestBusinessService businessService) : base(logger, businessService)
        {
        }
    }
}
