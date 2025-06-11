using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;

namespace ChatApp.Features.ChatFeatures.Dtos;

public class ChatResponseDto
{
    public required Guid Id { get; set; }
}
