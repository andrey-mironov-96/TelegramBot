using app.common.DTO;
using app.common.Utils;
using app.business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace app.web.view.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialityController : ABaseController<SpecialityController, ISpecialityBusinessService, SpecialtyDTO>
    {

        public SpecialityController(ILogger<SpecialityController> logger, ISpecialityBusinessService businessService) : base(logger, businessService)
        {

        }
        [HttpPost, Route("specialities-by-faculty/{id}")]
        public async Task<ActionResult<PageableData<SpecialtyDTO>>> GetSpecialityGetFacultyId([FromBody] PageableData<SpecialtyDTO> data, [FromRoute]long id)
        {
            PageableData<SpecialtyDTO> result = await this.businessService.GetPageSpecialityOfFacultyAsync(data,id);
            return Ok(result);
        }

        [HttpGet, Route("get-all")]
        public async Task<ActionResult<PageableData<SpecialtyDTO>>> GetSpecialities()
        {
            IEnumerable<SpecialtyDTO> result = await this.businessService.GetSpecialtiesAsync();
            return Ok(result);
        }
        
    }
}