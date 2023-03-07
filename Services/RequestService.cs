using Contracts;
using Entities.Models;

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
    public ActionResult<RequestDTO> ChangeStatus(RequestChangeObject requestChangeObject)
    {

        if (requestChangeObject.NewStatus.Equals("PENDING"))
        {
            return new BadRequestObjectResult("You cannot change a request to its initial status.");
        }

        Request? targetRequest = _repository.Request.FindByCondition(r => r.Id == int.Parse(requestChangeObject.RequestId)).FirstOrDefault();

        if (targetRequest == null)
        {
            return new NotFoundObjectResult("No request by that Id was found.");
        }

        if (targetRequest.Status.Equals("COMPLETED"))
        {
            return new BadRequestObjectResult("This request has already been completed.");
        }

        AuthenticationObject requestChangerAuth = new AuthenticationObject();

        if (requestChangeObject == null || requestChangeObject.Username == null
            || requestChangeObject.Password == null)
        {
            return new BadRequestObjectResult("You entered one or more null values for your login.");
        }
        else
        {
            requestChangerAuth.Username = requestChangeObject.Username;
            requestChangerAuth.Password = requestChangeObject.Password;

            UserDTO authenticatedUser = AuthenticationService.Authenticate(requestChangerAuth, _repository).Value;

            if (authenticatedUser == null)
            {
                return new NotFoundObjectResult("Your login credentials were not accepted.");
            }
            else
            {
                // user is not reviewer or admin and can only cancel
                if (!bool.Parse(authenticatedUser.IsReviewer) && !(bool.Parse(authenticatedUser.IsAdmin)))
                {
                    if (requestChangeObject.NewStatus != "CANCELLED")
                    {
                        return new BadRequestObjectResult("You are not authorized to complete this action.");
                    }
                    else if (targetRequest.UserId != int.Parse(authenticatedUser.Id))
                    {
                        return new BadRequestObjectResult("You are only authorized to cancel your own requests.");
                    }
                    else if (targetRequest.Status.Equals("CANCELLED") || targetRequest.Status.Equals("REJECTED")
                        || targetRequest.Status.Equals("APPROVED"))
                    {
                        return new BadRequestObjectResult("You cannot alter the status of this request.");
                    }                    
                    else 
                    {
                        targetRequest.Status = requestChangeObject.NewStatus;
                        _repository.Request.Update(targetRequest);
                        _repository.Save();
                        return new OkObjectResult("Your request has been cancelled.");
                    }
                }
                // user is a reviewer, and can change request status from "PENDING" to "ON HOLD" or "IN PROGRESS"
                // I imagine ON HOLD means reviewer flags it for an admin, expecting it to be rejected.
                // I imagine IN PROGRESS means the reviewer finds the request valid and submits to admins for approval
                else if (bool.Parse(authenticatedUser.IsReviewer) && !bool.Parse(authenticatedUser.IsAdmin))
                {
                    if (targetRequest.UserId == int.Parse(authenticatedUser.Id) && requestChangeObject.NewStatus.Equals("CANCELLED")
                        && targetRequest.Status.Equals("PENDING"))
                    {
                        targetRequest.Status = requestChangeObject.NewStatus;
                        _repository.Request.Update(targetRequest);
                        _repository.Save();
                        return new OkObjectResult(targetRequest);
                    }
                    else if (!targetRequest.Status.Equals("PENDING"))
                    {
                        return new BadRequestObjectResult("You cannot alter this request.");
                    }
                    else if (requestChangeObject.NewStatus.Equals("APPROVED") || requestChangeObject.Equals("REJECTED")
                        || requestChangeObject.NewStatus.Equals("COMPLETED"))
                    {
                        return new BadRequestObjectResult("You are not authorized to approve or reject requests, or mark them completed.");
                    }
                    else if (requestChangeObject.NewStatus.Equals("ON HOLD") || requestChangeObject.NewStatus.Equals("IN PROGRESS")
                        && targetRequest.Status.Equals("PENDING") && !(targetRequest.UserId == int.Parse(authenticatedUser.Id)))
                    {
                        targetRequest.Status = requestChangeObject.NewStatus;
                        _repository.Request.Update(targetRequest);
                        _repository.Save();
                        return new OkObjectResult(targetRequest);
                    }
                    else
                    {
                        return new BadRequestObjectResult("Unkown issue with your status change request.");
                    }

                }
                // user is admin and can change any thing except but not to pending, and not completed.
                // cannot alter own requests.
                else if (bool.Parse(authenticatedUser.IsAdmin))
                {
                    if (targetRequest.Status.Equals("COMPLETED"))
                    {
                        return new BadRequestObjectResult("This request has already been completed.");
                    }
                    if (requestChangeObject.NewStatus.Equals("PENDING"))
                    {
                        return new BadRequestObjectResult("Cannot change a request to pending.");
                    }
                    if (int.Parse(authenticatedUser.Id) == targetRequest.UserId)
                    {
                        return new BadRequestObjectResult("You cannot change your own request, Mr. Admin.");
                    }
                    // status of REJECTED and APPROVED can only be changed to completed.
                    // I imagine COMPLETED means the request has reached the end of the cycle
                    // and will be deleted from the system, or at least ignored forever after.
                    if ((targetRequest.Status.Equals("APPROVED") || targetRequest.Status.Equals("REJECTED"))
                        && !requestChangeObject.NewStatus.Equals("COMPLETED"))
                    {
                        return new BadRequestObjectResult("Can only mark approved or rejected requests as comnpleted.");
                    }
                    // admin can approve or reject pending requests.
                    // admin can approve or reject on hold and in progress requests.
                    // admin can mark approved and rejected requests as complete.
                    // admin is powerful.
                    targetRequest.Status = requestChangeObject.NewStatus;
                    _repository.Request.Update(targetRequest);
                    _repository.Save();
                    return new OkObjectResult(targetRequest);
                }
            }

        }

        
    }

}

