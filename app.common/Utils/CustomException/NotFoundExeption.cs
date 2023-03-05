using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.common.Utils.CustomException
{
    public class NotFoundExeption : BaseCustomException
    {
        public NotFoundExeption(string message) : base(message)
        {
        }
    }
}