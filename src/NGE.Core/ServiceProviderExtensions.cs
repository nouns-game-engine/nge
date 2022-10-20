using Microsoft.Xna.Framework.Graphics;

namespace NGE.Core
{
    public static class ServiceProviderExtensions
    {
        public static GraphicsDevice GetGraphicsDevice(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IGraphicsDeviceService>().GraphicsDevice;
        }

        public static T GetRequiredService<T>(this IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(T));

            if (service is T typed)
                return typed;

            throw new InvalidOperationException($"Could not find a service with type {typeof(T).FullName}, when one is required");
        }
    }

}
