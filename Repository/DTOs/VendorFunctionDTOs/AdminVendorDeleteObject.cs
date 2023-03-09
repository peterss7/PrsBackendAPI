using Repository.DTOs.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOs.VendorFunctionDTOs;

public class AdminVendorDeleteObject
{

    public AuthenticationObject? AuthUser {get; set;}
    public int Id { get; set;}

}
