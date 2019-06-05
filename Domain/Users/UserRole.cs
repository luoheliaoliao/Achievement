namespace Domain.Users
{
    public class UserRole : IDomain
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}
