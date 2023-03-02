

using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private IUserRepository _user;
        private IVendorRepository _vendor;
        private IProductRepository _product;
        private IRequestRepository _request;
        private IRequestLineRepository _requestLine;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }

                return _user;
            }
        }

        public IVendorRepository Vendor
        {
            get
            {
                if (_vendor == null)
                {
                    _vendor = new VendorRepository(_repoContext);
                }

                return _vendor;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_repoContext);
                }

                return _product;
            }
        }

        public IRequestRepository Request
        {
            get
            {
                if (_request == null)
                {
                    _request = new RequestRepository(_repoContext);
                }

                return _request;
            }
        }

        public IRequestLineRepository RequestLine
        {
            get
            {
                if (_requestLine == null)
                {
                    _requestLine = new RequestLineRepository(_repoContext);
                }

                return _requestLine;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}