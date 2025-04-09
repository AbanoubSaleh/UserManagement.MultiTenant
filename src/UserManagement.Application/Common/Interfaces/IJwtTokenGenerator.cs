using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace UserManagement.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string userName, IEnumerable<string>? roles = null, IEnumerable<Claim>? additionalClaims = null);
        bool ValidateToken(string token);
    }
}