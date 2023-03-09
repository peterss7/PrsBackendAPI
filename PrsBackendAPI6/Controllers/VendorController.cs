using Contracts;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs.ModelDTO;
using Repository.DTOs.VendorFunctionDTOs;
using Services;

namespace PrsBackendAPI6.Controllers;

[Route("/api/Vendor")]
[ApiController]
public class VendorController : ControllerBase
{


    private VendorService _repository;
    public VendorController(IRepositoryWrapper repository)
    {
        _repository = new VendorService(repository);
    }

    [HttpGet("search-vendors")]    
    public ActionResult<List<VendorDTO>?> GetByCondition([FromQuery] VendorDTO vendorDto)
    {
        return _repository.FindByConditions(vendorDto);
    }

    [HttpGet]
    public ActionResult<List<VendorDTO>> FindAll()
    {
        return _repository.FindAll();
    }


    [HttpPost("Create")]
    public ActionResult<VendorDTO> Create([FromBody] AdminAddVendorObject adminVendorObject)
    {
        return _repository.Create(adminVendorObject);
    }

    [HttpDelete("Delete")]
    public ActionResult<VendorDTO> Delete([FromBody] AdminVendorDeleteObject adminVendorObject)
    {
        return _repository.Delete(adminVendorObject);
    }

    [HttpPut("Update")]
    public ActionResult<VendorDTO> Update([FromBody] VendorDTO vendorDTO)
    {
        return _repository.Update(vendorDTO);
    }


}

