using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using PrsUtilities.ExpressionExtensions;
using Repository.DTOs;
using Repository.DTOs.ModelDTO;
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
            return new OkObjectResult(RequestDTOService.GetDtosFromRequests(foundRequests));
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
            List<RequestDTO> allRequests = RequestDTOService.GetDtosFromRequests(requests);
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

        RequestDTO createdRequestDTO = RequestDTOService.GetDtoFromRequest(_repository.Request.FindByCondition(r => r.Id == highestId).ToList()[0]);

        return new OkObjectResult(createdRequestDTO);
    }
    public ActionResult<RequestDTO> Delete(int id)
    {

        var deleteRequest = _repository.Request.FindByCondition(r => r.Id == id).FirstOrDefault();

        if (deleteRequest != null)
        {
            _repository.Request.Delete(deleteRequest);
            _repository.Save();
            return new OkObjectResult(RequestDTOService.GetDtoFromRequest(deleteRequest));
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

            RequestDTO returnedRequest = RequestDTOService.GetDtoFromRequest(_repository.Request.FindByCondition(v => v.Id == int.Parse(requestDto.Id)).FirstOrDefault());
            return new OkObjectResult(returnedRequest);
        }

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

    public ActionResult<RequestDTO> ReviewRequest(RequestChangeObject requestChangeObject)
    {
        Request targetRequest = _repository.Request.FindByCondition(r => r.Id == int.Parse(requestChangeObject.RequestId)).ToList()[0];

        if (targetRequest != null)
        {   
            User? tryUser = _repository.User.FindByCondition(u => u.Username == requestChangeObject.Username).FirstOrDefault();
            if (tryUser != null)
            {
                if (tryUser.Password.Equals(requestChangeObject.Password))
                {
                    if (!targetRequest.Status.Equals("REVIEW"))
                    {
                        return new BadRequestObjectResult($"This request has an invalid status for review: {targetRequest.Status}");
                    }
                    else if (!tryUser.IsReviewer && !tryUser.IsAdmin){
                        return new BadRequestObjectResult($"You are not authorized to review requests: reviewer:{tryUser.IsReviewer}, admin:{tryUser.IsAdmin} ");
                    }
                    else if (targetRequest.UserId == tryUser.Id)
                    {
                        return new BadRequestObjectResult("One may not review their own request.");
                    }
                    else if (!requestChangeObject.NewStatus.Equals("APPROVED") && !requestChangeObject.NewStatus.Equals("REJECTED"))
                    {
                        return new BadRequestObjectResult($"You have tried entering an invalid status: {requestChangeObject.NewStatus}");
                    }
                    else
                    {
                        targetRequest.Status = requestChangeObject.NewStatus;
                        _repository.Request.Update(targetRequest);
                        _repository.Save();
                        return new OkObjectResult($"Status changed: {requestChangeObject.NewStatus}");
                    }
                }
            }
            else
            {
                return new NotFoundObjectResult("There was no user/password match.");
            }
            
        }
        else
        {
            return new NotFoundObjectResult("No request by that id was foun.");
        }
        return new BadRequestObjectResult("Unknown error");
    }

    public ActionResult<RequestDTO> SubmitRequest(RequestChangeObject requestChangeObject)
    {
        Request? targetRequest = _repository.Request.FindByCondition(r => r.Id == int.Parse(requestChangeObject.RequestId)).FirstOrDefault();

        if (targetRequest == null)
        {
            return new NotFoundObjectResult($"No request was afound by that id: {requestChangeObject.RequestId}");
        }
        else
        {
            User? tryUser = _repository.User.FindByCondition(u => u.Username == requestChangeObject.Username).FirstOrDefault();
            if (tryUser == null)
            {             
                return new NotFoundObjectResult($"No user was found by that id: {tryUser.Id}.");
            }
            else
            {
                if (!tryUser.Password.Equals(requestChangeObject.Password))
                {
                    return new NotFoundObjectResult(" Bad username/password combination.");
                }
                else
                {
                    if (!targetRequest.Status.Equals("PENDING"))
                    {
                        return new BadRequestObjectResult($"You are not authorized to change the status of this request: {targetRequest.Status}");
                    }
                    else
                    {
                        
                        
                        if (targetRequest.UserId != tryUser.Id)
                        {
                            return new BadRequestObjectResult($"You may only submit your own requests. requestUserId: {targetRequest.UserId}, your user Id: {tryUser.Id}");
                        }
                        else
                        {

                            List<Request> requests = _repository.Request.FindAll().ToList();
                            decimal allRequestsSum = 0;
                            string pricedRequestStatus = "";

                            foreach(Request request in requests)
                            {
                                allRequestsSum += request.Total;
                            }


                            if (targetRequest.Total > (allRequestsSum / 3))
                            {
                                pricedRequestStatus = "REVIEW";
                            }
                            else
                            {
                                pricedRequestStatus = "APPROVED";
                            }

                            targetRequest.Status = pricedRequestStatus;
                            _repository.Request.Update(targetRequest);
                            _repository.Save();

                            return new OkObjectResult($"The status of requestId {targetRequest.Id} was changed to {targetRequest.Status}. Request Updated and submitted.");
                        }
                            
                        
                        
                    }
                }
            }
        }

        
    }

}

