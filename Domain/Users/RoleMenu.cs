namespace Domain.Users
{
    public class RoleMenu : IDomain
    {
        public long Id { get; set; }
        public long MenuId { get; set; }
        public long RoleId { get; set; }
    }
}
