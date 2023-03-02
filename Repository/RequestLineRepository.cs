using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class RequestLineRepository : RepositoryBase<RequestLine>, IRequestLineRepository
{
    public RequestLineRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

   
}