using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Repository.DTOs;

public class ProductCreateDTO
{
    public int Id { get; set; }
    public string PartNumber { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Unit { get; set; }
    public string? PhotoPath { get; set; }
    public int VendorId { get; set; }
}
