using Discord;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CaliComp.Modules
{
    // Contains Random Number Generation commands for the bot to use
    
    public class RNGModule : ModuleBase
    {

        [Command("8ball")]
        [Summary("Ask a question to the magic 8ball")]
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
                var answer = replies[new Random().Next(replies.Count)];
                
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