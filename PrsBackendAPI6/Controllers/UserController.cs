using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using Services;
using System;
using System.Diagnostics;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace PrsBackendAPI6.Controllers;

[Route("/api/User")]
[ApiController]
public class UserController : ControllerBase
{
    private IRepositoryWrapper _repository;

    public UserController(IRepositoryWrapper repository)
    {
        _repository = repository;
    }


    [Route("/search-users")]
    [HttpGet]
    public ActionResult<List<User>> GetByCondition(string url)
    {
        var service = new UserConditionService(_repository);

        var users = service.FindByConditions(url);

        Debug.WriteLine("Returned into controller: " + users.Count + " num users");

        return users;
        
    }
    // Get All
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new
        {
            Users = _repository.User.FindAll().ToList()
        });
    }


 

}
