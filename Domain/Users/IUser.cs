using System;

namespace Domain.Users
{
    public interface IUser : IAggregateRoot
    {
        string Name { get; set; }
        string Password { get; set; }
        string Account { get; set; }
        string Image { get; set; }

        bool IsEnable { get; set; }
        DateTime AddTime { get; set; }
    }
}
