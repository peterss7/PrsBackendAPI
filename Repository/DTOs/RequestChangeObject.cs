using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOs;

public class RequestChangeObject
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? RequestId { get; set; }
    public string? NewStatus { get; set; }

}
