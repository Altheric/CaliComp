using Discord;
using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CaliComp.Modules
{
    // Contains the transcription command for the bot to use
    
    public class TranscribeModule : ModuleBase
    {
        private readonly IConfiguration _config;
        public TranscribeModule(IConfiguration config)
        {
            _config = config;
        }
        [Command("transcribe")]
        [Summary("transcribes current channel to a .txt file. ADMIN ONLY")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Transcribe()
        {
            //Collect all messages from the channel. Also yes, I'm using maxint. fuck the police.
            //Warning: this is a REALLY hacky way of doing this and WILL disconnect the bot several times for very large channels.
            //Just wait it out for a bit.
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(int.MaxValue).FlattenAsync();
            //Create a stringbuilder for what will be written to the text file
            var sb = new StringBuilder();
            //name of the Server, and format it for a better filename
            String serverName = Context.Guild.Name.ToLower().Replace(" ", "_").Replace("-", "_");
            //name of the channel, and format it for a better filename
            String channelName = Context.Channel.Name.ToLower().Replace(" ", "_").Replace("-", "_");
            //count of messages, just used for the novelty of knowing just how many messages there are
            int msgCount = messages.Count();
            //Path to write towards to
            String path = _config["LocalOutputPath"]+"/transcriptions/";
            //Check for existence of directory to write to
            if(!Directory.Exists(path))
            {
                //If not, create an output directory
                Directory.CreateDirectory(path);
            }
            //Name of the file
            String fileName = "transcribed_"+serverName+"@"+channelName+".txt";
            //Path to write to
            String writePath = path+fileName;
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
            sb.AppendLine($"Total amount of messages: {msgCount}");
            System.IO.File.WriteAllText(writePath, sb.ToString());
            FileInfo fileInfo = new FileInfo(writePath);
            //Filesize of the uploaded file
            long fileSize = fileInfo.Length;
            //Check if file is smaller than 8mb or 8388608 bytes
            if(fileSize < 8388608)
            {
                //if filesize is smaller than 8mb, DM the command user the file.
                await Context.Channel.SendMessageAsync($"Done transcribing the messages! Check your DM's!");
                await Context.User.SendMessageAsync($"I've transcribed {msgCount} messages, below is the transcribed .txt file.");
                await Context.User.SendFileAsync(writePath);
                File.Delete(writePath);
            }
            else
            {
                //Else tell the user that it aint gonna work.
                await Context.Channel.SendMessageAsync($"Done transcribing the messages! Check your DM's!");
                await Context.User.SendMessageAsync($"I've transcribed {msgCount} messages, however the file size was too big to upload to Discord. Please contact the bot owner to hand you the file.");
            }
            
        }
    }
}