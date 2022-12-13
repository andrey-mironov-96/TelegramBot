namespace app.common.Configure
{
    public class EnvironmentConfigure
    {
        public static string GetEnvironment(Enum variableName)
        {
            #pragma warning disable CS8603
            return Environment.GetEnvironmentVariable(variableName.ToString());
        }
    }
}