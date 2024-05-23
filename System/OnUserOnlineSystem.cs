using Bloodstone.API;
using BloodyMailBox.Utils;
using ProjectM;
using Stunlock.Network;
using System.Linq;
using BloodyMailBox.Exceptions;
using Bloody.Core;
using Bloody.Core.Models;
using BloodyBoss.Patch;
using Bloody.Core.Models.v1;
using Bloody.Core.GameData.v1;

namespace BloodyMailBox.System;

public class OnUserOnlineSystem
{

    internal static void OnUserOnline(ServerBootstrapSystem sender, NetConnectionId netConnectionId)
    {
        var userIndex = sender._NetEndPointToApprovedUserIndex[netConnectionId];
        var serverClient = sender._ApprovedUsersLookup[userIndex];
        var userEntity = serverClient.UserEntity;

        UserModel userModel = GameData.Users.FromEntity(userEntity);

        var action = () =>
        {
            OnTimedAuto(userModel);
        };
        ActionSchedulerPatch.RunActionOnceAfterDelay(action, 10);
    }

    private static void OnTimedAuto(UserModel user)
    {
        var messages = MailBox.ReadMailBox(user.CharacterName.ToString().ToLower());
        var messagesUnread = messages.Where(message => message.Open == false);
        var totalMessagesUnread = messagesUnread.Count();
        var entityManager = VWorld.Server.EntityManager;
        if (totalMessagesUnread > 0)
        {
            ServerChatUtils.SendSystemMessageToClient(entityManager, (ProjectM.Network.User)user.Internals.User, $"You have {FontColorChat.White($"{totalMessagesUnread}")} unread messages in your mailbox.");
            try
            {
                ServerChatUtils.SendSystemMessageToClient(entityManager, (ProjectM.Network.User)user.Internals.User, FontColorChat.Yellow($"------------ MailBox ------------"));
                foreach (var message in messagesUnread)
                {
                    var read = message.Open ? "Read" : "Unread";
                    ServerChatUtils.SendSystemMessageToClient(
                        entityManager,
                        (ProjectM.Network.User)user.Internals.User,
                        FontColorChat.Yellow($"[{FontColorChat.White($"{message.Id}")}][{FontColorChat.White($"{read}")}] Message Author: {FontColorChat.White($"{message.Author}")}")
                        );
                }
                ServerChatUtils.SendSystemMessageToClient(entityManager, (ProjectM.Network.User)user.Internals.User, FontColorChat.Yellow($"------------ End MailBox ------------"));
            }
            catch (MailBoxException e)
            {
                ServerChatUtils.SendSystemMessageToClient(entityManager, (ProjectM.Network.User)user.Internals.User, FontColorChat.Red($"{e.Message}"));
            }
        }
        if (messages.Count == Plugin.MaxMessages.Value)
            ServerChatUtils.SendSystemMessageToClient(entityManager, (ProjectM.Network.User)user.Internals.User, $"\r\n {FontColorChat.White($"ATTENTION")} \r\n {FontColorChat.Red("Your mailbox is full, delete messages to receive more.")}");

    }

}

