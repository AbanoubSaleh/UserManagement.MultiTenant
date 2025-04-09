using MediatR;
using UserManagement.Application.Common.Models;

namespace UserManagement.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<AuthenticationResult>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string TenantCode { get; set; } = null!;
    }
}