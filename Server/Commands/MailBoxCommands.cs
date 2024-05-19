using Bloodstone.API;
using Bloody.Core;
using BloodyMailBox.Exceptions;
using BloodyMailBox.Utils;
using ProjectM;
using System.Linq;
using VampireCommandFramework;
using static RootMotion.FinalIK.InteractionObject;

namespace BloodyMailBox.Server.Commands
{
    [CommandGroup("mailbox")]
    internal class MailBoxCommands
    {

        [Command("send", usage: "<UserNick> \"<Message>\"", description: "Send a message to the mailbox of a server user", adminOnly: false)]
        public static void SendMail(ChatCommandContext ctx, string name, string message)
        {
            var user = Core.Users.All.FirstOrDefault(user => user.CharacterName.ToString().ToLower() == name.ToString().ToLower()) ?? throw ctx.Error($"User {FontColorChat.White($"{name}")} does not exist on this server.");

            if (ctx.User.CharacterName.ToString().ToLower() == name.ToString().ToLower() && !Plugin.OwnMessages.Value) throw ctx.Error("You are not allowed to send messages to yourself.");

            try {
                MailBox.MessageToMailBox(name.ToString().ToLower(), message, ctx.User.CharacterName.ToString());
                ctx.Reply(FontColorChat.Yellow($"Message sent to user {FontColorChat.White($"{name}")} successfully."));
                if (user.IsConnected) 
                    ServerChatUtils.SendSystemMessageToClient(VWorld.Server.EntityManager, (ProjectM.Network.User)user.Internals.User, $"You have a new message from {FontColorChat.White($"{ctx.User.CharacterName}")} in your mailbox.");
                return;
            }
            catch(MailBoxException e) {
                throw ctx.Error(e.Message);
            }

        }

        [Command("read", usage: "<idMessage>", description: "Read a certain message from your mailbox", adminOnly: false)]
        public static void ReadMessageFromMailBox(ChatCommandContext ctx, int id)
        {
            try
            {
                var message = MailBox.SelectMessageFromMailBox(ctx.User.CharacterName.ToString().ToLower(), id);
                ctx.Reply(FontColorChat.Yellow($"------------ Message {FontColorChat.White($"{id}")} ------------"));
                ctx.Reply(FontColorChat.Yellow($"Message Author: {FontColorChat.White($"{message.Author}")}"));
                ctx.Reply(FontColorChat.Yellow($"Message Body:"));
                ctx.Reply(FontColorChat.Yellow($"{FontColorChat.White($"{message.Body}")}"));
                ctx.Reply(FontColorChat.Yellow($"------------ End Message {FontColorChat.White($"{id}")} ------------"));
                if (!message.Open) MailBox.SetOpenedMessageFromMailBox(ctx.User.CharacterName.ToString().ToLower(), id);
            }
            catch (MailBoxException e)
            {
                throw ctx.Error(e.Message);
            }

        }

        [Command("list", usage: "", description: "List of all your messages in the mailbox", adminOnly: false)]
        public static void ListMail(ChatCommandContext ctx)
        {
            try
            {
                var read = "";
                var messages = MailBox.ReadMailBox(ctx.User.CharacterName.ToString().ToLower());
                if (Plugin.MaxMessages.Value > 0)
                {
                    ctx.Reply(FontColorChat.Yellow($"------------ MailBox [{FontColorChat.White($"{messages.Count()}")}/{FontColorChat.White($"{Plugin.MaxMessages.Value}")}] ------------"));
                } 
                else
                {
                    ctx.Reply(FontColorChat.Yellow($"------------ MailBox ------------"));
                }

                foreach ( var message in messages )
                {
                    read = message.Open ? "Read" : "Unread";
                    ctx.Reply(FontColorChat.Yellow($"[{FontColorChat.White($"{message.Id}")}][{FontColorChat.White($"{read}")}] Message Author: {FontColorChat.White($"{message.Author}")}"));
                }
                ctx.Reply(FontColorChat.Yellow($"------------ End MailBox ------------"));
            }
            catch (MailBoxException e)
            {
                throw ctx.Error(e.Message);
            }
        }

        [Command("delete", usage: "<idMessage>", description: "Delete a certain message from your mailbox", adminOnly: false)]
        public static void DeleteMail(ChatCommandContext ctx, int id)
        {
            try
            {
                var message = MailBox.DeleteMessageFromMailBox(ctx.User.CharacterName.ToString().ToLower(), id);
                ctx.Reply(FontColorChat.Yellow($"Message {FontColorChat.White($"{id}")} deleted"));
            }
            catch (MailBoxException e)
            {
                throw ctx.Error(e.Message);
            }
        }

        [Command("deleteall", usage: "", description: "Delete all messages from your mailbox", adminOnly: false)]
        public static void DeleteAllMessagesFromMailBox(ChatCommandContext ctx)
        {
            try
            {
                var message = MailBox.DeleteAllMessageFromMailBox(ctx.User.CharacterName.ToString().ToLower());
                ctx.Reply(FontColorChat.Yellow($"All messages in the mailbox have been deleted"));
            }
            catch (MailBoxException e)
            {
                throw ctx.Error(e.Message);
            }
        }
    }
}
