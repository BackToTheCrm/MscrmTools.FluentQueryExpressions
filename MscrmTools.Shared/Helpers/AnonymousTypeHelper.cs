using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Xrm.Sdk;

namespace MscrmTools.FluentQueryExpressions.Helpers
{
    [ExcludeFromCodeCoverage]
    internal class AnonymousTypeHelper
    {
        /// <summary>
        ///     Creates an array of attribute names array from an Anonymous Type Initializer.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="anonymousTypeInitializer">The anonymous type initializer.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">lambda must return an object initializer</exception>
        public static string[] GetAttributeNamesArray<TEntity>(Expression<Func<TEntity, object>> anonymousTypeInitializer) where TEntity : Entity

        {
            var initializer = anonymousTypeInitializer.Body as NewExpression;

            if (initializer?.Members == null)
            {
                throw new ArgumentException("lambda must return an object initializer");
            }

            // Search for and replace any occurence of Id with the actual Entity's Id
            return initializer.Members.Select(GetLogicalAttributeName<TEntity>).ToArray();
        }

        /// <summary>
        ///     Normally just returns the name of the property, in lowercase.  But Id must be looked up via reflection.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static string GetLogicalAttributeName<TEntity>(MemberInfo property) where TEntity : Entity

        {
            var name = property.Name.ToLower();
            if (name == "id")
            {
                var attribute = typeof(TEntity).GetProperty("Id")?.GetCustomAttributes<AttributeLogicalNameAttribute>().FirstOrDefault();

                if (attribute == null)
                {
                    throw new ArgumentException($"{property.Name} does not contain an AttributeLogicalNameAttribute. Unable to determine id");
                }

                name = attribute.LogicalName;
            }

            return name;
        }
    }
}
