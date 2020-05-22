using System;
using System.Collections.Generic;

namespace AzureDrake.Bot.Models
{
    public partial class User
    {
        public User()
        {
            Bans = new HashSet<Ban>();
        }

        public string Id { get; set; }
        public string Channel { get; set; }
        public string Username { get; set; }
        public int RankId { get; set; }
        public int Points { get; set; }
        public int SubStreak { get; set; }
        public int SubTier { get; set; }
        public int SubTotal { get; set; }
        public long BitsTotal { get; set; }
        public bool Subbed { get; set; }
        public bool Moderator { get; set; }
        public bool Vip { get; set; }
        public bool Banned { get; set; }

        public virtual Rank Rank { get; set; }
        public virtual ICollection<Ban> Bans { get; set; }
    }
}
