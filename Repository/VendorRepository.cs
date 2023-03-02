using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class VendorRepository : RepositoryBase<Vendor>, IVendorRepository
    {
        public VendorRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}