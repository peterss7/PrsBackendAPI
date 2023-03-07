using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DTOs;
using System.Linq.Expressions;
using System.Reflection;

namespace Repository;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext { get; set; }
    public RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }

    public IQueryable<T> FindAll() => RepositoryContext.Set<T>().AsNoTracking();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
        RepositoryContext.Set<T>().Where(expression).AsNoTracking();

    public IQueryable<T> FindByConditions(List<Expression<Func<T, bool>>> expressions)
    {
        var parameter = Expression.Parameter(typeof(T));
        Expression body = Expression.Constant(true);

        foreach(var expression in expressions)
        {
            var visitor = new ReplaceExpressionVisitor(expression.Parameters[0], parameter);
            body = Expression.AndAlso(body, visitor.Visit(expression.Body));
        }

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return RepositoryContext.Set<T>().Where(lambda).AsNoTracking();
    }

    public T Create(T entity)
    {
        var newEntity = RepositoryContext.Set<T>().Add(entity).Entity;        
        return newEntity;
    }   

    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);


    //public abstract int GenerateId<T1>() where T1 : class;
    

    /*
    public int GenerateId<T>() where T : class
    {
    int nextAvailableId = 1;

    // Check if the type T has an Id property or field
    PropertyInfo? idProperty = typeof(T).GetProperty("Id");

    FieldInfo? idField = typeof(T).GetField("Id");

    var entities = RepositoryContext.Set<T>().AsNoTracking().ToList();

    foreach (T entity in entities)
    {
    // Get the value of the Id property or field, if it exists
    int entityId = 0;
    if (idProperty != null)
    {
    entityId = (int)idProperty.GetValue(entity);
    }
    else if (idField != null)
    {
    entityId = (int)idField.GetValue(entity);
    }

    // Check if the entity's Id is greater than the next available Id
    if (entityId >= nextAvailableId)
    {
    nextAvailableId = entityId + 1;
    }
    }

    return nextAvailableId;

    }
    */
    
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