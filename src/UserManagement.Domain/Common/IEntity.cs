using System;

namespace UserManagement.Domain.Common;

public interface IEntity
{
    Guid Id { get; set; }
}