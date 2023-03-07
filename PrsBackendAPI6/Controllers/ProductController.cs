using Contracts;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using Services;

namespace PrsBackendAPI6.Controllers;

[Route("/api/Product")]
[ApiController]
public class ProductController : ControllerBase
{


    private ProductService _repository;

    public ProductController(IRepositoryWrapper repository)
    {
        _repository = new ProductService(repository);
    }

    [HttpGet("search-products")]
    public ActionResult<List<ProductDTO>?> GetByCondition([FromQuery] ProductDTO productDto)
    {
        return _repository.FindByConditions(productDto);
    }

    [HttpGet]
    public ActionResult<List<ProductDTO>> FindAll()
    {
        return _repository.FindAll();
    }


    [HttpPost("Create")]
    public ActionResult<ProductDTO> Create([FromBody] ProductDTO productDto)
    {
        return _repository.Create(productDto);
    }

    [HttpDelete("Delete")]
    public ActionResult<ProductDTO> Delete([FromBody] int id)
    {
        return _repository.Delete(id);
    }

    [HttpPut("Update")]
    public ActionResult<ProductDTO> Update([FromBody] ProductDTO productDTO)
    {
        return _repository.Update(productDTO);
    }


}

