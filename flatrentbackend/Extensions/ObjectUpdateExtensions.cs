using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FlatRent.Extensions
{
    public static class ObjectUpdateExtensions
    {
        private static readonly IEnumerable<Type> NonUpdatableAttributes = new[] { typeof(KeyAttribute), typeof(ForeignKeyAttribute), typeof(InversePropertyAttribute) };

        public static void UpdateProperties<T>(this T to, T from)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                if (property.HasAnyAttributes(NonUpdatableAttributes)) continue;
                
                var toValue = property.GetValue(to);
                var fromValue = property.GetValue(from);
                if (fromValue == property.PropertyType.GetDefault()) continue;
                property.SetValue(to, fromValue);
            }
        }

        public static bool HasAnyAttributes(this PropertyInfo property, IEnumerable<Type> types)
        {
            return types.Any(t => (property.GetCustomAttributes(t)?.Count() ?? 0) > 0);
        }

        public static object GetDefault(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static void SetProperty<TItem, TMember>(this IEnumerable<TItem> objects,
            Expression<Func<TItem, TMember>> memberToUpdate, TMember update)
        {
            var propertyInfo = memberToUpdate.GetPropertyInfo();
            foreach (var obj in objects)
            {
                propertyInfo.SetValue(obj, update);
            }
        }

        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(
            this Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var type = typeof(TSource);

            if (!(propertyLambda.Body is MemberExpression member))
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a field, not a property.");

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a property that is not from type {type}.");

            return propInfo;
        }
    }
}