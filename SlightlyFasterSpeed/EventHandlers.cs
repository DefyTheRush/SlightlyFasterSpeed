using System;
using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using MEC;

namespace SlightlyFasterSpeed
{
    public class EventHandlers
    {
        HashSet<ReferenceHub> PlayersWith207 = new HashSet<ReferenceHub>();
        public void RunOnRACommandEvent(ref RACommandEvent Rcom)
        {
            string[] Arguments = Rcom.Command.Split(' ');
            ReferenceHub Sender = Rcom.Sender.SenderId == "SERVER CONSOLE" || Rcom.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(Rcom.Sender.SenderId);
            switch (Arguments[0].ToLower())
            {
                case "fastspeed":
                    Rcom.Allow = false;
                    if (!Sender.CheckPermission("sfs.allow"))
                    {
                        Rcom.Sender.RAMessage("You are not authorized to use this command!");
                        return;
                    }

                    try
                    {
                        if (!CheckIfIdIsValid(Int32.Parse(Arguments[1])))
                        {
                            Rcom.Sender.RAMessage("Please enter a player ID from the player list!");
                            return;
                        }

                        ReferenceHub ChosenPlayer = Player.GetPlayer(Int32.Parse(Arguments[1]));
                        if (!ChosenPlayer.TryGetComponent(out SpeedComponent spd))
                        {
                            Rcom.Sender.RAMessage("Faster speed enabled for player \"" + ChosenPlayer.GetNickname() + "\"!");
                            PlayersWith207.Add(ChosenPlayer);
                            ChosenPlayer.gameObject.AddComponent<SpeedComponent>();
                        }
                        else
                        {
                            Rcom.Sender.RAMessage("Faster speed disabled for player \"" + ChosenPlayer.GetNickname() + "\"!");
                            PlayersWith207.Remove(ChosenPlayer);
                            UnityEngine.Object.Destroy(ChosenPlayer.GetComponent<SpeedComponent>());
                        }
                    }
                    catch (Exception)
                    {
                        Rcom.Sender.RAMessage("Please enter a valid ID!");
                        return;
                    }
                    break;
            }
        }

        public void RunOnPlayerSpawn(PlayerSpawnEvent plySpwn)
        {
            if (PlayersWith207.Contains(plySpwn.Player))
                Timing.CallDelayed(1f, () => plySpwn.Player.effectsController.EnableEffect("SCP-207"));
        }

        public void RunOnRoundStart()
        {
            PlayersWith207.Clear();
        }

        private bool CheckIfIdIsValid(int id)
        {
            foreach (ReferenceHub hubs in Player.GetHubs())
            {
                if (hubs.GetPlayerId() == id)
                    return true;
            }
            return false;
        }
    }
}
