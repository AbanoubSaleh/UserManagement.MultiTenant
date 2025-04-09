namespace UserManagement.Domain.Common;

public interface IDeletedEntity
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
}