using System;
using System.Collections.Generic;

namespace AzureDrake.Bot.Models
{
    public partial class RankPerms
    {
        public int RankId { get; set; }
        public string Channel { get; set; }
        public string RankPerm { get; set; }

        public virtual Rank Ranks { get; set; }
    }
}
