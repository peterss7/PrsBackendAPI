using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using Repository.DTOs.ModelDTO;
using Repository.DTOs.VendorFunctionDTOs;
using System.Linq.Expressions;

namespace Services;

public class VendorService
{

    private IRepositoryWrapper _repository;

    public VendorService(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public ActionResult<List<VendorDTO>?> FindByConditions(VendorDTO vendorDto)
    {
        List<Expression<Func<Vendor, bool>>> expressions = new List<Expression<Func<Vendor, bool>>>();

        if (!string.IsNullOrEmpty(vendorDto.Id))
        {
            expressions.Add(u => u.Id == int.Parse(vendorDto.Id));
        }
        if (!string.IsNullOrEmpty(vendorDto.Code))
        {
            expressions.Add(u => u.Code == vendorDto.Code);
        }
        if (!string.IsNullOrEmpty(vendorDto.Name))
        {
            expressions.Add(u => u.Name == vendorDto.Name);
        }        
        if (!string.IsNullOrEmpty(vendorDto.Address))
        {
            expressions.Add(u => u.Address == vendorDto.Address);
        }
        if (!string.IsNullOrEmpty(vendorDto.City))
        {
            expressions.Add(u => u.City == vendorDto.City);
        }
        if (!string.IsNullOrEmpty(vendorDto.State))
        {
            expressions.Add(u => u.State == vendorDto.State);
        }
        if (!string.IsNullOrEmpty(vendorDto.Zip))
        {
            expressions.Add(u => u.Zip == vendorDto.Zip);
        }
        if (!string.IsNullOrEmpty(vendorDto.Phone))
        {
            expressions.Add(u => u.Phone == vendorDto.Phone);
        }
        if (!string.IsNullOrEmpty(vendorDto.Email))
        {
            expressions.Add(u => u.Email == vendorDto.Email);
        }                

        List<Vendor> foundVendors = _repository.Vendor.FindByConditions(expressions).ToList();
        if (foundVendors.Count > 0)
        {
            return new OkObjectResult(GetDtosFromVendors(foundVendors));
        }
        else
        {
            return new NotFoundObjectResult("No vendors were found");
        }


    }

    public ActionResult<List<VendorDTO>> FindAll()
    {
        List<Vendor> vendors = _repository.Vendor.FindAll().ToList();

        if (vendors != null)
        {
            List<VendorDTO> allVendors = GetDtosFromVendors(vendors);
            return new OkObjectResult(allVendors);
        }
        else
        {
            return new BadRequestObjectResult("No vendors were found");
        }
    }

    public ActionResult<VendorDTO> Create(AdminAddVendorObject? adminVendor)
    {
        string  code, name, address, state, zip;
        string? city = null;
        string? phone = null;
        string? email = null;
     
        AuthenticationObject authObject = new AuthenticationObject
        {
            Username = adminVendor.User.Username,
            Password = adminVendor.User.Password
        };

        bool isUser = AuthenticationService.Authenticate(authObject, _repository);

        if (!isUser)
        {
            string errorMsg = $"Bad username/password combination: {authObject.Username}... {authObject.Password} in vendor service";
            return new BadRequestObjectResult(errorMsg);
        }

        UserDTO user = UserDTOService.GetDtoFromUser(_repository.User.FindByCondition(u => u.Username == authObject.Username).FirstOrDefault());

        if (!bool.Parse(user.IsAdmin))
        {
            return new BadRequestObjectResult("You are not authorized to add a vendor.");
        }

        if (!string.IsNullOrEmpty(adminVendor.Vendor.Code))
        {
            code = adminVendor.Vendor.Code;
        }
        else
        {
            return new BadRequestObjectResult("Code cannot be null.");
        }
        if (!string.IsNullOrEmpty(adminVendor.Vendor.Name))
        {
            name = adminVendor.Vendor.Name;
        }
        else
        {
            return new BadRequestObjectResult("Name cannot be null.");
        }
        if (!string.IsNullOrEmpty(adminVendor.Vendor.Address))
        {
            address = adminVendor.Vendor.Address;
        }
        else
        {
            return new BadRequestObjectResult("Address cannot be null.");
        }
        if (!string.IsNullOrEmpty(adminVendor.Vendor.City))
        {
            city = adminVendor.Vendor.City;
        }        
        if (!string.IsNullOrEmpty(adminVendor.Vendor.State))
        {
            state = adminVendor.Vendor.State;
        }
        else
        {
            return new BadRequestObjectResult("State cannot be null");
        }
        if (!string.IsNullOrEmpty(adminVendor.Vendor.Zip))
        {
            zip = adminVendor.Vendor.Zip;
        }
        else
        {
            return new BadRequestObjectResult("Zip cannot be null");
        }
        if (!string.IsNullOrEmpty(adminVendor.Vendor.Phone))
        {
            phone = adminVendor.Vendor.Phone;
        }
        if (!string.IsNullOrEmpty(adminVendor.Vendor.Email))
        {
            email = adminVendor.Vendor.Email;
        }        

        var newVendor = new Vendor
        {
            Code = code,
            Name = name,
            Address = address,
            City = city,
            State = state,
            Zip = zip,
            Phone = phone,
            Email = email
        };
        _repository.Vendor.Create(newVendor);
        _repository.Save();

        int highestId = 0;
        List<Vendor> allVendors = _repository.Vendor.FindAll().ToList();
        foreach (Vendor vendor in allVendors)
        {
            if (vendor.Id > highestId)
            {
                highestId = vendor.Id;
            }
        }

        VendorDTO createdVendorDTO = GetDtoFromVendor(_repository.Vendor.FindByCondition(u => u.Id == highestId).ToList()[0]);

        return new OkObjectResult(createdVendorDTO);
    }

    public ActionResult<VendorDTO> Delete(AdminVendorDeleteObject adminVendor)
    {

        AuthenticationObject authObject = new AuthenticationObject
        {
            Username = adminVendor.AuthUser.Username,
            Password = adminVendor.AuthUser.Password
        };

        bool IsUser = AuthenticationService.Authenticate(authObject, _repository);
        
        if (!IsUser)
        {
            return new NotFoundObjectResult("Login credentials were not accepted.");
        }

        UserDTO user = UserDTOService.GetDtoFromUser(_repository.User.FindByCondition(u => u.Username == authObject.Username).FirstOrDefault());

        if (!bool.Parse(user.IsAdmin))
        {
            return new BadRequestObjectResult("You are not authorized to delete vendors.");
                
        }

        Vendor? deleteVendor = _repository.Vendor.FindByCondition(u => u.Id == adminVendor.Id).FirstOrDefault();

        if (deleteVendor != null)
        {
            _repository.Vendor.Delete(deleteVendor);
            _repository.Save();
            return new OkObjectResult(GetDtoFromVendor(deleteVendor));
        }
        else
        {
            return new BadRequestObjectResult("Cannot delete Vendor. No user by that Id was found.");
        }
    }


    public ActionResult<VendorDTO> Update(VendorDTO vendorDto)
    {

        Vendor? vendor = new Vendor();

        if (vendorDto.Id == null)
        {
            return new BadRequestObjectResult("must include ID to update");
        }
        else
        {
            vendor = _repository.Vendor.FindByCondition(v => v.Id == int.Parse(vendorDto.Id)).FirstOrDefault();

            if (vendor == null)
            {
                return new NotFoundObjectResult("Vendor of that Id was not found. Nothing was Updated.");
            }

            if (!string.IsNullOrEmpty(vendorDto.Code) && !vendorDto.Code.Equals("string"))
            {
                vendor.Code = vendorDto.Code;
            }
            if (!string.IsNullOrEmpty(vendorDto.Name) && !vendorDto.Name.Equals("string"))
            {
                vendor.Name = vendorDto.Name;
            }
            if (!string.IsNullOrEmpty(vendorDto.Address) && !vendorDto.Address.Equals("string"))
            {
                vendor.Address = vendorDto.Address;
            }
            if (!string.IsNullOrEmpty(vendorDto.City) && !vendorDto.City.Equals("string"))
            {
                vendor.City = vendorDto.City;
            }
            if (!string.IsNullOrEmpty(vendorDto.State) && !vendorDto.State.Equals("string"))
            {
                vendor.State = vendorDto.State.ToLower();
            }
            if (!string.IsNullOrEmpty(vendorDto.Zip) && !vendorDto.Zip.Equals("string"))
            {
                vendor.Zip = vendorDto.Zip;
            }
            if (!string.IsNullOrEmpty(vendorDto.Phone) && !vendorDto.Phone.Equals("string"))
            {
                vendor.Phone = vendorDto.Phone;
            }
            if (!string.IsNullOrEmpty(vendorDto.Email) && !vendorDto.Email.Equals("string"))
            {
                vendor.Email = vendorDto.Email;
            }

            _repository.Vendor.Update(vendor);
            _repository.Save();

            VendorDTO returnedVendor = GetDtoFromVendor(_repository.Vendor.FindByCondition(v => v.Id == int.Parse(vendorDto.Id)).FirstOrDefault());


            
            
            return new OkObjectResult(returnedVendor);
        }

    }

    public VendorDTO GetDtoFromVendor(Vendor vendor)
    {
        VendorDTO returnedDto = new VendorDTO
        {
            Id = vendor.Id.ToString(),
            Code = vendor.Code,
            Name = vendor.Name,
            Address = vendor.Address,
            City = vendor.Phone,
            State = vendor.State,
            Zip = vendor.Zip,
            Phone = vendor.Phone,
            Email = vendor.Email
        };

        return returnedDto;
    }

    public List<VendorDTO> GetDtosFromVendors(List<Vendor> vendors)
    {

        List<VendorDTO> vendorsDto = new List<VendorDTO>();
        
        foreach (Vendor vendor in vendors)
        {
            vendorsDto.Add(GetDtoFromVendor(vendor));
        }
        
        return vendorsDto;
    }
    
}

