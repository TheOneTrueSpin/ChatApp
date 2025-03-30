using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Database.Entities;
using ChatApp.Shared.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Shared.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DataContext _dataContext;
    public RoleRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<List<Role>> GetRolesForNewUser()
    {
        return await _dataContext.Roles.Where(r => r.Name == "User").ToListAsync();
    }

}
