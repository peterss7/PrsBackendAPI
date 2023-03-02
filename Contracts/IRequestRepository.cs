

using Entities.Models;

namespace Contracts;

public interface IRequestRepository : IRepositoryBase<Request>
{
    public void Recalculate(Request request, IEnumerable<RequestLine> requestLine, IEnumerable<Product> product);   
}