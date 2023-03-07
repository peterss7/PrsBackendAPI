using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Contracts;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    IQueryable<T> FindByConditions(List<Expression<Func<T, bool>>> expressions);
    T Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    int GenerateId<T>() where T : class;
    
}