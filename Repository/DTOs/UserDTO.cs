

using System.Text.Json.Serialization;

namespace Repository.DTOs;

public class UserDTO
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Phone  { get; set; }
    public string? Email { get; set; }
    public string? IsReviewer { get; set; }
    public string? IsAdmin { get; set; }
}
