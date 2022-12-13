using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinesDAL.Services
{
    public interface ICommandService
    {
        Task<ExecuteResult> ExecuteCommandAsync(string command);
    }
}