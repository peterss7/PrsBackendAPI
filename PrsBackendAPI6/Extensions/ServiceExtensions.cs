using Contracts;
using Repository;

namespace PrsBackendAPI6.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureRepositoryWrapper(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }
}
