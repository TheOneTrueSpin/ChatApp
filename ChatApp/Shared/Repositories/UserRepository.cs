using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Database.Entities;
using ChatApp.Shared.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Shared.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<User?> GetUserByIdWithRoles(Guid id)
    {
        return await _dataContext.Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByIdentityProviderUIdWithRoles(string identityProviderUId)
    {
        return await _dataContext.Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.IdentityProviderUId == identityProviderUId);
    }
    public async Task<List<User>> GetUsersByUserIds(List<Guid> userIds)
    {
        return await _dataContext.Set<User>()
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
    }
    public void Add(User user)
    {
        _dataContext.Set<User>().Add(user);
    }

    public void Remove(User user)
    {
        _dataContext.Set<User>().Remove(user);
    }
}
