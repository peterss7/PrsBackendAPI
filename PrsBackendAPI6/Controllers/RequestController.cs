using Contracts;
using Microsoft.AspNetCore.Mvc;
using PrsUtilities;
using Repository.DTOs;
using Services;

namespace PrsBackendAPI6.Controllers;

[Route("/api/Request")]
[ApiController]
public class RequestController : ControllerBase
{


    private RequestService _repository;
    public RequestController(IRepositoryWrapper repository)
    {
        _repository = new RequestService(repository);
    }

    [HttpGet("search-requests")]
    public ActionResult<List<RequestDTO>?> GetByCondition([FromQuery] RequestDTO requestDto)
    {
        return _repository.FindByConditions(requestDto);
    }

    [HttpGet]
    public ActionResult<List<RequestDTO>> FindAll()
    {
        return _repository.FindAll();
    }

    [HttpGet("request-cost")]
    public ActionResult<string> GetRequestCost(int id)
    {
        List<PricedRequestLine>? pricedRequestLines = _repository.GetCost(id).Value;

        if (pricedRequestLines == null)
        {
            return BadRequest("That requestID did created a null result.");
        }
        else
        {
            return Ok(PriceTableStringBuilder.CreatePriceTable(pricedRequestLines));
        }
            

        
    }


    [HttpPost("Create")]
    public ActionResult<RequestDTO> Create([FromBody] RequestDTO requestDto)
    {
        return _repository.Create(requestDto);
    }

    [HttpDelete("Delete")]
    public ActionResult<RequestDTO> Delete([FromBody] int id)
    {
        return _repository.Delete(id);
    }

    [HttpPut("Update")]
    public ActionResult<RequestDTO> Update([FromBody] RequestDTO requestDTO)
    {
        return _repository.Update(requestDTO);
    }


}

