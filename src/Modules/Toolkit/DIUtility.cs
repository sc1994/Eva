using Microsoft.Extensions.DependencyInjection;

namespace Eva.ToolKit;

public static class DIUtility
{
    private static IServiceProvider? StaticProvider { get; set; }

    public static void SetGlobalServiceProvider(IServiceProvider provider)
    {
        StaticProvider = provider;
    }

    private static IServiceProvider GetStaticProvider()
    {
        return StaticProvider ?? throw new InvalidOperationException("Global service provider is not set.");
    }

    public static T GetScopeRequiredService<T>() where T : notnull
    {
        using var scope = GetStaticProvider().CreateScope();

        return scope.ServiceProvider.GetRequiredService<T>();
    }

    public static T GetRequiredService<T>() where T : notnull
    {
        return GetStaticProvider().GetRequiredService<T>();
    }
}