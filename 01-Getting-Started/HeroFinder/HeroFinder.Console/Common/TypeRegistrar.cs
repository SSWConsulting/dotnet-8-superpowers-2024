using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Spectre.Console.Cli;

namespace HeroFinder.Console.Common;

/// <summary>
/// Create a custom TypeRegistrar that wraps our the underlying DI Container provider by .NET.
/// This allows Spectre.Console to use the same DI Container and services as the rest of the application.
/// </summary>
public sealed class TypeRegistrar(IHostBuilder builder) : ITypeRegistrar
{
    public ITypeResolver Build()
    {
        return new TypeResolver(builder.Build());
    }

    public void Register(Type service, Type implementation)
    {
        builder.ConfigureServices((_, services) => services.AddSingleton(service, implementation));
    }

    public void RegisterInstance(Type service, object implementation)
    {
        builder.ConfigureServices((_, services) => services.AddSingleton(service, implementation));
    }

    public void RegisterLazy(Type service, Func<object> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        builder.ConfigureServices((_, services) => services.AddSingleton(service, _ => func()));
    }
}