using Contracts;
using Entities.Models;

using System.Diagnostics;


namespace Services;

public class UserConditionService
{

    private IRepositoryWrapper _repository;

    public UserConditionService(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public List<User> FindByConditions(string url)
    {

        var baseUri = new Uri("http://localhost:5000/api/User/");
        var absoluteUrl = new Uri(baseUri, url);
        var queryString = absoluteUrl.Query;
        var queries = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryString);


        //{id}, {idgt}, {idlt}
        //{id}, null  , nulll
        //null, {idgt}, {idlt}
        //null, {idgt}, null
        //null, null  , {idlet}
        int? id = 0;
        int? idgt = 0;
        int idlt = 0;

        string usernameParam;
        string passwordParam;
        string firstNameParam;
        //{id}, {idgt}, {idlt}
        //{id}, null  , nulll
        //null, {idgt}, {idlt}
        //null, {idgt}, null
        //null, null  , {idlet}
        string lastname = "";
        string lastnameUpperLetter;
        string lastnameLowerLetter;
        string phoneParam;
        string emailParam;
        bool isReviewerParam;
        bool isAdminParam;

        foreach (var parameter in queries)
        {

            var key = parameter.Key;
            var value = parameter.Value;

            if (key == "Id")
            {
                id = int.Parse(value);
            }
            if (key == "idgt")
            {
                idgt = int.Parse(value);
            }
            if (key == "idlt")
            {
                idlt = int.Parse(value);
            }
            if (key == "Username")
            {
                usernameParam = value;
            }
            if (key == "Password")
            {
                passwordParam = value;
            }
            if (key == "Firstname")
            {
                firstNameParam = value;
            }
            if (key == "Lastname")
            {
                lastname = value;
            }
            if (key == "Phone")
            {
                phoneParam = value;
            }
            if (key == "Email")
            {
                emailParam = value;
            }
            if (key == "IsReviewer")
            {
                isReviewerParam = bool.Parse(value);
            }
            if (key == "IsAdmin")
            {
                isAdminParam = bool.Parse(value);
            }
        }

        Debug.WriteLine("Id: " + id);
        Debug.WriteLine("idgt: " + idgt);
        Debug.WriteLine("idlt: " + idlt);
        Debug.WriteLine("lastname: " + lastname);

        
        List<User> users = new List<User>();

        if (id != 0)
        { 
            users = _repository.User.FindByCondition(u => u.Id == id).ToList();
        }
        else if (idgt != 0)
        {
            users = _repository.User.FindByCondition(u => u.Id >= idgt).ToList();

            
            if (idlt != 0)
            {
                var userIdLower = _repository.User.FindByCondition(u => u.Id <= idlt).ToList();
                
                List<User> newUsers = users.Join(userIdLower,
                    users => users.Id,
                    userIdLower => userIdLower.Id,
                    (users, userIdLower) => new User
                    {
                        Id = users.Id,
                        Username = users.Username,
                        Password = users.Password,
                        Firstname = users.Firstname,
                        Lastname = users.Lastname,
                        Phone = users.Phone,
                        Email = users.Email,
                        IsReviewer = users.IsReviewer,
                        IsAdmin = users.IsAdmin

                    })
                    .ToList();
                users = newUsers;
            }       
            
        }
       
        return users;
    }

}
