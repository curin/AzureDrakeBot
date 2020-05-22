using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Events;

namespace AzureDrake.Bot
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        string Command { get; }
        string[] ArgumentDescription { get; }
        string[] CommandPermissions { get; }
        bool Run(DrakeBot bot, OnChatCommandReceivedArgs e);
    }
}
