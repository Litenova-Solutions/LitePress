using System.Reflection;
using System.Text.Json.Serialization.Metadata;

namespace LitePress.Infrastructure.Marten.Serialization.Internal;

/// <summary>
/// Shared helpers used by aggregate configurations and conventions.
/// </summary>
internal static class JsonTypeInfoExtensions
{
    internal static void IgnoreProperty(this JsonTypeInfo typeInfo, string propertyName)
    {
        var property = typeInfo.Properties.FirstOrDefault(p => p.Name == propertyName);
        if (property is not null)
        {
            property.ShouldSerialize = static (_, _) => false;
        }
    }

    internal static void UseNonPublicConstructorIfPresent(this JsonTypeInfo typeInfo, Type clrType)
    {
        var privateParameterlessConstructor = clrType.GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null,
            Type.EmptyTypes,
            modifiers: null);

        if (privateParameterlessConstructor is not null)
        {
            typeInfo.CreateObject = () => Activator.CreateInstance(clrType, nonPublic: true)!;
        }
    }

    internal static void EnableNonPublicPropertySetters(this JsonTypeInfo typeInfo, Type clrType)
    {
        foreach (var property in typeInfo.Properties)
        {
            if (property.Set is not null)
            {
                continue;
            }

            var clrProperty = clrType.GetProperty(
                property.Name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (clrProperty?.SetMethod is not null)
            {
                property.Set = (instance, value) => clrProperty.SetValue(instance, value);
            }
        }
    }

    internal static void IncludeNonPublicInstanceFields(this JsonTypeInfo typeInfo, Type clrType)
    {
        foreach (var field in clrType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
        {
            if (typeInfo.Properties.Any(property => property.Name == field.Name))
            {
                continue;
            }

            var propertyInfo = typeInfo.CreateJsonPropertyInfo(field.FieldType, field.Name);
            propertyInfo.Get = instance => field.GetValue(instance);
            propertyInfo.Set = (instance, value) => field.SetValue(instance, value);
            typeInfo.Properties.Add(propertyInfo);
        }
    }
}
