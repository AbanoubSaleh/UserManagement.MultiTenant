using MediatR;
using UserManagement.Application.Common.Models;

public class LoginCommand : IRequest<AuthenticationResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string TenantCode { get; set; }
}