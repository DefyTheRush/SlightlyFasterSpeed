using System;
using EXILED;

namespace SlightlyFasterSpeed
{
    public class Plugin : EXILED.Plugin
    {
        bool IsPluginEnabled;
        public EventHandlers Handler;
        public override string getName { get; } = "SlightlyFasterSpeed - By DefyTheRush";

        public void ReloadConfig()
        {
            IsPluginEnabled = Config.GetBool("slightlyfasterspeed_enable");
            if (!IsPluginEnabled)
                Log.Info("Plugin disabled!");
            else
                Log.Info("Plugin enabled!");
        }

        public override void OnDisable()
        {
            Events.PlayerSpawnEvent -= Handler.RunOnPlayerSpawn;
            Events.RoundStartEvent -= Handler.RunOnRoundStart;
            Events.RemoteAdminCommandEvent -= Handler.RunOnRACommandEvent;
            Handler = null;
            Log.Info("Disabling \"SlightlyFasterSpeed\"!");
        }

        public override void OnEnable()
        {
            ReloadConfig();
            if (!IsPluginEnabled)
                return;

            Log.Info("Starting up \"SlightlyFasterSpeed\"! (Created by DefyTheRush)");
            Handler = new EventHandlers();
            Events.RemoteAdminCommandEvent += Handler.RunOnRACommandEvent;
            Events.RoundStartEvent += Handler.RunOnRoundStart;
            Events.PlayerSpawnEvent += Handler.RunOnPlayerSpawn;
        }

        public override void OnReload()
        {
            Log.Info("Reloading \"FasterSpeed\"!");
        }
    }
}
