using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Database.Entities;

public class Role
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public List<User>? Users { get; set; }
}
