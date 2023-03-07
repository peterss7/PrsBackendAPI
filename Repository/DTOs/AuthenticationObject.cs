using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOs;

public class AuthenticationObject
{
    public string Username { get; set; }
    public string Password { get; set; }
}
