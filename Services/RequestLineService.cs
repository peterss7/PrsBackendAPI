using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using System.Linq.Expressions;


namespace Services;

public class RequestLineService
{

    private IRepositoryWrapper _repository;

    public RequestLineService(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public ActionResult<List<RequestLineDTO>?> FindByConditions(RequestLineDTO requestLineDto)
    {
        List<Expression<Func<RequestLine, bool>>> expressions = new List<Expression<Func<RequestLine, bool>>>();

        if (!string.IsNullOrEmpty(requestLineDto.Id))
        {
            expressions.Add(r => r.Id == int.Parse(requestLineDto.Id));
        }
        if (!string.IsNullOrEmpty(requestLineDto.RequestId))
        {
            expressions.Add(r => r.RequestId == int.Parse(requestLineDto.RequestId));
        }
        if (!string.IsNullOrEmpty(requestLineDto.ProductId))
        {
            expressions.Add(r => r.ProductId == int.Parse(requestLineDto.ProductId));
        }
        if (!string.IsNullOrEmpty(requestLineDto.Quantity))
        {
            expressions.Add(r => r.Quantity == int.Parse(requestLineDto.Quantity));
        }
        

        List<RequestLine> foundRequestLines = _repository.RequestLine.FindByConditions(expressions).ToList();

        if (foundRequestLines.Count > 0)
        {
            return new OkObjectResult(GetDtosFromRequestLines(foundRequestLines));
        }
        else
        {
            return new NotFoundObjectResult("No requestLines were found");
        }


    }

    public ActionResult<List<RequestLineDTO>> FindAll()
    {
        List<RequestLine> requestLines = _repository.RequestLine.FindAll().ToList();

        if (requestLines != null)
        {
            List<RequestLineDTO> allRequestLines = GetDtosFromRequestLines(requestLines);
            return new OkObjectResult(allRequestLines);
        }
        else
        {
            return new BadRequestObjectResult("No requestLines were found");
        }
    }

    public ActionResult<RequestLineDTO> Create(RequestLineDTO requestLineDto)
    {
        string requestId, productId, quantity;
        


        if (!string.IsNullOrEmpty(requestLineDto.RequestId))
        {
            requestId = requestLineDto.RequestId;
        }
        else
        {
            return new BadRequestObjectResult("RequestId cannot be null.");
        }
        if (!string.IsNullOrEmpty(requestLineDto.ProductId))
        {
            productId = requestLineDto.ProductId;
        }
        else
        {
            return new BadRequestObjectResult("ProductId cannot be null.");
        }
        if (!string.IsNullOrEmpty(requestLineDto.Quantity))
        {
            quantity = requestLineDto.Quantity;
        }
        else
        {
            return new BadRequestObjectResult("Quantity cannot be null.");
        }
        

        var newRequestLine = new RequestLine
        {
            RequestId = int.Parse(requestId),
            ProductId = int.Parse(productId),
            Quantity = int.Parse(quantity)            
        };
        _repository.RequestLine.Create(newRequestLine);
        _repository.Save();

        int highestId = 0;
        List<RequestLine> allRequestLines = _repository.RequestLine.FindAll().ToList();
        foreach (RequestLine requestLine in allRequestLines)
        {
            if (requestLine.Id > highestId)
            {
                highestId = requestLine.Id;
            }
        }

        RequestLineDTO createdRequestLineDTO = GetDtoFromRequestLine(_repository.RequestLine.FindByCondition(r => r.Id == highestId).ToList()[0]);

        return new OkObjectResult(createdRequestLineDTO);
    }

    public ActionResult<RequestLineDTO> Delete(int id)
    {

        var deleteRequestLine = _repository.RequestLine.FindByCondition(r => r.Id == id).FirstOrDefault();

        if (deleteRequestLine != null)
        {
            _repository.RequestLine.Delete(deleteRequestLine);
            _repository.Save();
            return new OkObjectResult(GetDtoFromRequestLine(deleteRequestLine));
        }
        else
        {
            return new BadRequestObjectResult("Cannot delete RequestLine. No user by that Id was found.");
        }
    }


    public ActionResult<RequestLineDTO> Update(RequestLineDTO requestLineDto)
    {

        RequestLine? requestLine = new RequestLine();

        if (requestLineDto.Id == null)
        {
            return new BadRequestObjectResult("must include ID to update");
        }
        else
        {
            requestLine = _repository.RequestLine.FindByCondition(r => r.Id == int.Parse(requestLineDto.Id)).FirstOrDefault();

            if (requestLine == null)
            {
                return new NotFoundObjectResult("RequestLine of that Id was not found. Nothing was Updated.");
            }

            if (!string.IsNullOrEmpty(requestLineDto.RequestId) && !requestLineDto.RequestId.Equals("string"))
            {
                requestLine.RequestId = int.Parse(requestLineDto.RequestId);
            }
            if (!string.IsNullOrEmpty(requestLineDto.ProductId) && !requestLineDto.ProductId.Equals("string"))
            {
                requestLine.ProductId = int.Parse(requestLineDto.ProductId);
            }
            if (!string.IsNullOrEmpty(requestLineDto.Quantity) && !requestLineDto.Quantity.Equals("string"))
            {
                requestLine.Quantity = int.Parse(requestLineDto.Quantity);
            }
            _repository.RequestLine.Update(requestLine);
            _repository.Save();

            RequestLineDTO returnedRequestLine = GetDtoFromRequestLine(_repository.RequestLine.FindByCondition(v => v.Id == int.Parse(requestLineDto.Id)).FirstOrDefault());
            return new OkObjectResult(returnedRequestLine);
        }

    }

    public RequestLineDTO GetDtoFromRequestLine(RequestLine requestLine)
    {
        RequestLineDTO returnedDto = new RequestLineDTO
        {
            Id = requestLine.Id.ToString(),
            RequestId = requestLine.RequestId.ToString(),
            ProductId = requestLine.ProductId.ToString(),
            Quantity = requestLine.Quantity.ToString(),
        };

        return returnedDto;
    }

    public List<RequestLineDTO> GetDtosFromRequestLines(List<RequestLine> requestLines)
    {

        List<RequestLineDTO> requestLinesDto = new List<RequestLineDTO>();

        foreach (RequestLine requestLine in requestLines)
        {
            requestLinesDto.Add(GetDtoFromRequestLine(requestLine));
        }

        return requestLinesDto;
    }

}

