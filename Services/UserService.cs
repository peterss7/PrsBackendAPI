using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using System.Diagnostics;
using System.Linq.Expressions;


namespace Services;

public class UserService
{

    private IRepositoryWrapper _repository;

    public UserService(IRepositoryWrapper repository)
    {
        _repository = repository;
    }    

    public ActionResult<List<UserDTO>?> FindByConditions(UserSearchObject userSearch)
    {
        List<Expression<Func<User, bool>>> expressions = new List<Expression<Func<User, bool>>>();
        
        if (!string.IsNullOrEmpty(userSearch.Id)){
            expressions.Add(u => u.Id == int.Parse(userSearch.Id));            
        }
        if (!string.IsNullOrEmpty(userSearch.Idgt))
        {
            expressions.Add(u => u.Id > int.Parse(userSearch.Idgt));
        }
        if (!string.IsNullOrEmpty(userSearch.Idlt))
        {
            expressions.Add(u => u.Id < int.Parse(userSearch.Idlt));
        }
        if (!string.IsNullOrEmpty(userSearch.LnLowerLetter))
        {
            string lowerLetter = userSearch.LnLowerLetter;
            if (lowerLetter.Length > 1)
            {
                return new BadRequestObjectResult("invalid search param LastnameLowerLetter. It is longer than one letter.");
            }
            else
            {
                expressions.Add(u => LastnameLowerIds(lowerLetter).Contains(u.Id));
            }            
        }
        if (!string.IsNullOrEmpty(userSearch.LnUpperLetter))
        {
            string upperLetter = userSearch.LnUpperLetter;
            if (upperLetter.Length > 1)
            {
                return new BadRequestObjectResult("invalid search param LastnameUpperLetter. It is longer than one letter.");
            }
            else
            {
                expressions.Add(u => LastnameUpperIds(upperLetter).Contains(u.Id));
            }
        }
        if (!string.IsNullOrEmpty(userSearch.Firstname))
        {
            expressions.Add(u => u.Firstname == userSearch.Firstname);
        }
        if (!string.IsNullOrEmpty(userSearch.Lastname))
        {
            expressions.Add(u => u.Lastname == userSearch.Lastname);
        }
        if (!string.IsNullOrEmpty(userSearch.Email))
        {
            expressions.Add(u => u.Email == userSearch.Email);
        }
        if (!string.IsNullOrEmpty(userSearch.Phone))
        {
            expressions.Add(u => u.Phone == userSearch.Phone);
        }        
        if (!string.IsNullOrEmpty(userSearch.IsReviewer))
        {            
            try
            {
                expressions.Add(u => u.IsReviewer == bool.Parse(userSearch.IsReviewer));
            }
            catch
            {
                return new BadRequestObjectResult($"is reviewer was an invalid bool string: {userSearch.IsReviewer}");
            }
        }
        if (!string.IsNullOrEmpty(userSearch.IsAdmin))
        {
            try
            {
                expressions.Add(u => u.IsAdmin == bool.Parse(userSearch.IsAdmin));
            }
            catch
            {
                return new BadRequestObjectResult($"is admin was an invalid  bool string: {userSearch.IsAdmin}");
            }
        }        
        
        List<User> foundUsers = _repository.User.FindByConditions(expressions).ToList();
        if (foundUsers.Count > 0)
        {
            return new OkObjectResult(UserDTOService.GetDtosFromUsers(foundUsers));
        }
        else
        {
            return new NotFoundObjectResult("No users were found");
        }
        
        
    }
    
    public ActionResult<UserDTO> Authenticate (AuthenticationObject authentication)
    {
        return AuthenticationService.Authenticate(authentication, _repository);
    }

    public ActionResult<List<UserDTO>> FindAll()
    {        
        List<User> users = _repository.User.FindAll().ToList();
        
        if (users != null)
        {
            List<UserDTO> allUsers = UserDTOService.GetDtosFromUsers(users);
            
            foreach (UserDTO dto in allUsers)
            {
                Debug.WriteLine(dto.Firstname);
            }

            return new OkObjectResult(allUsers);
        }
        else
        {
            return new BadRequestObjectResult("No users were found");
        }
    }

    public ActionResult<UserDTO> Create(UserDTO userDto)
    {
        string username, password, firstname, lastname, isReviewer, isAdmin;
        string? phone = null;
        string? email = null;



        
        if (!string.IsNullOrEmpty(userDto.Username))
        {
            username = userDto.Username;
        }
        else
        {
            return new BadRequestObjectResult("Username cannot be null.");
        }
        if (!string.IsNullOrEmpty(userDto.Password))
        {
            password = userDto.Password;
        }
        else
        {
            return new BadRequestObjectResult("User password cannot be null."); 
        }
        if (!string.IsNullOrEmpty(userDto.Firstname))
        {
            firstname = userDto.Firstname;
        }
        else
        {
            return new BadRequestObjectResult("Firstname cannot be null."); 
        }
        if (!string.IsNullOrEmpty(userDto.Lastname))
        {
            lastname = userDto.Lastname;
        }
        else
        {
            return new BadRequestObjectResult("Lastname cannot be null."); 
        }
        if (!string.IsNullOrEmpty(userDto.Phone))
        {
            phone = userDto.Phone;
        }
        if (!string.IsNullOrEmpty(userDto.Email))
        {
            email = userDto.Email;
        }
        if (!string.IsNullOrEmpty(userDto.IsReviewer))
        {
            isReviewer = userDto.IsReviewer;
        }
        else
        {
            return new BadRequestObjectResult("Is reviewer cannot be null");
        }
        if (!string.IsNullOrEmpty(userDto.IsAdmin))
        {
            isAdmin = userDto.IsAdmin;
        }
        else
        {
            return new BadRequestObjectResult("IsAdmin cannot be null.");
        }

        if ((!userDto.IsReviewer.ToUpper().Equals("TRUE") && !userDto.IsReviewer.ToUpper().Equals("FALSE")) 
            || (!userDto.IsAdmin.ToUpper().Equals("TRUE") && !userDto.IsAdmin.ToUpper().Equals("FALSE")))
        {
            return new BadRequestObjectResult("One or both of the booleans is invalid");
        }


        var newUser = new User
        {   
            Username = username,
            Password = password,
            Firstname = firstname,
            Lastname = lastname,
            Phone = phone,
            Email = email,
            IsReviewer = bool.Parse(isReviewer),
            IsAdmin = bool.Parse(isAdmin)
        };
        _repository.User.Create(newUser);
        _repository.Save();

        int highestId = 0;
        List<User> allUsers = _repository.User.FindAll().ToList();
        foreach (User user in allUsers)
        {
            if (user.Id > highestId)
            {
                highestId = user.Id;
            }
        }

        UserDTO createdUserDto = UserDTOService.GetDtoFromUser(_repository.User.FindByCondition(u => u.Id == highestId).ToList()[0]);

        return new OkObjectResult(createdUserDto);
    }

    public ActionResult<UserDTO> Delete(int id)
    {

        var deleteUser = _repository.User.FindByCondition(u => u.Id == id).FirstOrDefault();

        if (deleteUser != null)
        {
            _repository.User.Delete(deleteUser);
            _repository.Save();
            return new OkObjectResult(UserDTOService.GetDtoFromUser(deleteUser));
        }
        else
        {
            return new BadRequestObjectResult("Cannot delete user. No user by that Id was found.");
        }
    }

    
    public ActionResult<UserDTO> Update(UserDTO userDto)
    {   
        User user = new User();
        

        if (userDto.Id == null)
        {
            return new BadRequestObjectResult("must include ID to update");
        }        
        else
        {
            user = _repository.User.FindByCondition(u => u.Id == int.Parse(userDto.Id)).FirstOrDefault();

            if (user == null)
            {
                return new NotFoundObjectResult("User of that Id was not found. Nothing was Updated.");
            }

            if (!string.IsNullOrEmpty(userDto.Username) && !userDto.Username.Equals("string"))
            {
                user.Username = userDto.Username;
            }
            if (!string.IsNullOrEmpty(userDto.Password) && !userDto.Password.Equals("string"))
            {
                user.Password = userDto.Password;
            }
            if (!string.IsNullOrEmpty(userDto.Firstname) && !userDto.Firstname.Equals("string"))
            {
                user.Firstname = userDto.Firstname;
            }
            if (!string.IsNullOrEmpty(userDto.Lastname) && !userDto.Lastname.Equals("string"))
            {
                user.Lastname = userDto.Lastname;
            }
            if (!string.IsNullOrEmpty(userDto.Phone) && !userDto.Phone.Equals("string"))
            {
                user.Phone = userDto.Phone;
            }
            if (!string.IsNullOrEmpty(userDto.Email) && !userDto.Email.Equals("string"))
            {
                user.Email = userDto.Email;
            }
            if (!string.IsNullOrEmpty(userDto.IsReviewer) && !userDto.IsReviewer.Equals("string")) 
            {
                user.IsReviewer = bool.Parse(userDto.IsReviewer);
            }
            if (!string.IsNullOrEmpty(userDto.IsAdmin) && !userDto.IsAdmin.Equals("string"))
            {
                user.IsAdmin = bool.Parse(userDto.IsAdmin);
            }

            _repository.User.Update(user);

            UserDTO returnedUser = UserDTOService.GetDtoFromUser(user);

            _repository.Save();            
            return new OkObjectResult(returnedUser);
        }
        
        
    }
    

    private List<int> LastnameUpperIds(string lowerLetter)
    {
        List<User> tempUsers = _repository.User.FindAll().ToList();
        List<int> tempInts = new List<int>();

        foreach (User user in tempUsers)
        {
            if (user.Lastname[0] >= lowerLetter[0])
            {
                tempInts.Add(user.Id);
            }
        }

        return tempInts;
    }

    private List<int> LastnameLowerIds(string upperLetter)
    {
        List<User> tempUsers = _repository.User.FindAll().ToList();
        List<int> tempInts = new List<int>();

        foreach (User user in tempUsers)
        {
            if (user.Lastname[0] <= upperLetter[0])
            {
                tempInts.Add(user.Id);
            }
        }

        return tempInts;
    }
    
 
}

