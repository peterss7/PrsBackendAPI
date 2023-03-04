
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Repository.DTOs;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Web;

namespace Services;

public  class ConditionService
{
    private IRepositoryWrapper _repository;

    public ConditionService(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    /*
    public List<T> FindByConditions<T>(string url, IRepositoryWrapper repository) where T : class
    {

 

        //var type = (T)new object();

        var type = "test";
        
        switch (type)
        {
            case "test":

                /*
                break;
            case VendorDTO:
                break;
            case ProductDTO:
                break;
            case RequestDTO:
                break;
            case RequestLineDTO:
                break;
                */
            //default:
              //  break;

        //}
    //}
    
}
