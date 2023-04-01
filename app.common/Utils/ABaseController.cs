using app.common.Utils.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.common.Utils
{
    public abstract class ABaseController<TController, TBusinessService, TDTO>: ControllerBase
        where TController : class
        where TBusinessService : IBaseBusinessService<TDTO>
        where TDTO : ABaseDTOEntity

    {
        protected readonly ILogger<TController> logger;
        protected readonly TBusinessService businessService;

        public ABaseController(ILogger<TController> logger, TBusinessService businessService)
        {
            this.logger = logger;
            this.businessService = businessService;
        }

        [HttpPost, Route("get-page")]
        public async Task<ActionResult<PageableData<TDTO>>> GetPage([FromBody] PageableData<TDTO> data)
        {
            PageableData<TDTO> result = await this.businessService.GetPage(data);
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult<TDTO>> GetById(long id)
        {
            TDTO result = await this.businessService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult<TDTO>> Delete(long id)
        {
            bool result = await this.businessService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPost, Route("save")]
        public async Task<ActionResult<TDTO>> Save([FromBody]TDTO value)
        {
            TDTO result = await this.businessService.SaveAsync(value);
            return Ok(result);
        }
    }
}