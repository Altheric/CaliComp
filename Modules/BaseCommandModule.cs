using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CaliComp.Modules
{
    // Contains basic commands for the bot to use
    
    public class BaseCommandModule : ModuleBase
    {
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        public BaseCommandModule(CommandService commands, IConfiguration config)
        {
            _commands = commands;
            _config = config;
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

        [Command("die")]
        [Summary("Shuts down bot, bot owner only.")]
        public async Task Die()
        {
            //Checks if command user id is the same as the ownerid put into the config.
            if(Context.User.Id.ToString() == _config["OwnerID"])
            {
                await ReplyAsync("CaliComp is shutting down~");
                System.Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} CaliComp shutdown");
                System.Environment.Exit(1);
            }
            else
            {
                await ReplyAsync("Sorry, only the owner of this instance of CaliComp can stop me. If there's something wrong, please contact the owner of the bot.");
            }
        }
    }
}