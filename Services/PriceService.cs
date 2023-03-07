using Contracts;
using Entities.Models;
using Repository;
using Repository.DTOs;

namespace Services
{
    public class PriceService
    {

        private IRepositoryWrapper _repository;

        public PriceService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public void RecalculateRequestTotal(RequestLineDTO requestLine)
        {
            List<RequestLine> relevantRequestLines = _repository.RequestLine.FindByCondition(rid => rid.RequestId == int.Parse(requestLine.Id)).ToList();

            decimal newTotal = 0;

            foreach (RequestLine _requestLine in relevantRequestLines)
            {
                Product? relevantProduct = _repository.Product.FindByCondition(pid => pid.Id == _requestLine.Id)
                    .FirstOrDefault();
                if (relevantProduct != null)
                {
                    newTotal += relevantProduct.Price
                        * _requestLine.Quantity;
                }
            }

            var targetRequest = _repository.Request.FindByCondition(request => request.Id == int.Parse(requestLine.Id)).FirstOrDefault();

            if (targetRequest != null)
            {
                var updatedRequest = new Request
                {
                    Id = targetRequest.Id,
                    Description = targetRequest.Description,
                    Justification = targetRequest.Justification,
                    RejectionReason = targetRequest.RejectionReason,
                    DeliveryMode = targetRequest.DeliveryMode,
                    SubmittedDate = targetRequest.SubmittedDate,
                    DateNeeded = targetRequest.DateNeeded,
                    Status = targetRequest.Status,
                    Total = newTotal,
                    UserId = targetRequest.UserId
                };

                _repository.Request.Update(updatedRequest);
            }

        }



    }
}