using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.Entities;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ITenantContext _tenantContext;

    public LoginCommandHandler(
        UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ITenantContext tenantContext)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _tenantContext = tenantContext;
    }

    public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || user.TenantId != _tenantContext.TenantId)
        {
            return new AuthenticationResult
            {
                Success = false,
                Errors = new[] { "Invalid credentials" }
            };
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return new AuthenticationResult
            {
                Success = false,
                Errors = new[] { "Invalid credentials" }
            };
        }

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);
        
        // Add additional claims including tenant information
        var additionalClaims = new List<Claim>
        {
            new Claim("tenant_id", _tenantContext.TenantId.ToString()),
            new Claim("tenant_code", _tenantContext.TenantCode)
        };

        // Use the overloaded GenerateToken method with more parameters
        var token = _jwtTokenGenerator.GenerateToken(
            user.Id, 
            user.UserName!, 
            roles, 
            additionalClaims);

        return new AuthenticationResult
        {
            Success = true,
            Token = token
        };
    }
}