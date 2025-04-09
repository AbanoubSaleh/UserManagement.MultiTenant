using MediatR;

public class GetUserProfileQuery : IRequest<UserProfileResult>
{
    public Guid UserId { get; set; }
}