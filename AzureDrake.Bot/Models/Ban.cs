using System;
using System.Collections.Generic;

namespace AzureDrake.Bot.Models
{
    public partial class Ban
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Channel { get; set; }
        public DateTime BanEnd { get; set; }
        public string BanReason { get; set; }

        public virtual User Users { get; set; }
    }
}
