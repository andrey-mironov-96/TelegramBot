namespace app.common.Utils
{
    public class ABaseDTOEntity
    {
        public long Id { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime? ChangeAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}