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
    public sealed class Turtle : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public float moveSpd = 0.12f;
        public float def = 1.11f;
        public float kbResist = 0.25f;
        public float dmgReduce = 0.05f;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seaweed))
            {
                Player.statDefense.FinalMultiplier *= def;
                Player.moveSpeed -= moveSpd;
                Player.maxRunSpeed -= moveSpd * 3;
                Player.GetDamage<GenericDamageClass>() -= dmgReduce;
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seaweed))
            {
                Player.maxRunSpeed -= moveSpd * 3;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seaweed))
            {
                modifiers.Knockback *= kbResist;
            }
        }
    }
    public sealed class Seaweed : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.Seaweed;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Turtle turtle = Main.LocalPlayer.GetModPlayer<Turtle>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Seaweed")
            .Replace("<def>", turtle.def.ToString())
                        .Replace("<kbResist>", turtle.kbResist.ToString())
                        .Replace("<moveSpd>", Math.Round(turtle.moveSpd * 100, 2).ToString())
                        .Replace("<dmg>", Math.Round(turtle.dmgReduce * 100, 2).ToString())
                        ));
        }
    }
}
