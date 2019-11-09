using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CaliComp.Modules
{
    // Contains local image grabbing commands for the bot to use

    public class GetLocalFileModule : ModuleBase
    {
        private readonly IConfiguration _config;
        public GetLocalFileModule(IConfiguration config)
        {
            _config = config;
        }
        [Command("honk")]
        [Summary("Get an animated gif of Chen honking")]
        public async Task Honk()
        {
            //Gets all animated gifs of Chen honking in assets/images/honk and then randomly uploads one
            string[] fileArray = Directory.GetFiles(_config["LocalImagePath"]+"/honk", "*.gif");

            var fileName = fileArray[new Random().Next(fileArray.Count() - 1)];

            await Context.Channel.SendFileAsync(fileName);
        }
        [Command("gotobed")]
        [Summary("Get an animated gif related to going to bed")]
        public async Task GoToBed()
        {
            string[] fileArray = Directory.GetFiles(_config["LocalImagePath"]+"/gotobed", "*.gif");

            var fileName = fileArray[new Random().Next(fileArray.Count() - 1)];

            await Context.Channel.SendFileAsync(fileName);
        }
    }
}