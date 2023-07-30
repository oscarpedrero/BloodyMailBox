using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using VampireCommandFramework;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using VRising.GameData;
using Bloodstone.API;
using Unity.Entities;
using System.Security.Cryptography.X509Certificates;

namespace BloodyMailBox
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("gg.deca.VampireCommandFramework")]
    [BepInDependency("gg.deca.Bloodstone")]
    public class Plugin : BasePlugin
    {

        public static ManualLogSource Logger;

        private Harmony _harmony;

        public static ConfigEntry<bool> OwnMessages;
        public static ConfigEntry<int> MaxMessages;

        public override void Load()
        {
            // Plugin startup logic
            CommandRegistry.RegisterAll();
            GameData.OnInitialize += GameDataOnInitialize;
            GameData.OnDestroy += GameDataOnDestroy;
            InitConfigServer();
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            
        }

        private void InitConfigServer()
        {
            OwnMessages = Config.Bind("Config", "ownMessages", false, "Allows you to send messages to your mailbox to yourself.");
            MaxMessages = Config.Bind("Config", "maxMessages", 20, "Maximum number of messages that a user can have in the mailbox. If the value is 0 they are infinite (Not recommended)");
        }

        private static void GameDataOnInitialize(World world)
        {
            
        }

        private static void GameDataOnDestroy()
        {
            //Logger.LogInfo("GameDataOnDestroy");
        }
    }
}
