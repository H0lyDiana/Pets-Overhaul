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
    public sealed class Glommer : ModPlayer
    {
        public int glommerSanityTime = 45;
        public int glommerSanityRecover = 1;
        public float glommerSanityAura = 0.2f;
        public int glommerSanityRange = 4000;

        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.GlommerPetItem))
            {
                Pet.timerMax = glommerSanityTime;
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUse(ItemID.GlommerPetItem) && Main.rand.NextBool(18000))
            {
                Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), ItemID.PoopBlock);
            }

            if (Pet.PetInUseWithSwapCd(ItemID.GlommerPetItem))
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player plr = Main.player[i];
                    if (Player.Distance(plr.Center) < glommerSanityRange && plr.active && plr.whoAmI != 255)
                    {
                        plr.GetModPlayer<GlobalPet>().abilityHaste += glommerSanityAura;
                    }
                }
                Player.statManaMax2 += (int)(Pet.abilityHaste * Player.statManaMax2);
                if (Pet.timer <= 0 && Player.statMana != Player.statManaMax2)
                {
                    Player.statMana += glommerSanityRecover;
                    Pet.timer = Pet.timerMax;
                }
            }
        }
    }
    public sealed class GlommerPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.GlommerPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Glommer glommer = Main.LocalPlayer.GetModPlayer<Glommer>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.GlommerPetItem")
                        .Replace("<sanityAmount>", Math.Round(glommer.glommerSanityAura * 100, 2).ToString())
                        .Replace("<sanityRange>", Math.Round(glommer.glommerSanityRange / 16f, 2).ToString())
                        .Replace("<manaRecover>", glommer.glommerSanityRecover.ToString())
                        .Replace("<manaRecoverCd>", Math.Round(glommer.glommerSanityTime / 60f, 2).ToString())
                        ));
        }
    }
}
