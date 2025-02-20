﻿using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class Puppy : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int catchChance = 65;
        public int rareCatchChance = 15;
        public int rareCritterCoin = 25000;
        public int rareEnemyCoin = 40000;
        public override void UpdateEquips()
        {
            if (Pet.PetInUse(ItemID.DogWhistle))
            {
                Player.AddBuff(BuffID.Hunter, 1);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUse(ItemID.DogWhistle) && target.active == false && target.rarity > 0 && target.CountsAsACritter == false && target.SpawnedFromStatue == false)
            {
                Pet.GiveCoins(ItemPet.Randomizer(rareEnemyCoin * target.rarity));
            }
        }
        public override void OnCatchNPC(NPC npc, Item item, bool failed)
        {

            if (Pet.PetInUse(ItemID.DogWhistle) && failed == false && npc.CountsAsACritter && npc.SpawnedFromStatue == false && npc.releaseOwner == 255)
            {
                if (npc.rarity > 0)
                {
                    Pet.GiveCoins(ItemPet.Randomizer(rareCritterCoin * npc.rarity));
                    for (int i = 0; i < ItemPet.Randomizer(rareCatchChance); i++)
                    {
                        Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), npc.catchItem, 1);
                        if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                        {
                            SoundEngine.PlaySound(SoundID.Item65 with { PitchVariance = 0.3f, MaxInstances = 5, Volume = 0.5f }, Player.position);
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < ItemPet.Randomizer(catchChance); i++)
                    {
                        Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), npc.catchItem, 1);
                        if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                        {
                            SoundEngine.PlaySound(SoundID.Item65 with { PitchVariance = 0.3f, MaxInstances = 1, Volume = 0.5f }, Player.position);
                        }
                    }

                }
            }
        }
    }
    public sealed class DogWhistle : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DogWhistle;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Puppy puppy = Main.LocalPlayer.GetModPlayer<Puppy>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DogWhistle")
                .Replace("<critter>", puppy.catchChance.ToString())
                .Replace("<rareCritter>", puppy.rareCatchChance.ToString())
                .Replace("<rareCritterCoin>", Math.Round(puppy.rareCritterCoin / 100f, 2).ToString())
                .Replace("<rareEnemyCoin>", Math.Round(puppy.rareEnemyCoin / 100f, 2).ToString())
            ));
        }
    }
}
