public class UserProfileResult
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public List<string> Roles { get; set; }
}