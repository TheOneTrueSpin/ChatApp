using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Database.Entities;
public class Message
{
    public required Guid Id {get;set;}
    public required DateTime SentOnUTC {get;set;}
    public User Sender {get; set;} = null!;
    public required Guid SenderId {get; set;}
    public required string MessageContents {get; set;}
    public required Guid ChatId {get;set;}
}