using Entities.Models;
using Repository.DTOs.ModelDTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public static class UserDTOService
{
    
    /*
    public static User GetUserFromDto(UserDTO userDto)
    {
        User newUser = new User
        {
            Id = int.Parse(userDto.Id),
            Username = userDto.Username,
            Firstname = userDto.Firstname,
            Lastname = userDto.Lastname,
            Phone = userDto.Phone,
            Email = userDto.Email,
            IsReviewer = bool.Parse(userDto.IsReviewer),
            IsAdmin = bool.Parse(userDto.IsAdmin)
        };

        return newUser;
    }
    */
    

    public static UserDTO GetDtoFromUser(User user)
    {
        Debug.WriteLine(user.Firstname);

        UserDTO returnedDto = new UserDTO
        {
            Id = user.Id.ToString(),
            Username = user.Username,
            Password = "**********",
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Phone = user.Phone,
            Email = user.Email,
            IsReviewer = user.IsReviewer.ToString(),
            IsAdmin = user.IsAdmin.ToString()
        };

        return returnedDto;
    }
    public static  List<UserDTO> GetDtosFromUsers(List<User> users)
    {

        List<UserDTO> usersDto = new List<UserDTO>();

        int temp = 0;
        foreach (User user in users)
        {
            usersDto.Add(GetDtoFromUser(user));
            Debug.WriteLine(usersDto[temp].Firstname);
            temp++;
        }

        foreach (UserDTO dto in usersDto)
        {
            Debug.WriteLine(dto.Firstname);
        }

        return usersDto;
    }
}
