using app.common.DTO;
using app.common.Utils;
using BusinesDAL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace app.web.view.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : ABaseController<FacultyController, IFacultyBusinessService, FacultyDTO>
    {
        private readonly ILogger<FacultyController> logger;
        private readonly IFacultyBusinessService businessService;

        public FacultyController(ILogger<FacultyController> logger, IFacultyBusinessService businessService) : base(logger, businessService)
        {
            this.logger = logger;
            this.businessService = businessService;
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