using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Database.ModelBuilders;

public static class ModelBuilderRoles
{
    public static ModelBuilder AddRoles(this ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Role>().HasData(
            new Role() { Id = 1, Name = "User" },
            new Role() { Id = 2, Name = "Admin" }
        );

        return modelBuilder;
    }
}
