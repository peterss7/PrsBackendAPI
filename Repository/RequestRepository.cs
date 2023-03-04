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

}