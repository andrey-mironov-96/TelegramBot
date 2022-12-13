using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AppBot
{
    public static class Extensions
    {
        public static T GetConfiguration<T>(this IServiceProvider serviceProvider)
         where T : class   
        {
            var obj = serviceProvider.GetService<IOptions<T>>();
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(T));
            }
            return obj.Value;
        }
    }
}