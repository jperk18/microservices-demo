using System.Linq.Expressions;

namespace Health.Shared.Application.Core;

public static class Reflection
{
    public static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
    {
        MemberExpression memberExpression = (MemberExpression)property.Body;

        return memberExpression.Member.Name;
    }
}