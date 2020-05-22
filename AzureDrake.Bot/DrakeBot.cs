using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using AzureDrake.Bot.Commands;
using AzureDrake.Bot.Models;
using System.Threading;


namespace AzureDrake.Bot
{
    public class DrakeBot
    {
        BotInfo _info;
        TwitchAPI _service;
        TwitchClient _client;
        Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();
        Dictionary<string, BroadcasterInfo> _channelBroadcaster = new Dictionary<string, BroadcasterInfo>();
        List<string> _commandPermissions = new List<string>();
        
        public TwitchClient Client => _client;
        public TwitchAPI Service => _service;
        public Dictionary<string, ICommand> Commands => _commands;
        public Dictionary<string, BroadcasterInfo> ChannelBroadcaster => _channelBroadcaster;
        public List<string> CommandPermissions => _commandPermissions;
        public BotInfo Info => _info;

        public DrakeBot()
        {
            _info = BotInfo.Load();
            _service = new TwitchAPI();
            _service.Settings.ClientId = _info.ClientID;

            _client = new TwitchClient();
            _client.Initialize(new ConnectionCredentials(_info.BotUsername,
                _info.OAuth));

            _client.OnAnonGiftedSubscription += _client_OnAnonGiftedSubscription;
            _client.OnBeingHosted += _client_OnBeingHosted;
            _client.OnChannelStateChanged += _client_OnChannelStateChanged;
            _client.OnChatCleared += _client_OnChatCleared;
            _client.OnChatColorChanged += _client_OnChatColorChanged;
            _client.OnChatCommandReceived += _client_OnChatCommandReceived;
            _client.OnCommunitySubscription += _client_OnCommunitySubscription;
            _client.OnConnected += _client_OnConnected;
            _client.OnConnectionError += _client_OnConnectionError;
            _client.OnDisconnected += _client_OnDisconnected;
            _client.OnError += _client_OnError;
            _client.OnExistingUsersDetected += _client_OnExistingUsersDetected;
            _client.OnFailureToReceiveJoinConfirmation += _client_OnFailureToReceiveJoinConfirmation;
            _client.OnGiftedSubscription += _client_OnGiftedSubscription;
            _client.OnHostingStarted += _client_OnHostingStarted;
            _client.OnHostingStopped += _client_OnHostingStopped;
            _client.OnHostLeft += _client_OnHostLeft;
            _client.OnIncorrectLogin += _client_OnIncorrectLogin;
            _client.OnJoinedChannel += _client_OnJoinedChannel;
            _client.OnLeftChannel += _client_OnLeftChannel;
            //_client.OnLog += _client_OnLog;
            _client.OnMessageCleared += _client_OnMessageCleared;
            _client.OnMessageReceived += _client_OnMessageReceived;
            _client.OnMessageSent += _client_OnMessageSent;
            _client.OnMessageThrottled += _client_OnMessageThrottled;
            _client.OnModeratorJoined += _client_OnModeratorJoined;
            _client.OnModeratorLeft += _client_OnModeratorLeft;
            _client.OnModeratorsReceived += _client_OnModeratorsReceived;
            _client.OnNewSubscriber += _client_OnNewSubscriber;
            _client.OnNoPermissionError += _client_OnNoPermissionError;
            _client.OnNowHosting += _client_OnNowHosting;
            _client.OnRaidedChannelIsMatureAudience += _client_OnRaidedChannelIsMatureAudience;
            _client.OnRaidNotification += _client_OnRaidNotification;
            _client.OnReconnected += _client_OnReconnected;
            _client.OnReSubscriber += _client_OnReSubscriber;
            _client.OnRitualNewChatter += _client_OnRitualNewChatter;
            _client.OnSelfRaidError += _client_OnSelfRaidError;
            //_client.OnSendReceiveData += _client_OnSendReceiveData;
            //_client.OnUnaccountedFor += _client_OnUnaccountedFor;
            _client.OnUserBanned += _client_OnUserBanned;
            _client.OnUserJoined += _client_OnUserJoined;
            _client.OnUserLeft += _client_OnUserLeft;
            _client.OnUserStateChanged += _client_OnUserStateChanged;
            _client.OnUserTimedout += _client_OnUserTimedout;
            _client.OnVIPsReceived += _client_OnVIPsReceived;
            _client.OnWhisperCommandReceived += _client_OnWhisperCommandReceived;
            _client.OnWhisperReceived += _client_OnWhisperReceived;
            _client.OnWhisperSent += _client_OnWhisperSent;
            _client.OnWhisperThrottled += _client_OnWhisperThrottled;

            SetupCommands();

            

            _client.Connect();
            
        }

        private void SetupCommands()
        {
            _commands.Add("help", new HelpCommand());
            _commands.Add("clip", new ClipCommand());
        }

        public void AddCommand(ICommand command)
        {
            _commandPermissions.AddRange(command.CommandPermissions);
            _commands.Add(command.Command, command);
        }

        private void _client_OnWhisperThrottled(object sender, OnWhisperThrottledEventArgs e)
        {
            
        }

        private void _client_OnWhisperSent(object sender, OnWhisperSentArgs e)
        {
            
        }

        private void _client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            
        }

        private void _client_OnWhisperCommandReceived(object sender, OnWhisperCommandReceivedArgs e)
        {
            
        }

        private void _client_OnVIPsReceived(object sender, OnVIPsReceivedArgs e)
        {
            
        }

        private void _client_OnUserTimedout(object sender, OnUserTimedoutArgs e)
        {
            
        }

        private void _client_OnUserStateChanged(object sender, OnUserStateChangedArgs e)
        {
            
        }

        private void _client_OnUserLeft(object sender, OnUserLeftArgs e)
        {
            
        }

        private void _client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            if (e.Username.ToLower() == Info.BotUsername.ToLower())
                return;
            var getUser = Service.Helix.Users.GetUsersAsync(null, new List<string>(new[] { e.Username }), _channelBroadcaster[e.Channel].AccessToken);
            var getChatters = Service.Undocumented.GetChattersAsync(e.Channel);
            getChatters.Wait();

            TwitchLib.Api.Core.Enums.UserType userType = TwitchLib.Api.Core.Enums.UserType.Viewer;
            foreach (var chatter in getChatters.Result)
            {
                if (chatter.Username == e.Username)
                {
                    userType = chatter.UserType;
                    break;
                }
            }

            getUser.Wait();
            AddUser(getUser.Result.Users[0], userType, e.Channel);
        }

        private void _client_OnUserBanned(object sender, OnUserBannedArgs e)
        {
            
        }

        private void _client_OnUnaccountedFor(object sender, OnUnaccountedForArgs e)
        {
            
        }

        private void _client_OnSendReceiveData(object sender, OnSendReceiveDataArgs e)
        {
            
        }

        private void _client_OnSelfRaidError(object sender, EventArgs e)
        {
            
        }

        private void _client_OnRitualNewChatter(object sender, OnRitualNewChatterArgs e)
        {
            
        }

        private void _client_OnReSubscriber(object sender, OnReSubscriberArgs e)
        {
            
        }

        private void _client_OnReconnected(object sender, OnReconnectedEventArgs e)
        {
            
        }

        private void _client_OnRaidNotification(object sender, OnRaidNotificationArgs e)
        {
            
        }

        private void _client_OnRaidedChannelIsMatureAudience(object sender, EventArgs e)
        {
            
        }

        private void _client_OnNowHosting(object sender, OnNowHostingArgs e)
        {
            
        }

        private void _client_OnNoPermissionError(object sender, EventArgs e)
        {
            
        }

        private void _client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            
        }

        private void _client_OnModeratorsReceived(object sender, OnModeratorsReceivedArgs e)
        {
            
        }

        private void _client_OnModeratorLeft(object sender, OnModeratorLeftArgs e)
        {
            
        }

        private void _client_OnModeratorJoined(object sender, OnModeratorJoinedArgs e)
        {
            
        }

        private void _client_OnMessageThrottled(object sender, OnMessageThrottledEventArgs e)
        {
            
        }

        private void _client_OnMessageSent(object sender, OnMessageSentArgs e)
        {
            
        }

        private void _client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            
        }

        private void _client_OnMessageCleared(object sender, OnMessageClearedArgs e)
        {
            
        }

        private void _client_OnLog(object sender, OnLogArgs e)
        {
            
        }

        private void _client_OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            
        }

        private void _client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            var getChatters = Service.Undocumented.GetChattersAsync(e.Channel);
            getChatters.Wait();
            var chatters = getChatters.Result;

            if (chatters.Count == 0)
                return;
            List<string> usernames = new List<string>();

            foreach (var chatter in chatters)
                usernames.Add(chatter.Username);

            var getUsers = Service.Helix.Users.GetUsersAsync(null, usernames, _channelBroadcaster[e.Channel].AccessToken);
            getUsers.Wait();
            Parallel.For(0, getUsers.Result.Users.Length, (i, chatter) =>
            {
                AddUser(getUsers.Result.Users[i], chatters[i].UserType, e.Channel);
            });
        }

        private void AddUser(TwitchLib.Api.Helix.Models.Users.User userData, TwitchLib.Api.Core.Enums.UserType userType, string channel)
        {
            using (DragonBotContext db = new DragonBotContext())
            {
                BroadcasterInfo broadcaster = _channelBroadcaster[channel];

                List<string> userID = new List<string>(new[] { userData.Id });

                var Sub = Service.Helix.Subscriptions.GetUserSubscriptionsAsync(broadcaster.ID, userID, broadcaster.AccessToken);
                var ban = Service.Helix.Moderation.GetBannedUsersAsync(broadcaster.ID, userID, null, null, broadcaster.AccessToken);
                if (db.Users.Find(userData.Id, broadcaster.ID) != null)
                    return;

                User user = new User();

                if (userType == TwitchLib.Api.Core.Enums.UserType.Viewer)
                {
                    var Follows = Service.Helix.Users.GetUsersFollowsAsync(null, null, 20, userData.Id, broadcaster.ID);

                    bool finished = false;
                    while (!finished)
                    {
                        try
                        {
                            Follows.Wait();
                            finished = true;
                        }
                        catch
                        {
                            Thread.Sleep(30000);
                        }
                    }
                    if (Follows.Result.Follows.Length == 0)
                        return;

                    user.Moderator = false;
                    user.Vip = false;
                    user.Rank = db.Ranks.Find(Info.DefaultRank, broadcaster.ID);

                }
                else if (userType == TwitchLib.Api.Core.Enums.UserType.VIP)
                {
                    user.Moderator = false;
                    user.Vip = true;
                    user.Rank = db.Ranks.Find(Info.DefaultVIPRank, broadcaster.ID);
                }
                else
                {
                    user.Moderator = true;
                    user.Rank = db.Ranks.Find(Info.DefaultModRank, broadcaster.ID);
                    user.Vip = true;
                }

                user.Username = userData.DisplayName;
                user.Id = userData.Id;
                user.Points = 0;
                user.Channel = broadcaster.ID;
                user.Banned = false;
                user.BitsTotal = 0;
                user.SubStreak = 0;
                user.SubTotal = 0;

                try
                {
                    Sub.Wait();
                    user.Subbed = Sub.Result.Data.Length == 0;

                    if (user.Subbed)
                        user.SubTier = int.Parse(Sub.Result.Data[0].Tier);
                    else
                        user.SubTier = 0;
                }
                catch
                {
                    user.Subbed = false;
                    user.SubTier = 0;
                }

                ban.Wait();
                user.Banned = ban.Result.Data == null;

                ///
                /// Save
                ///
            }
        }

        private void _client_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            
        }

        private void _client_OnHostLeft(object sender, EventArgs e)
        {
            
        }

        private void _client_OnHostingStopped(object sender, OnHostingStoppedArgs e)
        {
            
        }

        private void _client_OnHostingStarted(object sender, OnHostingStartedArgs e)
        {
            
        }

        private void _client_OnGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
        {
            
        }

        private void _client_OnFailureToReceiveJoinConfirmation(object sender, OnFailureToReceiveJoinConfirmationArgs e)
        {
            
        }

        private void _client_OnExistingUsersDetected(object sender, OnExistingUsersDetectedArgs e)
        {
            
        }

        private void _client_OnError(object sender, OnErrorEventArgs e)
        {
            
        }

        private void _client_OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            
        }

        private void _client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            
        }

        private void _client_OnConnected(object sender, OnConnectedArgs e)
        {
            string[] channels = _info.ChannelName.Split(new char[] { ',' });
            string[] accessTokens = _info.AccessToken.Split(new char[] { ',' });

            for (int i = 0; i < channels.Length; i++)
            {
                string channel = channels[i].ToLower();
                var task = Service.Helix.Users.GetUsersAsync(null, new List<string>(new[] { channel }));
                task.Wait();
                string id = task.Result.Users[0].Id;
                _channelBroadcaster.Add(channel, new BroadcasterInfo(this, id, accessTokens[i]));

                using (DragonBotContext db = new DragonBotContext())
                {
                    if (db.Ranks.Find(_info.DefaultRank, id) == null)
                    {
                        db.Ranks.Add(new Rank()
                        {
                            Channel = id,
                            Id = _info.DefaultRank,
                            RankName = "Default",
                            RankPriority = _info.DefaultRank,
                        });
                        db.SaveChanges();
                    }
                    if (db.Ranks.Find(_info.DefaultVIPRank, id) == null)
                    {
                        db.Ranks.Add(new Rank()
                        {
                            Channel = id,
                            Id = _info.DefaultVIPRank,
                            RankName = "Default",
                            RankPriority = _info.DefaultVIPRank,
                        });
                        db.SaveChanges();
                    }

                    if (db.Ranks.Find(_info.DefaultModRank, id) == null)
                    {
                        db.Ranks.Add(new Rank()
                        {
                            Channel = id,
                            Id = _info.DefaultModRank,
                            RankName = "Default",
                            RankPriority = _info.DefaultModRank,
                        });
                        db.SaveChanges();
                    }
                }

                _client.JoinChannel(channel);
            }
        }

        private void _client_OnCommunitySubscription(object sender, OnCommunitySubscriptionArgs e)
        {
            
        }

        private void _client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (_commands.ContainsKey(e.Command.CommandText.ToLower()))
            {
                Task command = new Task(() => { ICommand com = _commands[e.Command.CommandText.ToLower()]; if (!com.Run(this, e)) Client.SendMessage(e.Command.ChatMessage.Channel, "@" + e.Command.ChatMessage.Username + " you don't have permission to access the " + com.Name + " command"); });
                command.Start();
            }
            else
            {
                _client.SendMessage(e.Command.ChatMessage.Channel, "@" + e.Command.ChatMessage.Username + " " + "The command " + e.Command.CommandText + " does not exist");
            }
            _client.SendMessage(e.Command.ChatMessage.Channel, "/delete " + e.Command.ChatMessage.Id);
        }

        private void _client_OnChatColorChanged(object sender, OnChatColorChangedArgs e)
        {
            
        }

        private void _client_OnChatCleared(object sender, OnChatClearedArgs e)
        {
            
        }

        private void _client_OnChannelStateChanged(object sender, OnChannelStateChangedArgs e)
        {
            
        }

        private void _client_OnBeingHosted(object sender, OnBeingHostedArgs e)
        {
            
        }

        private void _client_OnAnonGiftedSubscription(object sender, OnAnonGiftedSubscriptionArgs e)
        {
            
        }

        internal void Follows_OnNewFollowersDetected(object sender, TwitchLib.Api.Services.Events.FollowerService.OnNewFollowersDetectedArgs e)
        {
            Parallel.ForEach(e.NewFollowers, (follower) =>
            {
                string token = null;
                var inf = new KeyValuePair<string, BroadcasterInfo>();
                foreach (var info in ChannelBroadcaster)
                    if (info.Value.ID == e.Channel)
                    {
                        token = info.Value.AccessToken;
                        inf = info;
                    }
                var getUser = Service.Helix.Users.GetUsersAsync(null, new List<string>(new[] { follower.FromUserName }), token);
                var getChatters = Service.Undocumented.GetChattersAsync(inf.Key);
                getChatters.Wait();

                TwitchLib.Api.Core.Enums.UserType userType = TwitchLib.Api.Core.Enums.UserType.Viewer;
                foreach (var chatter in getChatters.Result)
                {
                    if (chatter.Username == follower.FromUserName)
                    {
                        userType = chatter.UserType;
                        break;
                    }
                }

                getUser.Wait();
                AddUser(getUser.Result.Users[0], userType, inf.Key);
            });
        }

        public bool HasPermission(string userid, string channel, string permission)
        {
            Rank rank = null;
            using (DragonBotContext db = new DragonBotContext())
            {
                User user = db.Users.Find(userid, channel);
                if (user != null)
                {
                    rank = user.Rank;
                }
                else
                {
                    rank = db.Ranks.Find(_info.DefaultRank, _channelBroadcaster[channel].ID);
                }
            }
            return true;
            return rank.RankPerms.Any((perm) => { return perm.RankPerm == permission; });
        }
    }
}
