using app.common.DTO;
using app.common.Utils;
using BusinesDAL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace app.web.view.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ABaseController<QuestionController, IQuestionBusinessService, QuestionDTO>
    {
        
        public QuestionController(ILogger<QuestionController> logger, IQuestionBusinessService businessService) : base(logger, businessService)
        {
        }

        [HttpGet, Route("next-position")]
        public IActionResult GetNextPosition(long testId)
        {
            int nextPosition = this.businessService.GetNextQuestionPosition(testId);
            return Ok(nextPosition);
        }
        
    }
}