using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

[Table("User")]
public class User
{
    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }
    [Column(Order = 1)]
    [StringLength(30)]
    public string Username { get; set; }
    [Column(Order = 2)]
    [StringLength(30)]
    public string Password { get; set; }
    [Column(Order = 3)]
    [StringLength(30)]
    public string Firstname { get; set; }
    [Column(Order = 4)]
    [StringLength(30)]
    public string Lastname { get; set; }
    [Column(Order = 5)]
    [StringLength(12)]
    public string? Phone { get; set; }
    [Column(Order = 6)]
    [StringLength(60)]
    public string? Email { get; set; }
    [Column(Order = 7)]
    public bool IsReviewer { get; set; }
    [Column(Order = 8)]
    public bool IsAdmin { get; set; }

    public ICollection<Request>? Requests{ get; set; }
}
