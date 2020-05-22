using System;
using System.Collections.Generic;

namespace AzureDrake.Bot.Models
{
    public partial class Rank
    {
        public Rank()
        {
            RankPerms = new HashSet<RankPerms>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Channel { get; set; }
        public string RankName { get; set; }
        public int RankPriority { get; set; }

        public virtual ICollection<RankPerms> RankPerms { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
