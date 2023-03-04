

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

[Table("Vendor")]
public class Vendor {


    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }
    [Column(Order = 1)]
    [StringLength(30)]
    public string Code { get; set; }
    [Column(Order = 2)]
    [StringLength(100)]
    public string Name { get; set; }
    [Column(Order = 3)]
    [StringLength(30)]
    public string Address { get; set; }
    [Column(Order = 4)]
    [StringLength(30)]
    public string? City { get; set; }
    [Column(Order = 5)]
    [StringLength(2)]
    public string State { get; set; }
    [Column(Order = 7)]
    [StringLength(30)]
    public string Zip { get; set; }
    [Column(Order = 8)]
    [StringLength(30)]
    public string? Phone { get; set; }
    [Column(Order = 9)]
    [StringLength(255)]
    public string? Email { get; set; }

    public ICollection<Product>? Products { get; set; }

}
