using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace PrsBackendAPI6.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private IRepositoryWrapper _repository;

    public UserController(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    // Get by any condition
    [HttpGet]
    public IActionResult GetByCondition(Uri uri)
    {

        /*
         * 
         *  use URI class to get the expression from the URL and oass it to getByCondition
         * 
         */
        
        
     //   return users = _repository.User.FindByCondition(expression).ToList();

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

    [HttpPost]
    public IActionResult Create([FromBody] UserCreateDTO userDTO)
    {
        if (userDTO == null)
        {
            return BadRequest("User data is null");
        }


        var user = new User
        {
            Id = userDTO.Id,
            Username = userDTO.Username,
            Password = userDTO.Password,
            Firstname = userDTO.Firstname,
            Lastname = userDTO.Lastname,
            Phone = userDTO.Phone,
            Email = userDTO.Email,
            IsReviewer = userDTO.IsReviewer,
            IsAdmin = userDTO.IsAdmin
        };

        _repository.User.Create(user);
        _repository.Save();

        return CreatedAtAction(nameof(GetByCondition), new { id = user.Id }, user);
        
    }


 

}
