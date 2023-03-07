using Contracts;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using Services;

namespace PrsBackendAPI6.Controllers;

[Route("/api/RequestLine")]
[ApiController]
public class RequestLineController : ControllerBase
{


    private RequestLineService _repository;
    public RequestLineController(IRepositoryWrapper repository)
    {
        _repository = new RequestLineService(repository);
    }

    [HttpGet("search-requests")]
    public ActionResult<List<RequestLineDTO>?> GetByCondition([FromQuery] RequestLineDTO requestLineDto)
    {
        return _repository.FindByConditions(requestLineDto);
    }

    [HttpGet]
    public ActionResult<List<RequestLineDTO>> FindAll()
    {
        return _repository.FindAll();
    }


    [HttpPost("Create")]
    public ActionResult<RequestLineDTO> Create([FromBody] RequestLineDTO requestLineDto)
    {
        return _repository.Create(requestLineDto);
    }

    [HttpDelete("Delete")]
    public ActionResult<RequestLineDTO> Delete([FromBody] int id)
    {
        return _repository.Delete(id);
    }

    [HttpPut("Update")]
    public ActionResult<RequestLineDTO> Update([FromBody] RequestLineDTO requestDTO)
    {
        return _repository.Update(requestDTO);
    }


}

