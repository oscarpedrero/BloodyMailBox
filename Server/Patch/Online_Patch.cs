using Bloodstone.API;
using BloodyMailBox.Utils;
using HarmonyLib;
using ProjectM;
using Stunlock.Network;
using System.Linq;
using VRising.GameData;
using BloodyMailBox.Server.Timer;
using System;
using VRising.GameData.Models;
using BloodyMailBox.Exceptions;

namespace BloodyMailBox.Patch;

[HarmonyPatch]
public class ServerBootstrapSystem_Patch
{

    private static AutoLoadNewMessages _autoLoadTimer;

    [HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserConnected))]
    [HarmonyPrefix]
    public static void Postfix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId)
    {
        
        var userIndex = __instance._NetEndPointToApprovedUserIndex[netConnectionId];
        var serverClient = __instance._ApprovedUsersLookup[userIndex];
        var userEntity = serverClient.UserEntity;

        var user = GameData.Users.FromEntity(userEntity);
        _autoLoadTimer = new AutoLoadNewMessages();
        StartAutoLoad(user);


    }

    private static void StartAutoLoad(UserModel user)
    {
        _autoLoadTimer.Start(
            world =>
            {
                OnTimedAuto(user);
            },
            input =>
            {
                if (input is not int secondAutoUIr)
                {
                    Plugin.Logger.LogError("Starting timer delay function parameter is not a valid integer");
                    return TimeSpan.MaxValue;
                }

                var seconds = 10;
                return TimeSpan.FromSeconds(seconds);
            });
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
        if(messages.Count == Plugin.MaxMessages.Value) 
            ServerChatUtils.SendSystemMessageToClient(entityManager, (ProjectM.Network.User)user.Internals.User, $"\r\n {FontColorChat.White($"ATTENTION")} \r\n {FontColorChat.Red("Your mailbox is full, delete messages to receive more.")}");
        StopAutoLoad();
    }

    public static void StopAutoLoad()
    {
        _autoLoadTimer?.Stop();
    }

}

