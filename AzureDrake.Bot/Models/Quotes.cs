using System;
using System.Collections.Generic;

namespace AzureDrake.Bot.Models
{
    public partial class Quotes
    {
        public int Id { get; set; }
        public DateTime SubmitTime { get; set; }
        public string Submitter { get; set; }
        public string AttributedTo { get; set; }
        public string Quote { get; set; }
        public DateTime StreamDate { get; set; }
        public string StreamTime { get; set; }
        public string StreamTitle { get; set; }
        public string StreamGame { get; set; }
        public string Channel { get; set; }
    }
}
