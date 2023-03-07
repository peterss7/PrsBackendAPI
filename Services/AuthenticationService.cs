using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public static class AuthenticationService
{
    public static ActionResult<UserDTO> Authenticate(AuthenticationObject authObject, IRepositoryWrapper _repository)
    {        

        if (authObject == null || string.IsNullOrEmpty(authObject.Username)
          || string.IsNullOrEmpty(authObject.Password))
        {
            return new BadRequestObjectResult("Authentication object contains null values.");
        }
        else
        {
            List<Expression<Func<User, bool>>> expressions = new List<Expression<Func<User, bool>>>();
            expressions.Add(u => u.Username == authObject.Username);
            expressions.Add(u => u.Password == authObject.Password);

            User? authenticatedUser = _repository.User.FindByConditions(expressions).FirstOrDefault();

            if (authenticatedUser == null)
            {
                return new NotFoundObjectResult("Bad user/password combination.");
            }
            string testedPassword = authenticatedUser.Password;

            if (!testedPassword.Equals(authObject.Password))
            {
                return new NotFoundObjectResult("bad user/password cominbation.");
            }
            else
            {
                return new OkObjectResult(UserDTOService.GetDtoFromUser(authenticatedUser));
            }
        }
    }
}
