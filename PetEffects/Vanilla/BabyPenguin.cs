﻿using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class BabyPenguin : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        internal int penguinOldChilledTime = 0;
        public int snowFish = 25;
        public int oceanFish = 15;
        public int regularFish = 5;
        public float chillingMultiplier = 0.45f;
        public int snowFishChance = 100;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUse(ItemID.Fish))
            {
                if (Player.ZoneSnow)
                {
                    Player.fishingSkill += snowFish;
                    Player.accFlipper = true;
                }
                else if (Player.ZoneBeach)
                {
                    Player.fishingSkill += oceanFish;
                    Player.accFlipper = true;
                }
                else
                {
                    Player.fishingSkill += regularFish;
                }
            }

            if (Pet.PetInUseWithSwapCd(ItemID.Fish) && Player.HasBuff(BuffID.Chilled))
            {
                if (Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)] > penguinOldChilledTime)
                {
                    Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)] -= (int)(Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)] * chillingMultiplier);
                }
                penguinOldChilledTime = Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)];
            }


        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (Pet.PetInUse(ItemID.Fish) && (fish.type == ItemID.FrostMinnow || fish.type == ItemID.AtlanticCod))
            {
                for (int i = 0; i < ItemPet.Randomizer(snowFishChance * fish.stack); i++)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.fishingItem), fish, 1);
                }
            }
        }
    }
    public sealed class Fish : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.Fish;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            BabyPenguin babyPenguin = Main.LocalPlayer.GetModPlayer<BabyPenguin>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Fish")
                .Replace("<fp>", babyPenguin.regularFish.ToString())
                .Replace("<oceanFp>", babyPenguin.oceanFish.ToString())
                .Replace("<snowFp>", babyPenguin.snowFish.ToString())
                .Replace("<catchChance>", babyPenguin.snowFishChance.ToString())
                .Replace("<chilledMult>", babyPenguin.chillingMultiplier.ToString())
            ));
        }
    }
}
