using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CaliComp.Modules
{
    // Contains basic commands for the bot to use
    
    public class BaseCommandModule : ModuleBase
    {
        private readonly CommandService _commands;
        public BaseCommandModule(CommandService commands)
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