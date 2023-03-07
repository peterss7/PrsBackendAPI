
namespace Repository.DTOs;

public class RequestDTO
{
    public string? Id { get; set; }
    public string? Description { get; set; }
    public string? Justification { get; set; }
    // Nullable
    public string? RejectionReason { get; set; }
    public string? DeliveryMode { get; set; }
    public string? SubmittedDate { get; set; }
    public string? DateNeeded { get; set; }
    public string? Status { get; set; }
    public string? Total { get; set; }
    public string? UserId { get; set; }
    

}
