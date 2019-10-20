using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace CaliComp.Modules
{
    // Contains local image grabbing commands for the bot to use
    
    public class GetLocalFileModule : ModuleBase
    {

        [Command("honk")]
        [Summary("Get an animated gif of Chen honking")]
        public async Task Honk()
        {
            //Gets all animated gifs of Chen honking in assets/images/honk and then randomly uploads one
            string[] fileArray = Directory.GetFiles(@"assets/images/honk", "*.gif");

            var fileName = fileArray[new Random().Next(fileArray.Count() - 1)];

            await Context.Channel.SendFileAsync(fileName);
        }
    }
}