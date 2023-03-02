
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

[Table("RequestLine")]
public class RequestLine
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 0)]
    [Required]
    public int Id { get; set; }
    [Column(Order = 1)]
    [ForeignKey(nameof(RequestId))]
    public int RequestId { get; set; }
    [Column(Order = 2)]
    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
    [Column(Order = 3)]
    public int Quantity { get; set; }

    public Product Product { get; set; }
    public Request Request { get; set; }
}
