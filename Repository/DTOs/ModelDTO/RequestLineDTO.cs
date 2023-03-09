using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.DTOs.ModelDTO;

public class RequestLineDTO
{
    public string? Id { get; set; }
    public string? RequestId { get; set; }
    public string? ProductId { get; set; }
    public string? Quantity { get; set; }
}
