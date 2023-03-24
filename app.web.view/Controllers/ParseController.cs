using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.common.DTO;
using Microsoft.AspNetCore.Mvc;
using WebParse.Business;
using WebParse.Models;

namespace app.web.view.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParseController : ControllerBase
    {
        private readonly ILogger<ParseController> logger;
        private readonly IWebParseService webParseService;

        public ParseController(ILogger<ParseController> logger, IWebParseService webParseService)
        {
            this.logger = logger;
            this.webParseService = webParseService;
        }
        [HttpGet]
        public async Task<ActionResult<ParsingResult<Dictionary<string, List<AdmissionPlan>>>>> GetParsingResultAsync()
        {
            ParsingResult<Dictionary<string, List<AdmissionPlan>>> result = await webParseService.GetDataFromULGUSite();
            return Ok(result);
        }
    }
}