namespace app.common.DTO
{
    public class ParsingResult<T>
    {
        public bool ResultParse { get; set; }

        public T Data { get; set; }

        public List<StackTraceDTO> StackTraces { get; set; } = new List<StackTraceDTO>();
    }
}