using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThisisaTest.Database;

namespace ThisisaTest.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.AddField("Field1", "test")
                .AddField("Field2", "test", true)
                .AddField("Field3", "test", true);

            await ReplyAsync("", false, builder.Build());
        }
    }
    public class Ping2 : ModuleBase<SocketCommandContext>
    {
        [Command("ping2"), RequireUserPermission(GuildPermission.AddReactions)]
        public async Task PingAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("This is a Test")
                .WithDescription("This is also a test, but longer and ***with formatting***")
                .WithColor(Color.Red);

            await ReplyAsync("", false, builder.Build());
        }
    }

    public class Ping3 : ModuleBase<SocketCommandContext>
    {
        [Command("ping3")]
        public async Task PingAsync(string ID)
        {
            await ReplyAsync($"<@&535270290695913484> - https://www.torn.com/profiles.php?XID={ID} needs a revive!");

            await Context.Message.DeleteAsync();
        }
    }

    public class RoleGive : ModuleBase<SocketCommandContext>
    {
        [Command("DUMMYTEST")]
        public async Task PingAsync()
        {
            var user = Context.User;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Reviver");        
            await(user as IGuildUser).AddRoleAsync(role);
            await Context.Message.DeleteAsync();
            
        }
    }

    public class RoleTake : ModuleBase<SocketCommandContext>
    {
        [Command("MORE TEST")]
        public async Task PingAsync()
        {
            var user = Context.User;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Reviver");
            await (user as IGuildUser).RemoveRoleAsync(role);
            await Context.Message.DeleteAsync();


        }
    }
}
