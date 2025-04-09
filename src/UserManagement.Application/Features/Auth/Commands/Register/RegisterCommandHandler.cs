using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITenantContext _tenantContext;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            ITenantContext tenantContext,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _tenantContext = tenantContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                TenantId = _tenantContext.TenantId
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
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

            // Use the overloaded GenerateToken method with the required parameters
            var token = _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.UserName,
                roles,
                additionalClaims);

            return new AuthenticationResult
            {
                Success = true,
                Token = token
            };
        }
    }
}