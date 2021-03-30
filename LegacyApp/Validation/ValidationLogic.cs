using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp.Validation
{
    public static class ValidationLogic
    {
        public static Dictionary<PropertyInfo, InvalidationAttribute[]> GetAttributeDictionnary(Type type)
        {
            return type.GetProperties()
                .ToDictionary(property => property, property => property.GetCustomAttributes(true)
                .Where(attribute => typeof(InvalidationAttribute).IsAssignableFrom(attribute.GetType())).Cast<InvalidationAttribute>().ToArray());
        }

        public static bool Validate(Type type, string propertyName, object instance, bool validateDependance = true)
        {
            var dictionary = GetAttributeDictionnary(type);
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null || !dictionary.ContainsKey(property))
            {
                return true;
            }
            return dictionary[property].Validate(property, instance, validateDependance);
        }

        public static bool Validate(this IEnumerable<InvalidationAttribute> attributes, PropertyInfo property, object instance, bool validateDependance = true)
        {
            return !attributes.Select(attribute => attribute.Invalidate(property, instance, validateDependance)).ToList().All(invalid => invalid);
        }
    }
}
