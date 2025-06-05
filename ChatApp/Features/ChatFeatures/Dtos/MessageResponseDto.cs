using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;

namespace ChatApp.Features.ChatFeatures.Dtos;
public class MessageResponseDto
{
    public required Guid Id {get;set;}
    public required DateTime SentOnUTC {get;set;}
    public required Guid SenderId {get; set;}
    public required string MessageContents {get; set;}
    public required Guid ChatId {get;set;}
}
