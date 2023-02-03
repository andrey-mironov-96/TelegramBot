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
        public FacultyController(ILogger<FacultyController> logger, IFacultyBusinessService businessService) : base(logger, businessService)
        {
            
        }
    }
}