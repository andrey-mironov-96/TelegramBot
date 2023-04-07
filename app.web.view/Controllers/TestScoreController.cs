using app.common.DTO;
using app.common.Utils;
using BusinesDAL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace app.web.view.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestScoreController : ABaseController<TestScoreController, ITestScoreBusinessService, TestScoreDTO>
    {

        public TestScoreController(ILogger<TestScoreController> logger, ITestScoreBusinessService businessService) : base(logger, businessService)
        {
        }
    }
}