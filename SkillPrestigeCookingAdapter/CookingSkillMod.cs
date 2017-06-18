using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SkillPrestige;
using SkillPrestige.Mods;
using SkillPrestige.Professions;
using SkillPrestige.SkillTypes;
using StardewValley;

namespace SkillPrestigeCookingAdapter
{
    /// <summary>The Prestige Skill mod.</summary>
    public class CookingSkillMod : ISkillMod
    {
        /*********
        ** Properties
        *********/
        /// <summary>Whether the Cooking Skill mod is loaded.</summary>
        private readonly bool IsLoaded;

        /// <summary>The experience needed for each level.</summary>
        private readonly int[] ExpNeededForLevel = { 100, 380, 770, 1300, 2150, 3300, 4800, 6900, 10000, 15000 };


        /*********
        ** Accessors
        *********/
        /// <summary>The name to display for the mod in the log.</summary>
        public string DisplayName { get; } = "Cooking Skill";

        /// <summary>Skills to add to the prestige system.</summary>
        public IEnumerable<Skill> AdditionalSkills => this.GetSkills();

        /// <summary>Empty objects that contain a skill type for the saved data.</summary>
        public IEnumerable<Prestige> AdditonalPrestiges => this.GetPrestiges();


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public CookingSkillMod()
        {
            this.IsLoaded = SkillPrestigeCookingAdapterMod.IsCookingSkillModLoaded;
        }

        /// <summary>Get the skills to add to the prestige system.</summary>
        private IEnumerable<Skill> GetSkills()
        {
            if (this.IsLoaded)
            {
                yield return new Skill
                {
                    Type = new SkillType("Cooking", 6),
                    SkillScreenPosition = SkillPrestigeCookingAdapterMod.IsLuckSkillModLoaded ? 7 : 6, // fix potential conflict with order due to luck skill mod
                    SourceRectangleForSkillIcon = new Rectangle(0, 0, 16, 16),
                    SkillIconTexture = SkillPrestigeCookingAdapterMod.IconTexture,
                    Professions = this.GetProfessions(),
                    SetSkillLevel = x => { }, // no set necessary, as the level isn't stored independently from the experience
                    GetSkillLevel = this.GetCookingLevel
                };
            }
        }

        /// <summary>Get the prestiges to add to the prestige system.</summary>
        private IEnumerable<Prestige> GetPrestiges()
        {
            if (this.IsLoaded)
                yield return new Prestige { SkillType = new SkillType("Cooking", 6) };
        }

        /// <summary>Get the cooking skill professions.</summary>
        private IEnumerable<Profession> GetProfessions()
        {
            var gourmet = new TierOneProfession
            {
                Id = 50,
                DisplayName = "Gourmet",
                EffectText = new[] { "+20% sell price" }
            };
            var satisfying = new TierOneProfession
            {
                Id = 51,
                DisplayName = "Satisfying",
                EffectText = new[] { "+25% buff duration once eaten" }
            };
            var efficient = new TierTwoProfession
            {
                Id = 52,
                DisplayName = "Efficient",
                EffectText = new[] { "15% chance to not consume ingredients" },
                TierOneProfession = gourmet
            };
            var professionalChef = new TierTwoProfession
            {
                Id = 53,
                DisplayName = "Prof. Chef",
                EffectText = new[] { "Home-cooked meals are always at least silver" },
                TierOneProfession = gourmet
            };
            var intenseFlavors = new TierTwoProfession
            {
                Id = 54,
                DisplayName = "Intense Flavors",
                EffectText = new[]
                {
                    "Food buffs are one level stronger",
                    "(+20% for max energy or magnetism)"
                },
                TierOneProfession = satisfying
            };
            var secretSpices = new TierTwoProfession
            {
                Id = 55,
                DisplayName = "Secret Spices",
                EffectText = new[] { "Provides a few random buffs when eating unbuffed food." },
                TierOneProfession = satisfying
            };
            gourmet.TierTwoProfessions = new List<TierTwoProfession>
            {
                efficient,
                professionalChef
            };
            satisfying.TierTwoProfessions = new List<TierTwoProfession>
            {
                intenseFlavors,
                secretSpices
            };

            return new Profession[]
            {
                gourmet,
                satisfying,
                efficient,
                professionalChef,
                intenseFlavors,
                secretSpices
            };
        }

        /// <summary>Get the player's current cooking level.</summary>
        private int GetCookingLevel()
        {
            this.FixExpLength();
            for (var index = ExpNeededForLevel.Length - 1; index >= 0; --index)
            {
                if (Game1.player.experiencePoints[6] >= ExpNeededForLevel[index])
                    return index + 1;
            }
            return 0;
        }

        private void FixExpLength()
        {
            if (Game1.player.experiencePoints.Length >= 7)
                return;
            var newExperienceArray = new int[7];
            for (var index = 0; index < 6; ++index)
                newExperienceArray[index] = Game1.player.experiencePoints[index];
            Game1.player.experiencePoints = newExperienceArray;
        }
    }
}
