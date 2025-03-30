using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;

namespace ChatApp.Shared.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> GetById(Guid id);
    public void Add(RefreshToken refreshToken);
    public void Remove(RefreshToken refreshToken);
}
