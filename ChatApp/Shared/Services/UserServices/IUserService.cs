using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Features.UserFeatures.Dtos;

namespace ChatApp.Shared.Services.UserServices
{
    public interface IUserService
    {
        public Task<UserResponseDto> GetUserByEmail(string email);
    }
}