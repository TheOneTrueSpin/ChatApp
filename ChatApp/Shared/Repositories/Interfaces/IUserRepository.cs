using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;

namespace ChatApp.Shared.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUserByIdWithRoles(Guid id);
    public Task<User?> GetUserByIdentityProviderUIdWithRoles(string identityProviderUId);
    public Task<List<User>> GetUsersByUserIds(List<Guid> userIds);
    public void Add(User user);
    public void Remove(User user);
    public Task<User?> GetUserByEmail(string email);
}
