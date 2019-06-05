using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class RoleButton : IDomain
    {
        public long Id { get; set; }
        public long ButtonId { get; set; }
        public long RoleId { get; set; }
    }
}
