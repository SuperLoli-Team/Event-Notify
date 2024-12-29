using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozyLib
{
    [CustomItem(ItemType.GunAK)]
    public class GrenadeLauncher : CustomWeapon
    {
        public override uint Id { get; set; } = 5252;
        public override string Name { get; set; } = "ZZZ";
        public override string Description { get; set; } = "Rozy Grenade Launcher V. 228";
        public override float Weight { get; set; } = 2;
        public override byte ClipSize { get; set; } = byte.MaxValue;
        public override float Damage { get; set; } = 0;
        public override SpawnProperties SpawnProperties { get; set; }

        protected override void OnShooting(ShootingEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                if (!Check(ev.Firearm)) return;
                _ = ShootGrenades(ev.Player);
            }
        }
        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Attacker.CurrentItem)) return;
            ev.DamageHandler.Damage = 0f;
        }
        private async Task ShootGrenades(Player player)
        {
            var grenade = player.AddItem(ItemType.GrenadeHE) as ExplosiveGrenade;
            if (grenade != null)
            {
                grenade.FuseTime = 1f;
                await Task.Delay(25); player.ThrowGrenade(Exiled.API.Enums.ProjectileType.FragGrenade);
                player.RemoveItem(grenade);
            }
        }
    }
}