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
        private readonly DiscordSocketClient _client;

        public Commands(CommandService commands, DiscordSocketClient client)
        {
            _commands = commands;
            _client = client;
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

        [Command("transcribe")]
        [Summary("transcribes current channel to a .txt file")]
        public async Task Transcribe()
        {
            //Collect all messages from the channel. Also yes, I'm using maxint. fuck the police.
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(int.MaxValue).FlattenAsync();
            //Create a stringbuilder for what will be written to the text file
            var sb = new StringBuilder();

            String path = @"output/test.txt";
            //Goes through every message in the IEnumerable. Reverse the order so it oldest is first.
            foreach(Discord.IMessage message in messages.Reverse())
            {
                //Grab various components from individual messages
                //time is the Timestamp of the message
                String time = message.Timestamp.ToString();
                //user is the author of the message, which is the discord username and identifier
                String user = message.Author.ToString();
                //msg is purely what you write down and type
                String msg = message.Content;
                //embeds is an IEmbed object, which contains all data regarding embeds
                IEnumerable<Discord.IEmbed> embeds = message.Embeds;
                //attachments is an IAttachment object, which contains all data regarding attachments
                IEnumerable<Discord.IAttachment> attachments = message.Attachments;
                sb.AppendLine(user);
                sb.AppendLine(time);
                if(msg != ""){
                    sb.AppendLine(msg);
                }
                if(embeds.Count() != 0)
                {
                    sb.AppendLine($"Embedded Content:");
                    foreach(Discord.IEmbed emb in embeds)
                    {
                        if(emb.Title != ""){
                            sb.AppendLine(emb.Title);
                        }
                        if(emb.Description != ""){
                        sb.AppendLine(emb.Description);
                        }
                        if(emb.Fields.Count() != 0){
                            foreach(Discord.EmbedField field in emb.Fields)
                            {
                                sb.AppendLine(field.Name);
                                sb.AppendLine(field.Value);
                            }
                        }
                    }
                }
                if(attachments.Count() != 0)
                {
                    sb.AppendLine($"Attachements:");
                    foreach(Discord.IAttachment attachment in attachments)
                    {
                        //ProxyUrl is the url discord assigns to anything uploaded
                        sb.AppendLine(attachment.ProxyUrl);
                    }
                }
                sb.AppendLine();
            }
            System.IO.File.WriteAllText(path, sb.ToString());

            await ReplyAsync("Done transcribing!");
        }

        [Command("honk")]
        [Summary("Get an animated gif of Chen honking")]
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