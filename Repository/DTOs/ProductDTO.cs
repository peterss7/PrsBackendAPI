using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Repository.DTOs;

public class ProductDTO
{
    public string? Id { get; set; }
    public string? PartNumber { get; set; }
    public string? Name { get; set; }
    public string? Price { get; set; }
    public string? Unit { get; set; }
    // nullable in model
    public string? Photopath { get; set; }
    public string? VendorId { get; set; }
}
