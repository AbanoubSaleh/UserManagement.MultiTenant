using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.Entities;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileResult>
{
    private readonly UserManager<User> _userManager;
    private readonly ITenantContext _tenantContext;

    public GetUserProfileQueryHandler(
        UserManager<User> userManager,
        ITenantContext tenantContext)
    {
        _userManager = userManager;
        _tenantContext = tenantContext;
    }

    public async Task<UserProfileResult> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        
        if (user == null || user.TenantId != _tenantContext.TenantId)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new UserProfileResult
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Roles = roles.ToList()
        };
    }
}