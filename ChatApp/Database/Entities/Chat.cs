using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Database.Entities;
public class Chat
{
    public required Guid Id {get; set;}
    public List<Message> Messages {get; set;} = new List<Message>();
    public List<User> Participants {get; set;} = new List<User>();

    
}
