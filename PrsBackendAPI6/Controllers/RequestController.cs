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
    
    [HttpPut("change-status")]
    public ActionResult<RequestDTO> SetRequestStatus([FromQuery] RequestChangeObject requestChangeObject)
    {
        // A user can cancel their own request but cannot change the status of any other requests.
        // User request change: authenticate user. Search for requests attached to that user. if RequestId in 
        // Request change object exists and belongs to user, then change request from current status to cancelled.
        // this will only work if the request has not already been approved, rejected, or completed.
        // RequestChangeObject takes username, password, target request id, and status can be null because
        // user can only cancel and so that will be default new status if user is not reviewer or admin.
        // if user is admin or reviewer, status must be entered as cancelled.

        // If user is a reviewer, then user can change request from pending to in progress or on hold. 
        // this will authenticate user, search for request by entered id. user id on request cannot equal
        // reviewers id. reviewer can only change selected request from pending to in progress or on hold. 
        // request change object takes username, password, target requestId, and new status.

        // if user is admin, then user can change any request from either pending, on hold, or in progress, to either rejected, or approved, or completed.
        // admin does not need to cancel requests. rejection is good enough. cancel is only so a user can withraw the request.
        // user will be authenticated, and choose requestId, new status. Admin cannot alter their own request, unless to cancel it.


        return Ok();
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

