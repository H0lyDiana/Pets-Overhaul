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
    public sealed class VoltBunny : ModPlayer
    {
        public float movespdFlat = 0.05f;
        public float movespdMult = 1.08f;
        public float movespdToDmg = 0.2f;
        public float staticParalysis = 3f;
        public int staticLength = 45;
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LightningCarrot))
            {
                Player.moveSpeed += movespdFlat;
                Player.moveSpeed *= movespdMult;
                Player.GetDamage<GenericDamageClass>() += (Player.moveSpeed - 1f) * movespdToDmg;
            }
        }
    }
    public sealed class EnemyHittingVoltBunny : GlobalNPC
    {
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (target.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.LightningCarrot) && npc.TryGetGlobalNPC(out NpcPet npcPet))
            {
                npcPet.AddSlow(NpcPet.SlowId.PikachuStatic, target.GetModPlayer<VoltBunny>().staticParalysis, target.GetModPlayer<VoltBunny>().staticLength);
            }
        }
    }
    public sealed class LightningCarrot : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.LightningCarrot;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            VoltBunny voltBunny = Main.LocalPlayer.GetModPlayer<VoltBunny>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.LightningCarrot")
                       .Replace("<flatSpd>", Math.Round(voltBunny.movespdFlat * 100, 2).ToString())
                       .Replace("<multSpd>", voltBunny.movespdMult.ToString())
                       .Replace("<spdToDmg>", Math.Round(voltBunny.movespdToDmg * 100, 2).ToString())
                       .Replace("<staticAmount>", Math.Round(voltBunny.staticParalysis * 100, 2).ToString())
                       .Replace("<staticTime>", Math.Round(voltBunny.staticLength / 60f, 2).ToString())
                       ));
        }
    }
}
