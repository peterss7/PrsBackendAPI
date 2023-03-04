using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.DTOs;

public class RequestLineDTO
{
    public int Id { get; set; }    
    public int RequestId { get; set; }    
    public int ProductId { get; set; }    
    public int Quantity { get; set; }
}
