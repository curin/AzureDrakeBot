using System;
using System.Collections.Generic;

namespace AzureDrake.Bot.Models
{
    public partial class Clip
    {
        public string Id { get; set; }
        public string Submitter { get; set; }
        public string Channel { get; set; }
        public DateTime SubmitTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string Title { get; set; }
        public string Game { get; set; }
    }
}
