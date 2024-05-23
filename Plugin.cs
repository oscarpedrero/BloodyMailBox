using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using VampireCommandFramework;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using Unity.Entities;
using Bloody.Core.API;
using BloodyMailBox.System;
using Bloody.Core.API.v1;

namespace BloodyMailBox
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("gg.deca.VampireCommandFramework")]
    [BepInDependency("gg.deca.Bloodstone")]
    [BepInDependency("trodi.Bloody.Core")]
    public class Plugin : BasePlugin
    {

        public static ManualLogSource Logger;

        private Harmony _harmony;

        public static ConfigEntry<bool> OwnMessages;
        public static ConfigEntry<int> MaxMessages;

        public override void Load()
        {
            
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Plugin startup logic
            CommandRegistry.RegisterAll();

            EventsHandlerSystem.OnInitialize += GameDataOnInitialize;
            EventsHandlerSystem.OnDestroy += GameDataOnDestroy;
            InitConfigServer();

            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            
        }

        private void InitConfigServer()
        {
            OwnMessages = Config.Bind("Config", "ownMessages", false, "Allows you to send messages to your mailbox to yourself.");
            MaxMessages = Config.Bind("Config", "maxMessages", 20, "Maximum number of messages that a user can have in the mailbox. If the value is 0 they are infinite (Not recommended)");
        }

        private static void GameDataOnInitialize(World world)
        {
            EventsHandlerSystem.OnUserConnected += OnUserOnlineSystem.OnUserOnline;
        }

        private static void GameDataOnDestroy()
        {
            //Logger.LogInfo("GameDataOnDestroy");
        }
    }
}
