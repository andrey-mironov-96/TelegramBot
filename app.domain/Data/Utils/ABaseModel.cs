namespace app.domain.Data.Utils
{
    public class ABaseModel
    {
        public long Id { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime? ChangeAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}