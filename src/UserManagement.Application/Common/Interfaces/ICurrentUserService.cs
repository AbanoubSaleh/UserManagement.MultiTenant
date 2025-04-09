using System;

namespace UserManagement.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string UserName { get; }
        bool IsAuthenticated { get; }
    }
}