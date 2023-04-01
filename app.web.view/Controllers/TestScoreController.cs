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
         private readonly ILogger<TestScoreController> logger;
        private readonly ITestScoreBusinessService businessService;
        public TestScoreController(ILogger<TestScoreController> logger, ITestScoreBusinessService businessService) : base(logger, businessService)
        {
            this.logger = logger;
            this.businessService = businessService;
        }
    }
}