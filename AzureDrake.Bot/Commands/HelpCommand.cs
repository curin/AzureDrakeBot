using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Events;

namespace AzureDrake.Bot.Commands
{
    public class HelpCommand : ICommand
    {
        string[] _argDesc = { "(command_name)"};
        string[] perms = { "help" };
        public string Name => "Help";

        public string Description => "Describes the function of any command. Arguments in parentheses are optional.";

        public string Command => "help";

        public string[] ArgumentDescription => _argDesc;

        public string[] CommandPermissions => perms;

        public bool Run(DrakeBot bot, OnChatCommandReceivedArgs e)
        {
            if (!bot.HasPermission(e.Command.ChatMessage.UserId, e.Command.ChatMessage.Channel, perms[0]))
                return false;
            string command = "";
            if (e.Command.ArgumentsAsList.Count == 0)
            {
                command = "help";
            }
            else
            {
                command = e.Command.ArgumentsAsList[0];
            }

            Help(bot, e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username, command);
            return true;
        }

        public static void Help(DrakeBot bot, string channel, string user, string command)
        {
            if (!bot.Commands.ContainsKey(command))
            {
                bot.Client.SendMessage(channel, "@" + user + " " + "The command " + command + " does not exist");
            }
            else
            {
                ICommand com = bot.Commands[command];
                string args = "";
                foreach (string desc in com.ArgumentDescription)
                    args += "{" + desc + "} ";
                args = args.Trim(' ');
                bot.Client.SendMessage(channel, "@" + user + " " + com.Name + " (!" + com.Command + " " + args + ") - " + com.Description);
            }
        }
    }
}
