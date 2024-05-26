using BepInEx;
using BloodyMailBox.Common.Models;
using BloodyMailBox.Exceptions;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Schema;

namespace BloodyMailBox.Utils
{
    internal class MailBox
    {

        public static readonly string ConfigPath = Path.Combine(Paths.ConfigPath, "BloodyMailBox");
        public static readonly string MailBoxPath = Path.Combine(ConfigPath, "MailBox");

        public static bool CreateMailBox(string user)
        {

            if (!Directory.Exists(ConfigPath)) Directory.CreateDirectory(ConfigPath);
            if (!Directory.Exists(MailBoxPath)) Directory.CreateDirectory(MailBoxPath);
            var jsonUser = Path.Combine(MailBoxPath, user + ".json");
            if (!File.Exists(jsonUser))
            {
                File.WriteAllText(jsonUser, "[]");
                return true;
            }
            
            return false;
            

        }

        public static List<MessageModel> ReadMailBox(string user)
        {
            try
            {
                var jsonUser = Path.Combine(MailBoxPath, user + ".json");
                if (!File.Exists(jsonUser))
                {
                    CreateMailBox(user);
                    return new List<MessageModel>();
                }
                string json = File.ReadAllText(jsonUser);
                var messages = JsonSerializer.Deserialize<List<MessageModel>>(json);

                return messages;
            } catch
            {
                return new List<MessageModel>();
            }
            

        }

        public static void SaveMailBox(string user, List<MessageModel> messages)
        {
            var jsonUser = Path.Combine(MailBoxPath, user + ".json");
            var jsonOutPut = JsonSerializer.Serialize(messages);
            File.WriteAllText(jsonUser, jsonOutPut);
            return;

        }

        public static void MessageToMailBox(string user, string message, string fromUser)
        {
            var messageModel = new MessageModel();
            List<MessageModel> MailBoxMessages = new List<MessageModel>();

            if (CreateMailBox(user))
            {
                messageModel.Id = 1;
            } else
            {
                var total = 0;
                MailBoxMessages = ReadMailBox(user);
                if(Plugin.MaxMessages.Value > 0 && MailBoxMessages.Count == Plugin.MaxMessages.Value) throw new MailBoxException($"The user {FontColorChat.White($"{user}")} has no space in the mailbox");

                if(MailBoxMessages.Count() > 0)
                {
                    total = MailBoxMessages.OrderByDescending(p => p.Id)
                            .Select(r => r.Id)
                            .First();
                }

                messageModel.Id = total + 1;
            }

            messageModel.Author = fromUser;
            messageModel.Body = message;
            messageModel.Open = false;

            MailBoxMessages.Add(messageModel);
            SaveMailBox(user, MailBoxMessages);

            return;
        }
        
        public static MessageModel SelectMessageFromMailBox(string user, int id)
        {
            List<MessageModel> MailBoxMessages = new List<MessageModel>();
            if (CreateMailBox(user))
            {
                throw new MailBoxException("The selected message does not exist.");
            }
            MailBoxMessages = ReadMailBox(user);
            var message = MailBoxMessages.FirstOrDefault(message => message.Id == id);

            return message;

        }
        
        public static bool DeleteMessageFromMailBox(string user, int id)
        {
            SelectMessageFromMailBox(user, id);
            List<MessageModel> MailBoxMessages = new List<MessageModel>();
            if (CreateMailBox(user))
            {
                throw new MailBoxException("The selected message does not exist.");
            }
            MailBoxMessages = ReadMailBox(user);
            var message = MailBoxMessages.RemoveAll(message => message.Id == id);
            SaveMailBox(user, MailBoxMessages);
            return true;

        }

        public static bool DeleteAllMessageFromMailBox(string user)
        {
            List<MessageModel> MailBoxMessages = new List<MessageModel>();
            if (CreateMailBox(user))
            {
                throw new MailBoxException("You don't have any messages to delete.");
            }
            SaveMailBox(user, MailBoxMessages);
            return true;

        }

        public static bool SetOpenedMessageFromMailBox(string user, int id)
        {
            SelectMessageFromMailBox(user, id);
            List<MessageModel> MailBoxMessages = new List<MessageModel>();
            if (CreateMailBox(user))
            {
                throw new MailBoxException("The selected message does not exist.");
            }
            MailBoxMessages = ReadMailBox(user);
            MailBoxMessages.Where(message => message.Id == id).ToList().ForEach(message => message.Open = true);
            SaveMailBox(user, MailBoxMessages);

            return true;

        }
    }
}
