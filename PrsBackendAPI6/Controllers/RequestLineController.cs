using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

using Repository.DTOs;
using System.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using HotChocolate.Execution;
using Services;

namespace PrsBackendAPI6.Controllers;

[Route("/api/RequestLine")]
[ApiController]
public class RequestLineController : ControllerBase
{
    private IRepositoryWrapper _repository;

    public RequestLineController(IRepositoryWrapper repository)
    {
        _repository = repository;
    }
    
    [Route("/search-requestlines")]
    [HttpGet]
    public ActionResult<List<RequestLine>> GetByCondition(string url)
    {

        //HttpContext query = (HttpContext)HttpContext.Request.Query;

        //var conditions = ConditionService.GetConditions<RequestLine>(query, _repository);
        

        

        /*
        foreach (var queryParam in queryDictionary)
        {
            var key = queryParam.Key;
            var value = queryParam.Value;

            //Console.WriteLine($"Key: {key}, and value: {value}");
            return Ok(($"Key: {key}, and value: {value}"));

        }
        */

        return Ok();

        /*
        ParameterExpression parameter = Expression.Parameter(typeof(User), "x");
        List<Expression> expressions = new List<Expression>();

        if (!string.IsNullOrEmpty(id))
        {
            expressions.Add(Expression.GreaterThan(Expression.Property(parameter, "Id"), Expression.Constant(int.Parse(id))));
            expressions.Add(Expression.LessThanOrEqual(Expression.Property(parameter, "Id"), Expression.Constant(100)));
        }

        if (age.HasValue)
        {
            expressions.Add(Expression.Equal(Expression.Property(parameter, "Age"), Expression.Constant(age.Value)));
        }

        if (!string.IsNullOrEmpty(gender))
        {
            expressions.Add(Expression.Equal(Expression.Property(parameter, "Gender"), Expression.Constant(gender)));
        }

        Expression body = expressions.Any() ? expressions.Aggregate(Expression.And) : Expression.Constant(true);
        Expression<Func<User, bool>> lambda = Expression.Lambda<Func<User, bool>>(body, parameter);

        IQueryable<User> query = _context.Users.Where(lambda);

        if (!string.IsNullOrEmpty(order))
        {
            query = query.OrderBy(order);
        }
        */
        

    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new
        {
            RequestLines = _repository.RequestLine.FindAll().ToList()
        });
    }



    [HttpPost]
    public IActionResult Create(RequestLineDTO requestLineDTO)
    {
        var _requestLine = new RequestLine
        {
            Id = requestLineDTO.Id,
            RequestId = requestLineDTO.RequestId,
            ProductId = requestLineDTO.ProductId,
            Quantity = requestLineDTO.Quantity
        };

        
        _repository.RequestLine.Create(_requestLine);

        return Ok(_requestLine);
    }

    [HttpPut]
    public IActionResult Update(RequestLineDTO requestLineDTO)
    {

        var _requestLine = new RequestLine
        {
            Id = requestLineDTO.Id,
            RequestId = requestLineDTO.RequestId,
            ProductId = requestLineDTO.ProductId,
            Quantity = requestLineDTO.Quantity
        };
        
        _repository.RequestLine.Update(_requestLine);
        
        
        return Ok();
    }


}
