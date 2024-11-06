using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string WebhookUrl { get; set; } = "putyourwebhookurl";

        public string WebhookUrlAbus { get; set; } = "putyourwebhookurl";
        public bool EventCodeActive { get; set; } = true;
        public bool EscapeCodeActive { get; set; } = true;
        public bool TeslaCodeActive { get; set; } = true;
        public bool HidCodeActive { get; set; } = true;
        public bool InfDCodeActive { get; set; } = true;
        public bool InfWCodeActive { get; set; } = true;
        public bool DCDCodeActive { get; set; } = true;
        public string EventHint { get; set; } = "<size=20><align=right><line-indent=-25.8em>\r\n        <b><color=#DA70D6>🙋Event Master:</color></b> {master}\r\n        <b><color=#BA55D3>😻Level of RP:</color></b> {rp}\r\n        <b><color=#8A2BE2>👓Event:</color></b> {name}\r\n      <b><color=#9932CC>⏰Last</color></b> {min}:{sec}<line-height=65em></align></size>";
        public string PreparationHint { get; set; } = "<size=20><align=right><line-indent=-25.8em>\r\n        <b><color=#DA70D6>🙋Event Master:</color></b> {master}\r\n        <b><color=#BA55D3>😻Level of RP:</color></b> {rp}\r\n        <b><color=#8A2BE2>👓Prepreparation for Event:</color></b> {name}\r\n      <b><color=#9932CC>⏰The preparation lasts</color></b> {min}:{sec}<line-height=65em></align></size>";
        public bool RoundDependence { get; set; } = true;
        public bool Debug { get; set; } = false;
    }
}