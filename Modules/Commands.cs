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

namespace CaliComp.Modules
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    
    public class Commands : ModuleBase
    {
        private readonly CommandService _commands;

        public Commands(CommandService commands)
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


        [Command("honk")]
        [Summary("Gets all animated gifs of Chen honking in assets/images/honk and then randomly uploads one")]
        public async Task Honk()
        {
            //Gets all animated gifs of Chen honking in assets/images/honk and then randomly uploads one
            string[] fileArray = Directory.GetFiles(@"assets/images/honk", "*.gif");

            var fileName = fileArray[new Random().Next(fileArray.Count() - 1)];

            await Context.Channel.SendFileAsync(fileName);
        }

        [Command("8ball")]
        [Summary("Ask a question to the magic 8ball")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task AskEightBall([Remainder]string args = null)
        {
            // I like using StringBuilder to build out the reply
            var sb = new StringBuilder();
            // let's use an embed for this one!
            var embed = new EmbedBuilder();

            // now to create a list of possible replies
            var replies = new List<string>();

            // add our possible replies
            replies.Add("Yes");
            replies.Add("No");
            replies.Add("Maybe");
            replies.Add("Reply hazy, try again later...");

            // time to add some options to the embed (like color and title)
            embed.Title = "Welcome to the 8-ball!";
            
            // we can get lots of information from the Context that is passed into the commands
            // here I'm setting up the preface with the user's name and a comma
            sb.AppendLine($"{Context.User.Username},");
            sb.AppendLine();

            // let's make sure the supplied question isn't null 
            if (args == null)
            {
                // if no question is asked (args are null), reply with the below text
                sb.AppendLine("Sorry, can't answer a question you didn't ask!");
            }
            else 
            {
                // if we have a question, let's give an answer!
                // get a random number to index our list with (arrays start at zero so we subtract 1 from the count)
                var answer = replies[new Random().Next(replies.Count - 1)];
                
                // build out our reply with the handy StringBuilder
                sb.AppendLine($"You asked: **\"{args}\"**");
                sb.AppendLine();
                sb.AppendLine($"The 8-ball answers: **\"{answer}\"**");
            }

            // now we can assign the description of the embed to the contents of the StringBuilder we created
            embed.Description = sb.ToString();

            // this will reply with the embed
            await ReplyAsync(null, false, embed.Build());
        }
    }
}