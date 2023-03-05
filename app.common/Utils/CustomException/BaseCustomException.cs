namespace app.common.Utils.CustomException
{
    public class BaseCustomException : Exception
    {
        public BaseCustomException(string message) : base(message) { }
    }
}