using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOs;

public class UserSearchObject
{
    public string? Id { get; set; }
    public string? Username { get; set; }    
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? IsReviewer { get; set; }
    public string? IsAdmin { get; set; }    
    public string? Idgt { get; set; }
    public string? Idlt { get; set; }
    public string? LnLowerLetter { get; set; }
    public string? LnUpperLetter { get; set; }
}
