using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PrsUtilities.ExpressionExtensions;

public static class ExpressionsExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
        var leftExpression = leftVisitor.Visit(left.Body);

        var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
        var rightExpression = rightVisitor.Visit(right.Body);

        var andExpression = Expression.AndAlso(leftExpression, rightExpression);

        return Expression.Lambda<Func<T, bool>>(andExpression, parameter);
    }
    private class ReplaceExpressionVisitor : ExpressionVisitor
    {

        private readonly Expression _oldExpression;
        private readonly Expression _newExpression;

        public ReplaceExpressionVisitor(Expression oldExpression, Expression newExpression)
        {
            _oldExpression = oldExpression;
            _newExpression = newExpression;
        }

        public override Expression Visit(Expression node)
        {
            return node == _oldExpression ? _newExpression : base.Visit(node);
        }

    }

}


