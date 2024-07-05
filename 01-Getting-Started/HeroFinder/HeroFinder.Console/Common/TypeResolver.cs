using Microsoft.Extensions.Hosting;

using Spectre.Console.Cli;

namespace HeroFinder.Console.Common;

/// <summary>
/// A custom TypeResolver that wraps our the underlying DI Container provider by .NET.
/// </summary>
public sealed class TypeResolver(IHost provider) : ITypeResolver, IDisposable
{
    private readonly IHost _host = provider ?? throw new ArgumentNullException(nameof(provider));

    public object? Resolve(Type? type)
    {
        return type != null ? _host.Services.GetService(type) : null;
    }

    public void Dispose()
    {
        _host.Dispose();
    }
}