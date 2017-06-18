using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;

namespace SkillPrestigeCookingAdapter
{
    /// <summary>The SMAPI entry point.</summary>
    public class SkillPrestigeCookingAdapterMod : Mod
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The cooking skill icon.</summary>
        internal static Texture2D IconTexture;

        /// <summary>Whether the Cooking Skill mod is loaded.</summary>
        internal static bool IsCookingSkillModLoaded;

        /// <summary>Whether the Luck Skill mod is loaded.</summary>
        internal static bool IsLuckSkillModLoaded;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            SkillPrestigeCookingAdapterMod.IconTexture = helper.Content.Load<Texture2D>("icon.png");
            SkillPrestigeCookingAdapterMod.IsCookingSkillModLoaded = helper.ModRegistry.IsLoaded("spacechase0.CookingSkill");
            SkillPrestigeCookingAdapterMod.IsLuckSkillModLoaded = helper.ModRegistry.IsLoaded("spacechase0.LuckSkill");
        }
    }
}
