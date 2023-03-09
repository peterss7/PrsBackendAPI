using Repository.DTOs.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOs.VendorFunctionDTOs;

public class AdminAddVendorObject
{
    public VendorDTO? Vendor { get; set; }
    public AuthenticationObject? User { get; set; }

}
