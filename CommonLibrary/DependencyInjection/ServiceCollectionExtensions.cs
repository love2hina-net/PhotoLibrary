using System.Reflection;

namespace love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;

public static class ServiceCollectionExtensions
{

    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddFromAssemblies(this Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        => AddFromAssemblies(services, AppDomain.CurrentDomain.GetAssemblies());

    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddFromAssemblies(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var query = from type in assemblies.SelectMany(a => a.GetTypes())
                    let attribute = type.GetCustomAttributes<DeclareServiceAttribute>().FirstOrDefault()
                    where (attribute != null) && (type.IsAbstract == false)
                    select new { Type = type, Attribute = attribute };

        foreach (var register in query)
        {
            services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(register.Type, register.Type, register.Attribute.Type));
        }

        return services;
    }

}

[AttributeUsage(AttributeTargets.Class)]
public class DeclareServiceAttribute : Attribute
{

    public Microsoft.Extensions.DependencyInjection.ServiceLifetime Type { get; set; }

    public DeclareServiceAttribute(Microsoft.Extensions.DependencyInjection.ServiceLifetime type)
    {
        Type = type;
    }

}
