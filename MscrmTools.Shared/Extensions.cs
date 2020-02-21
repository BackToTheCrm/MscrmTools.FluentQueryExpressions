using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace MscrmTools.FluentQueryExpressions
{
    public static class Extensions
    {
        public static Query<TEntity> Query<TEntity>(this IOrganizationService service) where TEntity : Entity
        {
            return new Query<TEntity>(service);
        }
    }
}