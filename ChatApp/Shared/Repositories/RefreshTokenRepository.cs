using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Database.Entities;
using ChatApp.Shared.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Shared.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DataContext _dataContext;
    public RefreshTokenRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<RefreshToken?> GetById(Guid id)
    {
        return await _dataContext.Set<RefreshToken>()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public void Add(RefreshToken user)
    {
        _dataContext.Set<RefreshToken>().Add(user);
    }

    public void Remove(RefreshToken user)
    {
        _dataContext.Set<RefreshToken>().Remove(user);
    }
}
