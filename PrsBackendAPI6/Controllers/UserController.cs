using Contracts;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using Services;

namespace PrsBackendAPI6.Controllers;

[Route("/api/User")]
[ApiController]
public class UserController : ControllerBase
{
    private UserService _repository;
    public UserController(IRepositoryWrapper repository)
    {
        _repository = new UserService(repository);
    }

    [HttpGet("search-users")]        
    public ActionResult<List<UserDTO>?> GetByCondition([FromQuery] UserSearchObject userSearchObject)
    {           
       return _repository.FindByConditions(userSearchObject);
    }
    
    [HttpGet]
    public ActionResult<List<UserDTO>>  FindAll()
    {        
        return _repository.FindAll();        
    }   
   

    [HttpPost("Create")]
    public ActionResult<UserDTO> Create([FromBody] UserDTO userDto)
    {
        return _repository.Create(userDto);
    }

    [HttpDelete("Delete")]
    public ActionResult<UserDTO> Delete([FromBody] int id)
    {
        return _repository.Delete(id);
    }
    
    [HttpPut("Update")]
    public ActionResult<UserDTO> Update([FromBody]UserDTO userDTO)
    {
        return _repository.Update(userDTO);
    }    
}

