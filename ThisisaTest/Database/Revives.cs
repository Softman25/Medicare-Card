using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using Discord;
using System.Threading.Tasks;
using Discord.WebSocket;
using ThisisaTest.Database;
using System.Linq;

namespace ThisisaTest.Database
{
    public class Revive : ModuleBase<SocketCommandContext>
    {
        [Command("setid")]
        public async Task SetIdAsync(int Profile = 0, IUser User = null)
        {
            User = Context.User;
            var ChannelName = Context.Channel.Name;
            
            if (Profile == 0)
            {
                await Context.User.SendMessageAsync("To set your ID, you must also include your Torn Profile Number. For example, '!setid 10101'. Please try again, using that syntax.");
                await Context.Message.DeleteAsync();
                return;
            }

            //Check if Profile is a string? How to say if (Profile = string) without it flipping a lid? It's clearly an integer so it makes sense, but some people are just going to put WORDS IN ANYWAY????

            if (ChannelName != "setid")
            {
                await Context.User.SendMessageAsync("Please use this command within the #setid channel, and only there!");
                await Context.Message.DeleteAsync();
            }
            else
            {
                await Data.SaveProfile(Profile, User.Id, 1);
                await Context.User.SendMessageAsync($"Your profile ID has been set as {Profile}. Please double check that this is correct. Enjoy your stay at Medicare! If you have any questions, please feel free to ask away");
                await Context.Message.DeleteAsync();
            }
        }

        [Command("giverole")]
        public async Task GiveRoleAsync()
        {

            var user = Context.User;
            var ChannelName = Context.Channel.Name;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Reviver");
            if (ChannelName != "rolemanagement")
            {
                await Context.User.SendMessageAsync("Please use this command within the #rolemanagement channel, and only there!");
                await Context.Message.DeleteAsync();
            }
            else
            {
                await (user as IGuildUser).AddRoleAsync(role);
                await Context.Message.DeleteAsync();
            }
        }

        [Command("takerole")]
        public async Task TakeRoleAsync()
        {

            var user = Context.User;
            var ChannelName = Context.Channel.Name;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Reviver");
            if (ChannelName != "rolemanagement")
            {
                await Context.User.SendMessageAsync("Please use this command within the #rolemanagement channel, and only there!");
                await Context.Message.DeleteAsync();
            }
            else
            {
                await (user as IGuildUser).RemoveRoleAsync(role);
                await Context.Message.DeleteAsync();
            }
        }

        [Command("ContractRevive"), Alias("cr")]
        public async Task ReviveAsync(IUser User = null)
        {
            User = Context.User;
            var ChannelName = Context.Channel.Name;

            if (ChannelName != "contract_revive")
            {
                await Context.User.SendMessageAsync("Please use this command within the #contract_revive channel, and only there!");
                await Context.Message.DeleteAsync();
                return;
            }

            int ProfileCheck = Data.SeeProfile(User.Id);
            if (ProfileCheck < 1)
            {
                await Context.User.SendMessageAsync("You need to set your profile ID. Go to the #setid channel and use '!setid [your Torn profile ID]'. Once you have done that, come back and request away :+1:.");
                await Context.Message.DeleteAsync();
                return;
            }
            await ReplyAsync($"<@&537465851109048321> - https://www.torn.com/profiles.php?XID={Data.SeeProfile(User.Id)} needs a revive! This revive has been recorded and will be invoiced accordingly.");
            await Data.SaveRecord(User.Id, 1, 1);
            await Context.Message.DeleteAsync();
        }

        [Command("PublicRevive"), Alias("r", "revive", "reviveme")]
        public async Task PublicReviveAsync(IUser User = null)
        {
            User = Context.User;

            var ChannelName = Context.Channel.Name;

            if (ChannelName != "public_revive")
            {
                await Context.User.SendMessageAsync("Please use this command within the #public_revive channel, and only there!");
                await Context.Message.DeleteAsync();
                return;
            }

            int ProfileCheck = Data.SeeProfile(User.Id);
            if (ProfileCheck < 1)
            {
                await Context.User.SendMessageAsync("You need to set your profile ID. Go to the #setid channel and use !setid [your Torn profile ID]. Once you have done that, come back and request away :+1:.");
                await Context.Message.DeleteAsync();
                return;
            }
            await ReplyAsync($"<@&537465851109048321> - https://www.torn.com/profiles.php?XID={Data.SeeProfile(User.Id)} needs a revive! Please remember to pay your reviver promptly!");
            await Context.Message.DeleteAsync();
        }

        [Command("done")]
        public async Task DoneAsync(string ID = null, IUser User = null)
        {
            if (ID == null)
            {
                await Context.User.SendMessageAsync("You need to include an ID in your revive completion report. Please try again.");
                await Context.Message.DeleteAsync();
                return;
            }
            await ReplyAsync($"The revive for {ID} has been completed!");
            await Context.Message.DeleteAsync();
            
            //await Task.Delay(10000);
            //IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(20).FlattenAsync();
            //await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);


            //SocketGuild guild = channel.Guild;
            //var thischannel = Context.Channel;

        }
    }

    public class InvoiceManip : ModuleBase<SocketCommandContext>
    {
        [Command("getinvoice"), RequireUserPermission(GuildPermission.AddReactions)]
        public async Task GetInvoiceAsync()
        {
            SocketGuildUser User1 = Context.User as SocketGuildUser;
            if (!User1.GuildPermissions.AddReactions)
            {
                await Context.User.SendMessageAsync("You do not have the required permission to use this command. If it is necessary, contact someone with the Admin role here");
                await Context.Message.DeleteAsync();
                return;
            }

            await Context.User.SendMessageAsync($"Here is the current number of contracted revives requested: {Database.Data.ReviveRecord(1)}");
            await Context.Message.DeleteAsync();
        }

        [Command("resetinvoice"), RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task ResetInvoiceAsync()
        {
            SocketGuildUser User1 = Context.User as SocketGuildUser;
            if (!User1.GuildPermissions.ManageChannels)
            {
                await Context.User.SendMessageAsync("You do not have the required permission to use this command. If it is necessary, contact someone with the Admin role here");
                await Context.Message.DeleteAsync();
                return;
            }
        
            using (var DbContext = new SqliteDbContext())
            {
                IQueryable<Invoice> ResetInvoice = DbContext.Invoice.Where(x => x.Amount > 0);
                foreach (Invoice Amount in ResetInvoice)
                {
                    Amount.Amount = 0;
                };
                await DbContext.SaveChangesAsync();                         
                await Context.User.SendMessageAsync("This message is only sent after the database changes have been saved, and hence confirms success of your reset!");
                await Context.Message.DeleteAsync();
            }  
        }

        
        
        //Helpful for purging all commands with ID X inside???

        //[Command("purge")]
        //[Summary("Deletes the specified amount of messages.")]
        //[RequireUserPermission(GuildPermission.Administrator)]
        //[RequireBotPermission(ChannelPermission.ManageMessages)]
        //public async Task PurgeChat(int amount)
            //IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(20).FlattenAsync();
            //await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
            //const int delay = 3000;
            //IUserMessage m = await ReplyAsync($"I have deleted {amount} messages for ya. :)");
            //await Task.Delay(delay);
            //await m.DeleteAsync();
    }
}
