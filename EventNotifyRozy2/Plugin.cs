using Discord.WebSocket;
using Discord;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PluginAPI.Events;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Items;
using InventorySystem.Items.MicroHID;
using SCPSLAudioApi;
using System.Collections;
using System.IO;
using Interactables.Interobjects.DoorUtils;
using Exiled.API.Enums;
using Subtitles;
using RozyLib;
using Exiled.CustomItems.API.Features;
using Exiled.CustomItems.API;
using HarmonyLib;

namespace EventNotifyRozy2
{
    public class EventPugin : Plugin<Config>
    {
        public override string Prefix => "EventNotifyRozy2";
        public override string Name => "EventNotifyRozy2";
        public override string Author => "SuperLoli Collective";
        public override System.Version Version { get; } = new System.Version(1, 4, 88);

        public static EventPugin plugin;
        public static CoroutineHandle hintCoroutine;
        public static bool EventMode = false;
        public static bool EventPreparation = false;
        public static string EventMaster = "Unknown";
        public static string EventName = "Unknown";
        public static string EventRP = "Unknown";
        public static TimeSpan time = new TimeSpan(0, 0, 0);
        public readonly HashSet<Player> InfHidPlayers = new HashSet<Player>();
        public readonly HashSet<Player> InfAmmoPlayers = new HashSet<Player>();
        public Dictionary<int, string> ProtectedChannels { get; private set; } = new Dictionary<int, string>();
        public Dictionary<string, Dictionary<string, float>> GunDamage { get; } = new Dictionary<string, Dictionary<string, float>>();
        public static List<int> EventHelpers { get; } = new List<int>();
        private static bool TeslaDisabled { get; set; } = false;
        public static EventPugin Instance { get; private set; }

        public static string EventMasterGroup = "Unknown";
        public static GrenadeLauncher GrenadeLauncher { get; private set; }
        private readonly Harmony harmony = new Harmony($"com.eventnotifyrozy2.plugin.{DateTime.Now.Ticks}");
        public static string GetUserGroup(CommandSender sender)
        {
            var player = Player.Get(sender as CommandSender);
            return player.Group?.BadgeText ?? "Unknown group";
        }
        private static bool _teslaDisabled = false;
        public bool WindowsActive { get; set; }
        public bool DoorsActive { get; set; }
        public bool EscapeActive { get; set; }
        public bool HidActive { get; set; }
        public bool TeslaActive { get; set; }
        public static bool GetTeslaDisabled()
        {
            return _teslaDisabled;
        }

        public static void SetTeslaDisabled(bool value)
        {
            _teslaDisabled = value;
        }

        public override void OnEnabled()
        {
            string rozy = @"
██████╗   ██████╗    ██████╗   ██╗   ██╗
██╔══██╗  ██╗   ██╗  ╚════██║  ██║   ██║
██████╔╝  ██║   ██║      ██╔╝  ██║   ██║
██ ██═╝   ██║   ██║     ██╔╝   ╚██╗ ██╔╝
██║  ██   ╚██████╔╝    ██╔╝      ╚███╔╝
╚═╝    ██  ╚═════╝    ██████╗    ╚███╔╝
                      ╚═════╝    ╚═══╝
";
            Log.Info("==================================================");
            Log.Info(" Thank you for trusting me UwU <3");
            Log.Info("         Maded by SuperLoli Collective");
            Log.Info("===========================================");
            Log.Info(">> Log in to our discord server: https://discord.gg/GCmkernxYV <<");
            Log.Send(rozy, Discord.LogLevel.Info, ConsoleColor.Yellow);
            plugin = this;
            Exiled.Events.Handlers.Server.RoundEnded += OnEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingPlayers;
            Exiled.Events.Handlers.Player.TriggeringTesla += OnTriggeringTesla;
            Exiled.Events.Handlers.Server.LocalReporting += OnLocalReporting;
            Exiled.Events.Handlers.Player.UsingRadioBattery += OnUsingRadioBattery;
            Exiled.Events.Handlers.Player.PlayerDamageWindow += OnPlayerDamageWindow;
            Exiled.Events.Handlers.Player.DamagingDoor += OnDamagingDoor;
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy += OnUsingMicroHIDEnergy;
            harmony.PatchAll();
            GrenadeLauncher = new GrenadeLauncher();
            CustomWeapon.RegisterItems();
            CustomItem.RegisterItems();
            WindowsActive = false;
            DoorsActive = false;
            base.OnEnabled();
            Instance = this;
        }
public void OnUsingMicroHIDEnergy(UsingMicroHIDEnergyEventArgs ev)
        {
            if (HidActive)
            {
                ev.IsAllowed = false;
            }
        }
        public void OnEscaping(EscapingEventArgs ev)
        {
            if (EscapeActive)
            {
                ev.IsAllowed = false;
            }
        }
            public void OnPlayerDamageWindow(DamagingWindowEventArgs ev)
        {
            if (WindowsActive)
            {
                ev.IsAllowed = false;
            }
        }
        public void OnDamagingDoor(DamagingDoorEventArgs ev)
        {
            if (DoorsActive)
            {
                ev.IsAllowed = false;
            }
        }
        private void OnUsingRadioBattery(UsingRadioBatteryEventArgs ev)
        {
            ev.IsAllowed = false;
        }
        private void OnLocalReporting(LocalReportingEventArgs ev)
        {
            string message = $"<b><color=green>{ev.Player.Sender.Nickname} </color>reports about <color=red>{ev.Target.Nickname}</color> for reason: <color=blue>{ev.Reason}</color></color></b>";

            foreach (Player admin in Player.List.Where(p => p.Group?.KickPower >= 1))
            {
                Timing.RunCoroutine(SendBroadcast(admin, message));
            }
        }

        private IEnumerator<float> SendBroadcast(Player admin, string message)
        {
            admin.Broadcast(10, message);
            yield return 0f;
        }


        public static void OnEnded(RoundEndedEventArgs ev)
        {
            if (plugin.Config.RoundDependence && (EventMode || EventPreparation))
            {
                EventMode = false;
                EventPreparation = false;
                Timing.KillCoroutines(hintCoroutine);
                time = new TimeSpan(0, 0, 0);
            }
        }
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (TeslaActive)
            {
                ev.DisableTesla = true;
            }
        }

        public void SetTeslaState(bool state)
        {
            TeslaDisabled = state;
        }


        public static IEnumerator<float> HintCoroutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1f);
                int isec = (int)time.TotalSeconds % 60;
                int imin = (int)time.TotalSeconds / 60;
                string hint = EventPreparation ? plugin.Config.PreparationHint : plugin.Config.EventHint;
                hint = hint.Replace("{master}", EventMaster).Replace("{rp}", EventRP).Replace("{name}", EventName);
                string sec = Convert.ToString(isec);
                string min = Convert.ToString(imin);
                if (isec < 10)
                {
                    sec = "0" + sec;
                }
                if (imin < 10)
                {
                    min = "0" + min;
                }
                hint = hint.Replace("{min}", min).Replace("{sec}", sec);
                time = time.Add(TimeSpan.FromSeconds(1));
                foreach (Player player in Player.List)
                {
                    player.ShowHint(hint, 1.1f);
                }
            }
        }

        private static void OnWaitingPlayers()
        {
        }

        public override void OnDisabled()
        {
            plugin = null;
            Exiled.Events.Handlers.Server.RoundEnded -= OnEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingPlayers;
            Exiled.Events.Handlers.Player.TriggeringTesla -= OnTriggeringTesla;
            Exiled.Events.Handlers.Server.LocalReporting -= OnLocalReporting;
            Exiled.Events.Handlers.Player.UsingRadioBattery -= OnUsingRadioBattery;
            Exiled.Events.Handlers.Player.PlayerDamageWindow -= OnPlayerDamageWindow;
            Exiled.Events.Handlers.Player.DamagingDoor -= OnDamagingDoor;
            harmony.UnpatchAll();
            ProtectedChannels.Clear();
            CustomWeapon.UnregisterItems();
            CustomItem.UnregisterItems();
            GrenadeLauncher = null;
            Instance = null;
            WindowsActive = false;
            DoorsActive = false;
            {
                base.OnDisabled();
            }
        }
    }
}