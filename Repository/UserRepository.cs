using Contracts;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{    
    public UserRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {        
    }
   
}