using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Permissions;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using Math = System.Math;

namespace RocketmodTemplate
{
    public class Main : RocketPlugin<Config>
    {
        public static Main Instance;
		public static string PluginName = "RocketmodTemplate";
        public static string PluginVersion = "1.0.0";

        protected override void Load()
        {
            Instance = this;

            Logger.Log("RocketmodTemplate has been loaded!", ConsoleColor.Yellow);
			Logger.Log(PluginName + " v" + PluginVersion, ConsoleColor.Yellow);
            Logger.Log("Made by Tortellio", ConsoleColor.Yellow);
            Logger.Log("Visit Tortellio Discord for more! https://discord.gg/pzQwsew", ConsoleColor.Yellow);

            U.Events.OnPlayerConnected += OnPlayerConnect;
        }

        protected override void Unload()
        {
            Instance = null;

            Logger.Log("RocketmodTemplate has been unloaded!", ConsoleColor.Yellow);
			Logger.Log("Visit Tortellio Discord for more! https://discord.gg/pzQwsew", ConsoleColor.Yellow);

            U.Events.OnPlayerConnected -= OnPlayerConnect;
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "example", "A fox jump a dog" },
        };
        
        private void OnPlayerConnect(UnturnedPlayer player)
        {
        }
    }
}