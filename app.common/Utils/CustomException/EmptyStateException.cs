namespace app.common.Utils.CustomException
{
    public class EmptyStateException : BaseCustomException
    {
        public EmptyStateException() : base("ex:es")
        {
        }

    }
}