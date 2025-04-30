using System.Linq.Expressions;
using GbLib.BuildingBlock.Domain.Interfaces;

namespace GbLib.BuildingBlock.Domain.Specifications;

public class AndSpecification<T> : BaseSpecification<T>
{
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Criteria = left.Criteria.AndAlso(right.Criteria);

        foreach (var include in right.Includes)
        {
            Includes.Add(include);
        }

        foreach (var include in left.Includes)
        {
            Includes.Add(include);
        }
    }
}

public class OrSpecification<T> : BaseSpecification<T>
{
    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Criteria = left.Criteria.OrElse(right.Criteria);

        Includes.AddRange(left.Includes);
        Includes.AddRange(right.Includes);

        IncludeStrings.AddRange(left.IncludeStrings);

        IncludeStrings.AddRange(right.IncludeStrings);
    }
}

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));


        var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
        var leftBody = leftVisitor.Visit(left.Body);


        var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
        var rightBody = rightVisitor.Visit(right.Body);


        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(leftBody, rightBody), parameter);
    }

    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        if (left == null)
        {
            return right;
        }

        if (right == null)
        {
            return left;
        }


        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
        var leftBody = leftVisitor.Visit(left.Body);


        var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
        var rightBody = rightVisitor.Visit(right.Body);


        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(leftBody, rightBody), parameter);
    }


    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }
    }
}