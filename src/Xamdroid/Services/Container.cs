using Microsoft.Extensions.DependencyInjection;
using Xamdroid.Models;

namespace Xamdroid.Services
{
    public static class Container
    {
        private static ServiceProvider _service;

        public static ServiceProvider Service
        {
            get
            {
                _service ??= CreateServices().BuildServiceProvider();
                return _service;
            }
        }

        public static void Initialize() => _service ??= CreateServices().BuildServiceProvider();

        private static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IDataStore<Item>>(_ => new MockDataStore());

            return services;
        }
    }
}