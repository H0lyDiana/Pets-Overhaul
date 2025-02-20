﻿using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class BabyEater : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public float moveSpd = 0.10f;
        public float jumpSpd = 0.5f;
        public int fallDamageTile = 20;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.EatersBone))
            {
                if (Player.ZoneCorrupt || Player.ZoneCrimson)
                {
                    Player.extraFall += fallDamageTile;
                }

                Player.moveSpeed += moveSpd;
                Player.jumpSpeedBoost += jumpSpd;
                Player.autoJump = true;
            }
        }
        public override void PostUpdate()
        {
            if (Pet.PetInUse(ItemID.EatersBone))
            {
                Player.armorEffectDrawShadow = true;
            }
        }
    }
    public sealed class EatersBone : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.EatersBone;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            BabyEater babyEater = Main.LocalPlayer.GetModPlayer<BabyEater>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.EatersBone")
                .Replace("<moveSpd>", Math.Round(babyEater.moveSpd * 100, 2).ToString())
                .Replace("<jumpSpd>", Math.Round(babyEater.jumpSpd * 100, 2).ToString())
                .Replace("<fallRes>", babyEater.fallDamageTile.ToString())
            ));
        }
    }
}
