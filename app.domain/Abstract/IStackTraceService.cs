using app.common.DTO;

namespace app.domain.Abstract
{
    public interface IStackTraceService
    {
        public Task AddStackTrace(StackTraceDTO stackTrace);
        public List<StackTraceDTO> GetStackTraces();
    }
}