namespace app.test.core.Utils
{
    public interface ITestService<T>
    {
        public abstract T GetService();
    }
}