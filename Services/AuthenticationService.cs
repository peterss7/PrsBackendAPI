using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using Repository.DTOs.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public static class AuthenticationService
{
    public static bool Authenticate(AuthenticationObject authObject, IRepositoryWrapper _repository)
    {        

        if (authObject == null || string.IsNullOrEmpty(authObject.Username)
          || string.IsNullOrEmpty(authObject.Password))
        {
            return false;
        }
        else
        {
            List<Expression<Func<User, bool>>> expressions = new List<Expression<Func<User, bool>>>();
            expressions.Add(u => u.Username == authObject.Username);
            expressions.Add(u => u.Password == authObject.Password);

            User? authenticatedUser = _repository.User.FindByConditions(expressions).FirstOrDefault();

            if (authenticatedUser == null)
            {
                return false;
            }
            string testedPassword = authenticatedUser.Password;

            if (!testedPassword.Equals(authObject.Password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
