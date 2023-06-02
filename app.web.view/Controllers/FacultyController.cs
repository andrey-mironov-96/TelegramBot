using app.common.DTO;
using app.common.Utils;
using app.business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace app.web.view.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : ABaseController<FacultyController, IFacultyBusinessService, FacultyDTO>
    {

        public FacultyController(ILogger<FacultyController> logger, IFacultyBusinessService businessService) : base(logger, businessService)
        {

        }
        
        [HttpGet, Route("get-all")]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetAllAsync()
        {
            try
            {
              return Ok(await this.businessService.GetFacultiesAsync());
            }
            catch(Exception e)
            {
                this.logger.LogError(e, e.Message);
                return BadRequest();
            }
        }
    }
}