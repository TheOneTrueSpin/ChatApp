using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;

namespace ChatApp.Shared.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public void Add(Chat chat);
        public void Remove(Chat chat);
    }
}