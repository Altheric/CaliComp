using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CaliComp.Modules
{
    // Contains local image grabbing commands for the bot to use

    public class ContentPostingModule : ModuleBase
    {
        private readonly IConfiguration _config;
        public ContentPostingModule(IConfiguration config)
        {
            _config = config;
        }
        [Command("honk")]
        [Summary("Get an animated gif of Chen honking")]
        public async Task Honk()
        {
            //Gets all links to animated gifs of Chen honking in assets/imagelinks/honk and then randomly posts one
            //Note that the .txt file that contains the links has to have the links seperated by commas
            string links = File.ReadAllText(_config["LocalImageLinkPath"]+"/honk.txt");
            string[] linkArray = links.Split(",");

            var link = linkArray[new Random().Next(linkArray.Count())];

            await Context.Channel.SendMessageAsync(link);
        }
        [Command("gotobed")]
        [Summary("Get an animated gif related to going to bed")]
        public async Task GoToBed()
        {
            string links = File.ReadAllText(_config["LocalImageLinkPath"]+"/gotobed.txt");
            string[] linkArray = links.Replace(" ", "").Split(",");

            var link = linkArray[new Random().Next(linkArray.Count())];

            await Context.Channel.SendMessageAsync(link);
        }
    }
}