using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using System.Linq.Expressions;


namespace Services;

public class RequestService
{

    private IRepositoryWrapper _repository;

    public RequestService(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public ActionResult<List<RequestDTO>?> FindByConditions(RequestDTO requestDto)
    {
        List<Expression<Func<Request, bool>>> expressions = new List<Expression<Func<Request, bool>>>();

        if (!string.IsNullOrEmpty(requestDto.Id))
        {
            expressions.Add(r => r.Id == int.Parse(requestDto.Id));
        }
        if (!string.IsNullOrEmpty(requestDto.Description))
        {
            expressions.Add(r => r.Description == requestDto.Description);
        }
        if (!string.IsNullOrEmpty(requestDto.Justification))
        {
            expressions.Add(r => r.Justification == requestDto.Justification);
        }
        if (!string.IsNullOrEmpty(requestDto.RejectionReason))
        {
            expressions.Add(r => r.RejectionReason == requestDto.RejectionReason);
        }
        if (!string.IsNullOrEmpty(requestDto.DeliveryMode))
        {
            expressions.Add(r => r.DeliveryMode == requestDto.DeliveryMode);
        }
        if (!string.IsNullOrEmpty(requestDto.SubmittedDate))
        {
            expressions.Add(r => r.SubmittedDate == requestDto.SubmittedDate);
        }
        if (!string.IsNullOrEmpty(requestDto.DateNeeded))
        {
            expressions.Add(r => r.DateNeeded == requestDto.DateNeeded);
        }
        if (!string.IsNullOrEmpty(requestDto.Status))
        {
            expressions.Add(r => r.Status == requestDto.Status);
        }
        if (!string.IsNullOrEmpty(requestDto.Total))
        {
            expressions.Add(r => r.Total == decimal.Parse(requestDto.Total));
        }
        if (!string.IsNullOrEmpty(requestDto.UserId))
        {
            expressions.Add(r => r.UserId == int.Parse(requestDto.UserId));
        }

        List<Request> foundRequests = _repository.Request.FindByConditions(expressions).ToList();

        if (foundRequests.Count > 0)
        {
            return new OkObjectResult(GetDtosFromRequests(foundRequests));
        }
        else
        {
            return new NotFoundObjectResult("No requests were found");
        }


    }

    public ActionResult<List<RequestDTO>> FindAll()
    {
        List<Request> requests = _repository.Request.FindAll().ToList();

        if (requests != null)
        {
            List<RequestDTO> allRequests = GetDtosFromRequests(requests);
            return new OkObjectResult(allRequests);
        }
        else
        {
            return new BadRequestObjectResult("No requests were found");
        }
    }

    public ActionResult<RequestDTO> Create(RequestDTO requestDto)
    {
        string description, justification, deliveryMode,
            status, total, userId, submittedDate, dateNeeded;
        string? rejectionReason = null;
        

        if (!string.IsNullOrEmpty(requestDto.Description))
        {
            description = requestDto.Description;
        }
        else
        {
            return new BadRequestObjectResult("Partnumber cannot be null.");
        }
        if (!string.IsNullOrEmpty(requestDto.Justification))
        {
            justification = requestDto.Justification;
        }
        else
        {
            return new BadRequestObjectResult("Justification cannot be null.");
        }
        if (!string.IsNullOrEmpty(requestDto.RejectionReason))
        {
            rejectionReason = requestDto.RejectionReason;
        }
        else
        {
            return new BadRequestObjectResult("RejectionReason cannot be null.");
        }
        if (!string.IsNullOrEmpty(requestDto.DeliveryMode))
        {
            deliveryMode = requestDto.DeliveryMode;
        }
        else
        {
            return new BadRequestObjectResult("DeliveryMode cannot be null");
        }
        if (!string.IsNullOrEmpty(requestDto.SubmittedDate))
        {
            submittedDate = requestDto.SubmittedDate;
        }
        else
        {
            return new BadRequestObjectResult("Submitted Date cannot be null");
        }
        if (!string.IsNullOrEmpty(requestDto.DateNeeded))
        {
            dateNeeded = requestDto.DateNeeded;
        }
        else
        {
            return new BadRequestObjectResult("DateNeeded cannot be null");
        }
        if (!string.IsNullOrEmpty(requestDto.Status))
        {
            status = requestDto.Status;
        }
        else
        {
            return new BadRequestObjectResult("DateNeeded cannot be null");
        }
        if (!string.IsNullOrEmpty(requestDto.Total))
        {
            total = requestDto.Total;
        }
        else
        {
            return new BadRequestObjectResult("DateNeeded cannot be null");
        }
        if (!string.IsNullOrEmpty(requestDto.UserId))
        {
            userId = requestDto.UserId;
        }
        else
        {
            return new BadRequestObjectResult("DateNeeded cannot be null");
        }

        var newRequest = new Request
        {
            Description = description,
            Justification = justification,
            RejectionReason = rejectionReason,            
            DeliveryMode = deliveryMode,
            SubmittedDate = submittedDate,
            DateNeeded = dateNeeded,
            Total = decimal.Parse(total),
            UserId = int.Parse(userId)
        };
        _repository.Request.Create(newRequest);
        _repository.Save();

        int highestId = 0;
        List<Request> allRequests = _repository.Request.FindAll().ToList();
        foreach (Request request in allRequests)
        {
            if (request.Id > highestId)
            {
                highestId = request.Id;
            }
        }

        RequestDTO createdRequestDTO = GetDtoFromRequest(_repository.Request.FindByCondition(r => r.Id == highestId).ToList()[0]);

        return new OkObjectResult(createdRequestDTO);
    }

    public ActionResult<RequestDTO> Delete(int id)
    {

        var deleteRequest = _repository.Request.FindByCondition(r => r.Id == id).FirstOrDefault();

        if (deleteRequest != null)
        {
            _repository.Request.Delete(deleteRequest);
            _repository.Save();
            return new OkObjectResult(GetDtoFromRequest(deleteRequest));
        }
        else
        {
            return new BadRequestObjectResult("Cannot delete Request. No user by that Id was found.");
        }
    }


    public ActionResult<RequestDTO> Update(RequestDTO requestDto)
    {

        Request? request = new Request();

        if (requestDto.Id == null)
        {
            return new BadRequestObjectResult("must include ID to update");
        }
        else
        {
            request = _repository.Request.FindByCondition(r => r.Id == int.Parse(requestDto.Id)).FirstOrDefault();

            if (request == null)
            {
                return new NotFoundObjectResult("Request of that Id was not found. Nothing was Updated.");
            }

            if (!string.IsNullOrEmpty(requestDto.Description) && !requestDto.Description.Equals("string"))
            {
                request.Description = requestDto.Description;
            }
            if (!string.IsNullOrEmpty(requestDto.Justification) && !requestDto.Justification.Equals("string"))
            {
                request.Justification = requestDto.Justification;
            }
            if (!string.IsNullOrEmpty(requestDto.RejectionReason) && !requestDto.RejectionReason.Equals("string"))
            {
                request.RejectionReason = requestDto.RejectionReason;
            }
            if (!string.IsNullOrEmpty(requestDto.DeliveryMode) && !requestDto.DeliveryMode.Equals("string"))
            {
                request.DeliveryMode = requestDto.DeliveryMode.ToLower();
            }
            if (!string.IsNullOrEmpty(requestDto.SubmittedDate) && !requestDto.SubmittedDate.Equals("string"))
            {
                request.SubmittedDate = requestDto.SubmittedDate;
            }
            if (!string.IsNullOrEmpty(requestDto.DateNeeded) && !requestDto.DateNeeded.Equals("string"))
            {
                request.DateNeeded = requestDto.DateNeeded;
            }
            if (!string.IsNullOrEmpty(requestDto.Status) && !requestDto.Status.Equals("string"))
            {
                request.Status = requestDto.Status;
            }
            if (!string.IsNullOrEmpty(requestDto.Total) && !requestDto.Total.Equals("string"))
            {
                request.Status = requestDto.Total;
            }
            if (!string.IsNullOrEmpty(requestDto.UserId) && !requestDto.UserId.Equals("string"))
            {
                request.Status = requestDto.UserId;
            }

            _repository.Request.Update(request);
            _repository.Save();

            RequestDTO returnedRequest = GetDtoFromRequest(_repository.Request.FindByCondition(v => v.Id == int.Parse(requestDto.Id)).FirstOrDefault());
            return new OkObjectResult(returnedRequest);
        }

    }

    public RequestDTO GetDtoFromRequest(Request request)
    {
        RequestDTO returnedDto = new RequestDTO
        {
            Id = request.Id.ToString(),
            Description = request.Description,
            Justification = request.Justification,
            RejectionReason = request.RejectionReason.ToString(),
            DeliveryMode = request.DeliveryMode,
            SubmittedDate = request.SubmittedDate,
            DateNeeded = request.DateNeeded.ToString(),
        };

        return returnedDto;
    }

    public List<RequestDTO> GetDtosFromRequests(List<Request> requests)
    {

        List<RequestDTO> requestsDto = new List<RequestDTO>();

        foreach (Request request in requests)
        {
            requestsDto.Add(GetDtoFromRequest(request));
        }

        return requestsDto;
    }

    public ActionResult<List<PricedRequestLine>> GetCost(int id)
    {
        
        List<PricedRequestLine> pricedRequestLines = new List<PricedRequestLine>();
        List<RequestLine> requestLines = _repository.RequestLine.FindByCondition(rl => rl.RequestId == id).ToList();

        decimal total = 0;

        foreach (RequestLine requestLine in requestLines)
        {
            var product = _repository.Product.FindByCondition(p => p.Id == requestLine.ProductId).FirstOrDefault();

            if (product != null)
            {
                total += requestLine.Quantity * product.Price;
            }
            else
            {
                return new BadRequestObjectResult("No product was associated with the request line");
            }
        }

        foreach(RequestLine requestLine in requestLines)
        {
            Product product = _repository.Product.FindByCondition(
                p => p.Id == requestLine.ProductId).FirstOrDefault();

            if (product != null)
            {
                decimal lineCost = product.Price * requestLine.Quantity;

                PricedRequestLine pricedRequestLine = new PricedRequestLine
                {
                    Id = requestLine.Id.ToString(),
                    RequestId = id.ToString(),
                    ProductId = product.Id.ToString(),
                    ProductPrice = product.Price.ToString(),
                    Quantity = requestLine.Quantity.ToString(),
                    LineItemPrice = lineCost.ToString(),
                    RequestCost = total.ToString()                  

                };

                pricedRequestLines.Add(pricedRequestLine);

            }            
            else
            {
                return new BadRequestObjectResult("No product was associated with the request line.");
            }
            
        }

        return pricedRequestLines;


    }

}

