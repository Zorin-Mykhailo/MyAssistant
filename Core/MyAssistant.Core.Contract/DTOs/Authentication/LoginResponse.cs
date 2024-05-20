using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAssistant.Core.Contract.DTOs.Authentication;

public record LoginResponse
{
    public string UserId { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public TokenApiResponse TokenApiResponse { get; set; } = default!;
}