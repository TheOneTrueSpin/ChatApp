using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Shared.Utils;

public interface IServiceModule
{
    public void Register(IServiceCollection services);
}
