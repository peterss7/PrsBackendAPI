using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrsUtilities.UserUrl;
using Repository.DTOs;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

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

    public ActionResult<VendorDTO> Create(VendorDTO vendorDto)
    {
        string  code, name, address, state, zip;
        string? city = null;
        string? phone = null;
        string? email = null;

        if (!string.IsNullOrEmpty(vendorDto.Code))
        {
            code = vendorDto.Code;
        }
        else
        {
            return new BadRequestObjectResult("Code cannot be null.");
        }
        if (!string.IsNullOrEmpty(vendorDto.Name))
        {
            name = vendorDto.Name;
        }
        else
        {
            return new BadRequestObjectResult("Name cannot be null.");
        }
        if (!string.IsNullOrEmpty(vendorDto.Address))
        {
            address = vendorDto.Address;
        }
        else
        {
            return new BadRequestObjectResult("Address cannot be null.");
        }
        if (!string.IsNullOrEmpty(vendorDto.City))
        {
            city = vendorDto.City;
        }        
        if (!string.IsNullOrEmpty(vendorDto.State))
        {
            state = vendorDto.State;
        }
        else
        {
            return new BadRequestObjectResult("State cannot be null");
        }
        if (!string.IsNullOrEmpty(vendorDto.Zip))
        {
            zip = vendorDto.Zip;
        }
        else
        {
            return new BadRequestObjectResult("Zip cannot be null");
        }
        if (!string.IsNullOrEmpty(vendorDto.Phone))
        {
            phone = vendorDto.Phone;
        }
        if (!string.IsNullOrEmpty(vendorDto.Email))
        {
            email = vendorDto.Email;
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

    public ActionResult<VendorDTO> Delete(int id)
    {

        var deleteVendor = _repository.Vendor.FindByCondition(u => u.Id == id).FirstOrDefault();

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

