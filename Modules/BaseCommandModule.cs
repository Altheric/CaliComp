using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using CaliComp.Services;
using Discord.Audio;

namespace CaliComp.Modules
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    
    public class BaseCommandModule : ModuleBase
    {
        private readonly CommandService _commands;
        public BaseCommandModule(CommandService commands, DiscordSocketClient client)
        {
            _commands = commands;
        }
        [Command("help")]
        [Summary("Shows all known commands")]
        public async Task Help()
        {
            List<CommandInfo> commands = _commands.Commands.ToList();
            EmbedBuilder embedBuilder = new EmbedBuilder();

            foreach (CommandInfo command in commands)
            {
                // Get the command Summary attribute information
                string embedFieldText = command.Summary ?? "No description available\n";

                embedBuilder.AddField(command.Name, embedFieldText);
            }

            await ReplyAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
        }
    }
}