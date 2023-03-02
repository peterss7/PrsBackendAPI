
namespace Repository.DTOs;

public class RequestCreateDTO
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Justification { get; set; }
    public string? RejectionReason { get; set; }
    public string DeliveryMode { get; set; }
    public string SubmittedDate { get; set; }
    public string DateNeeded { get; set; }
    public string Status { get; set; }
    public decimal Total { get; set; }
    public int UserId { get; set; }
    

}
