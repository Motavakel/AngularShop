using AutoMapper;
using System.Reflection;

namespace Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);
        var mappingMethodName = nameof(IMapFrom<object>.Mapping);

        foreach (var type in assembly.GetExportedTypes()
                 .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == mapFromType)))
        {
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(mappingMethodName) ??
                         type.GetInterfaces()
                             .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == mapFromType)
                             ?.GetMethod(mappingMethodName, new[] { typeof(Profile) });

            method?.Invoke(instance, new object[] { this });
        }
    }
}
