namespace app.common.DTO
{
    public class StackTraceDTO
    {
        public Guid Identity { get; set; }
        public DateTime CreateAt { get; set; }
        public string Step { get; set; }

        public string Error { get; set; }

    }
}