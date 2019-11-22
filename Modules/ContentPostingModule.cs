using Discord;
using Discord.Commands;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
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
            string links = File.ReadAllText(_config["LocalTextFilePath"]+"/imagelinks/honk.txt");
            string[] linkArray = links.Split(",");

            var link = linkArray[new Random().Next(linkArray.Count())];

            await Context.Channel.SendMessageAsync(link);
        }
        [Command("gotobed")]
        [Summary("Get an animated gif related to going to bed")]
        public async Task GoToBed()
        {
            string links = File.ReadAllText(_config["LocalTextFilePath"]+"/imagelinks/gotobed.txt");
            string[] linkArray = links.Replace(" ", "").Split(",");

            var link = linkArray[new Random().Next(linkArray.Count())];

            await Context.Channel.SendMessageAsync(link);
        }
        [Command("drinkfact")]
        [Summary("Get a random fact about beer, whiskey, sake, wine or a cocktail. Either use any of these types of drinks as a parameter or let the bot randomly decide by just inputting the command (Yes, the facts are nabbed from Catherine)")]
        public async Task DrinkFact(string drinkType = "")
        {
            //make input lowercase for easier checking
            drinkType = drinkType.ToLower();
            //Bool to make sure the task doesn't go on when it shouldn't
            Boolean drinkIsValid = true;
            //If no drinktype was given, determine which type of drink the fact will be about.
            if(drinkType == ""){
                string[] drinkArray = new string[5] {"cocktail", "beer", "sake", "whiskey", "wine"};
                drinkType += drinkArray[new Random().Next(drinkArray.Count())];
            }
            //Check if a valid drinktype was given
            else if(drinkType != "cocktail" && drinkType != "beer" && drinkType != "sake" && drinkType != "whiskey" && drinkType != "wine")
            {
                drinkIsValid = false;
                await ReplyAsync("Sorry, that's not a drink I know anything about~!");
                System.Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Command input was invalid.");
            }
            //If drink is valid, time to get the fact and build the embed
            if(drinkIsValid){
                //Make an EmbedBuilder.
                EmbedBuilder embedBuilder = new EmbedBuilder();
                //drinkImagePath is the path for a list of links for the image of the drink specific, these will later be in an array.
                //index 0 = cocktail, 1 = beer, 2 = sake, 3 = whiskey, 4 = wine
                string drinkImagePath = _config["LocalTextFilePath"]+"/imagelinks/drinklinks.txt";
                int drinkImageIndex = 0;
                //Determine index & color to use
                switch(drinkType)
                {
                    case "cocktail":
                    drinkImageIndex += 0;
                    embedBuilder.Color = new Discord.Color(255, 100, 100);
                    break;
                    case "beer":
                    drinkImageIndex += 1;
                    embedBuilder.Color = new Discord.Color(255, 155, 55);
                    break;
                    case "sake":
                    drinkImageIndex += 2;
                    embedBuilder.Color = new Discord.Color(200, 200, 200);
                    break;
                    case "whiskey":
                    drinkImageIndex += 3;
                    embedBuilder.Color = new Discord.Color(255, 55, 55);
                    break;
                    case "wine":
                    drinkImageIndex += 4;
                    embedBuilder.Color = new Discord.Color(255, 155, 155);
                    break;
                }
                //Get the image link to use, start with reading the imagelink file
                string drinkImageLinks = File.ReadAllText(drinkImagePath);
                //Split up the link into an array
                string[] drinkImageLinkArray = drinkImageLinks.Replace(" ", "").Split(",");
                //drinkFactPath is the path for a text file containing all the drink facts
                string drinkFactPath = _config["LocalTextFilePath"]+"/drinkfacts/"+drinkType+".txt";
                embedBuilder.Title =  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(drinkType);
                embedBuilder.ThumbnailUrl = drinkImageLinkArray[drinkImageIndex];
                //Get the image link to use, start with reading the imagelink file
                string drinkText = File.ReadAllText(drinkFactPath);
                //Split up the link into an array
                string[] drinkTextArray = drinkText.Split(";");
                //Get a random index number to show as fact
                int drinkTextIndex = new Random().Next(drinkTextArray.Count());
                StringBuilder sb = new StringBuilder();
                sb.Append(drinkTextArray[drinkTextIndex]);
                embedBuilder.AddField("Fact",sb.ToString());
                await ReplyAsync("", false, embedBuilder.Build());
            }
        }
    }
}