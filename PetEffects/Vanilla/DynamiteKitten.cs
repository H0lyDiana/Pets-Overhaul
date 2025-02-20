﻿using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class DynamiteKitten : ModPlayer
    {
        public int cooldown = 120;
        public float damageMult = 0.6f;
        public float kbMult = 1.7f;
        public int armorPen = 15;

        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.BallOfFuseWire))
            {
                Pet.timerMax = cooldown;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BallOfFuseWire) && Pet.timer <= 0)
            {
                int boom = Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.petProjectile), target.position, Vector2.Zero, ModContent.ProjectileType<DynamiteKittyBoom>(), (int)(damageDone * damageMult), hit.Knockback * kbMult, Main.myPlayer);
                Main.projectile[boom].ArmorPenetration = armorPen;
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class BallOfFuseWire : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BallOfFuseWire;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            DynamiteKitten dynamiteKitten = Main.LocalPlayer.GetModPlayer<DynamiteKitten>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BallOfFuseWire")
                        .Replace("<kb>", dynamiteKitten.kbMult.ToString())
                        .Replace("<dmg>", dynamiteKitten.damageMult.ToString())
                        .Replace("<armorPen>", dynamiteKitten.armorPen.ToString())
                        ));
        }
    }
}
