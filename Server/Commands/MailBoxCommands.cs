using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireCommandFramework;

namespace BloodyMailBox.Server.Commands
{
    [CommandGroup("mailbox")]
    internal class MailBoxCommands
    {

        [Command("send", usage: "\"<UserNick>\" \"<Message>\"", description: "Open a ticket to the server administrators", adminOnly: false)]
        public static void SendMail(ChatCommandContext ctx, string name, string message)
        {



        }

        [Command("open", usage: "\"<UserNick>\" \"<Message>\"", description: "Reply a Ticket", adminOnly: false)]
        public static void OpenMail(ChatCommandContext ctx, string name, string message)
        {

        }

        [Command("list", usage: "\"<UserNick>\" \"<Message>\"", description: "Reply a Ticket", adminOnly: false)]
        public static void ListMail(ChatCommandContext ctx, string name, string message)
        {

        }
    }
}
