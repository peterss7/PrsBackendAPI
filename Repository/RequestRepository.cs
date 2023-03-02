using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class RequestRepository : RepositoryBase<Request>, IRequestRepository
{
    public RequestRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    // recalculate will be called by controllers after a requestline is altered, added, or deleted, or the price of a product changes
    public Request Recalculate(Request request, IEnumerable<RequestLine> requestLine, IEnumerable<Product> product)
    {

        decimal total = 0;
        
        foreach(RequestLine _requestLine in requestLine)
        {
            foreach(Product _product in product)
            {
                if (_requestLine.ProductId == _product.Id)
                {
                    total += _product.Price;
                }
            }
        }

        var _request = new Request
        {
            Id = request.Id,
            Description = request.Description,
            Justification = request.Justification,
            RejectionReason = request.RejectionReason,
            DeliveryMode = request.DeliveryMode,
            SubmittedDate = request.SubmittedDate,
            DateNeeded = request.DateNeeded,
            Status = request.Status,
            Total = total,
            UserId = request.UserId
        };

        
        // updated request is passed back to the controller
        return request;
    }

}