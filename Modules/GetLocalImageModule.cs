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
    
    public class GetLocalImageModule : ModuleBase
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