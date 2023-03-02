

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

[Table("Request")]
public class Request
{
    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }
    [Column(Order = 1)]
    [StringLength(80)]
    public string Description { get; set; }
    [Column(Order = 2)]
    [StringLength(70)]
    public string Justification { get; set; }
    [Column(Order = 3)]
    [StringLength(80)]
    public string? RejectionReason { get; set; }
    [Column(Order = 4)]
    [StringLength(50)]
    public string DeliveryMode { get; set; }
    [Column(Order = 5)]
    [StringLength(50)]
    public string SubmittedDate { get; set; }
    [Column(Order = 6)]
    [StringLength(50)]
    public string DateNeeded { get; set; }
    [Column(Order = 7)]
    [StringLength(50)]
    public string Status { get; set; }
    [Column(Order = 8, TypeName = "decimal(11,2)")]
    public decimal Total { get; set; }
    [Column(Order = 9)]
    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }
    
    public User? User { get; set; }

    public ICollection<RequestLine> RequestLines { get; set; }

}

