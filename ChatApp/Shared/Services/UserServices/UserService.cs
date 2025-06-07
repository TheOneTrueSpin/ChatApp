using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ChatApp.Database.Entities;
using ChatApp.Features.UserFeatures.Dtos;
using ChatApp.Shared.Exceptions;
using ChatApp.Shared.Repositories.Interfaces;

namespace ChatApp.Shared.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<UserResponseDto> GetUserByEmail(string email)
    {
        User? user = await _userRepository.GetUserByEmail(email);
        if(user is null)
        {
            throw new ApiException("User by that email does not exist", HttpStatusCode.BadRequest);    
        }
        UserResponseDto userResponse = new UserResponseDto() { UserId = user.Id };
        return userResponse;
    }
}
