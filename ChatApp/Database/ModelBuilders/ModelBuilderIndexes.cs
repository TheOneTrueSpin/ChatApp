using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Database.ModelBuilders;

public static class ModelBuilderIndexes
{
    public static ModelBuilder AddIndexes(this ModelBuilder modelBuilder)
    {
        return modelBuilder;
    }
}
