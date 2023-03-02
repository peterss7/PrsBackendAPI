namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IVendorRepository Vendor { get; }
        IProductRepository Product { get; }
        IRequestRepository Request { get; }
        IRequestLineRepository RequestLine { get; }
        void Save();
    }
}