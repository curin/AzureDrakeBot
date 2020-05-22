using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TwitchLib.Client.Events;
using TwitchLib.Api.Helix.Models.Clips.GetClip;

namespace AzureDrake.Bot.Commands
{
    public class ClipCommand : ICommand
    {
        string[] fill = { "(Url/ID)" };
        string[] perms = { "clip" };
        public string Name => "Clip";

        public string Description => "Automatically clips the current broadcast and saves the clip for the broadcaster to review later (not currently saved). If a Clip url or id is passed that clip is saved rather than creating a new one.";

        public string Command => "clip";

        public string[] ArgumentDescription => fill;

        public string[] CommandPermissions => perms;

        public bool Run(DrakeBot bot, OnChatCommandReceivedArgs e)
        {
            BroadcasterInfo broadcaster = bot.ChannelBroadcaster[e.Command.ChatMessage.Channel];
            if (!bot.HasPermission(e.Command.ChatMessage.UserId, e.Command.ChatMessage.Channel, perms[0]))
                return false;
            Clip retrieved = null;
            if (e.Command.ArgumentsAsList.Count > 0)
            {
                string arg = e.Command.ArgumentsAsList[0];
                if (arg.StartsWith("https://clips.twitch.tv/"))
                {
                    arg = arg.Substring(24);
                }
                var split = arg.Split('/');

                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] != string.Empty)
                    {
                        arg = split[i];
                        break;
                    }
                }

                var getClip = bot.Service.Helix.Clips.GetClipAsync(arg);
                getClip.Wait();

                if (getClip.Result.Clips.Length == 0)
                {
                    bot.Client.SendMessage(e.Command.ChatMessage.Channel, "@" + e.Command.ChatMessage.Username + " your clip had an invalid id/url");
                    return true;
                }

                retrieved = getClip.Result.Clips[0];
                bot.Client.SendMessage(e.Command.ChatMessage.Channel, "@" + e.Command.ChatMessage.Username + " your clip was retrieved ( " + "https://clips.twitch.tv/" + arg + " )");
            }
            else
            {
                var task = bot.Service.Helix.Clips.CreateClipAsync(broadcaster.ID, broadcaster.AccessToken);
                task.Wait();
                var result = task.Result;
                var clip = result.CreatedClips[0];

                bot.Client.SendMessage(e.Command.ChatMessage.Channel, "@" + e.Command.ChatMessage.Username + " your clip was created and is available at https://clips.twitch.tv/" + result.CreatedClips[0].Id);

                var getClip = bot.Service.Helix.Clips.GetClipAsync(result.CreatedClips[0].Id);
                getClip.Wait();

                while (getClip.Result.Clips.Length == 0)
                {
                    Thread.Sleep(1000);
                    getClip = bot.Service.Helix.Clips.GetClipAsync(result.CreatedClips[0].Id);
                    getClip.Wait();
                }
                retrieved = getClip.Result.Clips[0];
            }

            var getGame = bot.Service.Helix.Games.GetGamesAsync(new List<string>(new[] { retrieved.GameId }));
            getGame.Wait();
            string game = getGame.Result.Games[0].Name;

            //TODO: SAVE to database

            return true;
        }
    }
}
