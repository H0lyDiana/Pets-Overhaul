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
    public sealed class Estee : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public float manaIncrease = 0.15f;
        public float manaMagicIncreasePer1 = 0.001f;
        public float penaltyMult = 0.5f;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.CelestialWand))
            {
                int manaMult;
                Player.statManaMax2 += (int)(Player.statManaMax2 * manaIncrease);
                if (Player.statManaMax2 >= 200)
                {
                    manaMult = Player.statManaMax2 - 200;
                }
                else
                {
                    manaMult = 0;
                }
                if (Player.GetTotalDamage<MagicDamageClass>().Additive > 1f)
                {
                    Player.GetDamage<MagicDamageClass>() -= (Player.GetTotalDamage<MagicDamageClass>().Additive - 1f) * penaltyMult;
                }
                Player.GetDamage<MagicDamageClass>() += manaMult * manaMagicIncreasePer1;
                if (Player.armor[0].netID == ItemID.SpectreHood && Player.armor[0].netID == ItemID.SpectreRobe && Player.armor[0].netID == ItemID.SpectrePants)
                {
                    Player.GetDamage<MagicDamageClass>() -= 0.15f;
                }
                if (Player.manaSick)
                {
                    Player.GetDamage<MagicDamageClass>() -= Player.manaSickReduction * 0.25f;
                }
            }
        }
    }
    public sealed class CelestialWand : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.CelestialWand;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Estee estee = Main.LocalPlayer.GetModPlayer<Estee>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.CelestialWand")
                        .Replace("<maxMana>", Math.Round(estee.manaIncrease * 100, 2).ToString())
                        .Replace("<dmgPenalty>", estee.penaltyMult.ToString())
                        .Replace("<manaToDmg>", Math.Round(estee.manaMagicIncreasePer1 * 100, 2).ToString())
                        ));
        }
    }
}
