using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using System.Linq.Expressions;


namespace Services;

public class ProductService
{

    private IRepositoryWrapper _repository;

    public ProductService(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public ActionResult<List<ProductDTO>?> FindByConditions(ProductDTO productDto)
    {
        List<Expression<Func<Product, bool>>> expressions = new List<Expression<Func<Product, bool>>>();

        if (!string.IsNullOrEmpty(productDto.Id))
        {
            expressions.Add(p => p.Id == int.Parse(productDto.Id));
        }
        if (!string.IsNullOrEmpty(productDto.PartNumber))
        {
            expressions.Add(p => p.PartNumber == productDto.PartNumber);
        }
        if (!string.IsNullOrEmpty(productDto.Name))
        {
            expressions.Add(p => p.Name == productDto.Name);
        }
        if (!string.IsNullOrEmpty(productDto.Price))
        {
            expressions.Add(p => p.Price == decimal.Parse(productDto.Price));
        }
        if (!string.IsNullOrEmpty(productDto.Unit))
        {
            expressions.Add(p => p.Unit == productDto.Unit);
        }
        if (!string.IsNullOrEmpty(productDto.Photopath))
        {
            expressions.Add(p => p.PhotoPath == productDto.Photopath);
        }
        if (!string.IsNullOrEmpty(productDto.VendorId))
        {
            expressions.Add(p => p.VendorId == int.Parse(productDto.VendorId));
        }        

        List<Product> foundProducts = _repository.Product.FindByConditions(expressions).ToList();
        if (foundProducts.Count > 0)
        {
            return new OkObjectResult(GetDtosFromProducts(foundProducts));
        }
        else
        {
            return new NotFoundObjectResult("No products were found");
        }


    }

    public ActionResult<List<ProductDTO>> FindAll()
    {
        List<Product> products = _repository.Product.FindAll().ToList();

        if (products != null)
        {
            List<ProductDTO> allProducts = GetDtosFromProducts(products);
            return new OkObjectResult(allProducts);
        }
        else
        {
            return new BadRequestObjectResult("No products were found");
        }
    }

    public ActionResult<ProductDTO> Create(ProductDTO productDto)
    {
        string partNumber, name, price, unit, vendorId;
        string? photopath = null;        

        if (!string.IsNullOrEmpty(productDto.PartNumber))
        {
            partNumber = productDto.PartNumber;
        }
        else
        {
            return new BadRequestObjectResult("Partnumber cannot be null.");
        }
        if (!string.IsNullOrEmpty(productDto.Name))
        {
            name = productDto.Name;
        }
        else
        {
            return new BadRequestObjectResult("Name cannot be null.");
        }
        if (!string.IsNullOrEmpty(productDto.Price))
        {
            price = productDto.Price;
        }
        else
        {
            return new BadRequestObjectResult("Price cannot be null.");
        }
        if (!string.IsNullOrEmpty(productDto.Unit))
        {
            unit = productDto.Unit;
        }
        else
        {
            return new BadRequestObjectResult("Unit cannot be null");
        }
        if (!string.IsNullOrEmpty(productDto.Photopath))
        {
            photopath = productDto.Photopath;
        }        
        if (!string.IsNullOrEmpty(productDto.VendorId))
        {
            vendorId = productDto.VendorId;
        }
        else
        {
            return new BadRequestObjectResult("VendorId cannot be null");
        }        

        var newProduct = new Product
        {
            PartNumber = partNumber,
            Name = name,
            Price = decimal.Parse(price),
            Unit = unit,
            PhotoPath = photopath,
            VendorId = int.Parse(vendorId)            
        };
        _repository.Product.Create(newProduct);
        _repository.Save();

        int highestId = 0;
        List<Product> allProducts = _repository.Product.FindAll().ToList();
        foreach (Product product in allProducts)
        {
            if (product.Id > highestId)
            {
                highestId = product.Id;
            }
        }

        ProductDTO createdProductDTO = GetDtoFromProduct(_repository.Product.FindByCondition(p => p.Id == highestId).ToList()[0]);

        return new OkObjectResult(createdProductDTO);
    }

    public ActionResult<ProductDTO> Delete(int id)
    {

        var deleteProduct = _repository.Product.FindByCondition(p => p.Id == id).FirstOrDefault();

        if (deleteProduct != null)
        {
            _repository.Product.Delete(deleteProduct);
            _repository.Save();
            return new OkObjectResult(GetDtoFromProduct(deleteProduct));
        }
        else
        {
            return new BadRequestObjectResult("Cannot delete Product. No user by that Id was found.");
        }
    }


    public ActionResult<ProductDTO> Update(ProductDTO productDto)
    {

        Product? product = new Product();

        if (productDto.Id == null)
        {
            return new BadRequestObjectResult("must include ID to update");
        }
        else
        {
            product = _repository.Product.FindByCondition(v => v.Id == int.Parse(productDto.Id)).FirstOrDefault();

            if (product == null)
            {
                return new NotFoundObjectResult("Product of that Id was not found. Nothing was Updated.");
            }

            if (!string.IsNullOrEmpty(productDto.PartNumber) && !productDto.PartNumber.Equals("string"))
            {
                product.PartNumber = productDto.PartNumber;
            }            
            if (!string.IsNullOrEmpty(productDto.Name) && !productDto.Name.Equals("string"))
            {
                product.Name = productDto.Name;
            }
            if (!string.IsNullOrEmpty(productDto.Price) && !productDto.Price.Equals("string"))
            {
                product.Price = decimal.Parse(productDto.Price);
            }
            if (!string.IsNullOrEmpty(productDto.Unit) && !productDto.Unit.Equals("string"))
            {
                product.Unit = productDto.Unit.ToLower();
            }
            if (!string.IsNullOrEmpty(productDto.Photopath) && !productDto.Photopath.Equals("string"))
            {
                product.PhotoPath = productDto.Photopath;
            }            
            if (!string.IsNullOrEmpty(productDto.VendorId) && !productDto.VendorId.Equals("string"))
            {
                product.VendorId = int.Parse(productDto.VendorId);
            }

            _repository.Product.Update(product);
            _repository.Save();

            ProductDTO returnedProduct = GetDtoFromProduct(_repository.Product.FindByCondition(v => v.Id == int.Parse(productDto.Id)).FirstOrDefault());
            return new OkObjectResult(returnedProduct);
        }

    }

    public ProductDTO GetDtoFromProduct(Product product)
    {
        ProductDTO returnedDto = new ProductDTO
        {
            Id = product.Id.ToString(),
            PartNumber = product.PartNumber,
            Name = product.Name,
            Price = product.Price.ToString(),
            Unit = product.Unit,
            Photopath = product.PhotoPath,
            VendorId = product.VendorId.ToString(),            
        };

        return returnedDto;
    }

    public List<ProductDTO> GetDtosFromProducts(List<Product> products)
    {

        List<ProductDTO> productsDto = new List<ProductDTO>();

        foreach (Product product in products)
        {
            productsDto.Add(GetDtoFromProduct(product));
        }

        return productsDto;
    }

}

