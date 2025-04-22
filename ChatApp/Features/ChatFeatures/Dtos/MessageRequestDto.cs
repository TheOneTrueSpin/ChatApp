using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Features.ChatFeatures.Dtos;
public class MessageRequestDto
{
    public required Guid ChatId {get;set;}
    public required string Message {get;set;}
    public required Guid SenderId {get;set;}
    
}