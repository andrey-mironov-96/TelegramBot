using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.common.Utils
{
    public class PageableData<TDTO>
        where TDTO : ABaseDTOEntity
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }

        public Filter[] Filters { get; set; }
        public IEnumerable<TDTO> Data { get; set; }
        public PageableData(int page, int pageSize, int total)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.Total = total;
        }
    }
}