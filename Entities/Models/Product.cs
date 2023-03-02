
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

[Table("Product")]
public class Product
{ 

    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }
    [Column(Order = 1)]
    [StringLength(30), Required]
    public string PartNumber { get; set; }
    [Column(Order = 2)]
    [StringLength(70), Required]
    public string Name { get; set; }
    [Column(Order = 3, TypeName = "decimal(11,2)"), Required]
    public decimal Price { get; set; }
    [Column(Order = 4)]
    [StringLength(50), Required]
    public string Unit { get; set; }
    [Column(Order = 5)]
    [StringLength(255)]
    public string? PhotoPath { get; set; }

    [ForeignKey(nameof(VendorId)), Required]
    [Column(Order = 6)]
    public int VendorId { get; set; }

    public Vendor Vendor { get; set; }

    public ICollection<RequestLine> RequestLine { get; set; }





}