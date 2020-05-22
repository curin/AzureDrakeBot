using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Api.Services;
using System.Threading;

namespace AzureDrake.Bot
{
    public class BroadcasterInfo
    {
        public BroadcasterInfo(DrakeBot bot, string id, string accessToken)
        {
            ID = id;
            AccessToken = accessToken;
            follows = new FollowerService(bot.Service);
            follows.SetChannelsById(new List<string>(new []{ id}));

            follows.OnNewFollowersDetected += bot.Follows_OnNewFollowersDetected;
            follows.Start();
        }
        public string ID;
        public string AccessToken;
        public FollowerService follows;

    }
}
