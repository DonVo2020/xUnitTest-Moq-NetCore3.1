using System;

namespace DonVo.Inventory.Infrastructures.Helpers
{
    public class PropertyCopier<OriginClass, DestinationClass>
        where OriginClass : class
        where DestinationClass : class
    {
        public static void Copy(OriginClass origin, DestinationClass destination)
        {
            var originProperties = origin.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();
            foreach (var originProperty in originProperties)
            {
                foreach (var destinationProperty in destinationProperties)
                {
                    if (originProperty.Name == destinationProperty.Name)
                    {
                        // Nullable Checking
                        bool originIsNullable = originProperty.PropertyType.IsGenericType && originProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                        bool destinationIsNullable = destinationProperty.PropertyType.IsGenericType && destinationProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                        var originType = originIsNullable ? Nullable.GetUnderlyingType(originProperty.PropertyType) : originProperty.PropertyType;
                        var destinationType = destinationIsNullable ? Nullable.GetUnderlyingType(destinationProperty.PropertyType) : destinationProperty.PropertyType;
                        if (originType == destinationType)
                        {
                            destinationProperty.SetValue(destination, originProperty.GetValue(origin));
                            break;
                        }
                    }
                }
            }
        }
    }
}