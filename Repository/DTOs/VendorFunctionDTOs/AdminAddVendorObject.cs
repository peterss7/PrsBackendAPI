using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs.ModelDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOs.VendorFunctionDTOs;

public class AdminAddVendorObject
{    
    [Required]
    [MaxLength(30, ErrorMessage = "Vendor code is an invalid length.")]
    public string Code { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "VendorName is an invalid length")]
    public string Name { get; set; }
    [Required]
    [MaxLength(30, ErrorMessage = "Address is an invalid length.")]
    public string Address { get; set; }
    [MaxLength(30, ErrorMessage = "City name is an invalid length.")]
    public string City { get; set; }
    [Required]
    [RegularExpression(@"^[a-zA-Z]{2}", ErrorMessage = "State name is an abbreviation. Please use an abbreviation.")]
    public string State { get; set; }
    [Required]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Zipcode is not in the proper format.")]
    public string Zip { get; set; }
    [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Phone Number is not in the proper format.")]
    public string? Phone { get; set; }
    [RegularExpression(@"^[a-zA-z0-9]+@[a-zA-Z]+.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
    [MaxLength(255, ErrorMessage = "This email address is too long. Threrefore invalid.")]
    public string? Email { get; set; }
    [Required]
    [MaxLength(30, ErrorMessage = "Username is invalid length.")]
    public string Username { get; set; }
    [Required]
    [MaxLength(30, ErrorMessage = "Password is invalid length.")]
    public string Password { get; set; }

}
