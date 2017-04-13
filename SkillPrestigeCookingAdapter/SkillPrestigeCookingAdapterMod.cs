using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace SkillPrestigeCookingAdapter
{
    public class SkillPrestigeCookingAdapterMod : Mod
    {
        /*********
        ** Properties
        *********/
        private static IModHelper Helper;
        private static Texture2D _cookingSkillTexture;
        private const string IconFileName = @"iconA.png";


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            SkillPrestigeCookingAdapterMod.Helper = helper;
        }

        public static Texture2D GetCookingSkillTexture()
        {
            if (_cookingSkillTexture != null) return _cookingSkillTexture;
            try
            {
                var cookingSkillPath = GetCookingSkillModPath();
                var fileStream = new FileStream(Path.Combine(cookingSkillPath, IconFileName), FileMode.Open);
                _cookingSkillTexture = Texture2D.FromStream(Game1.graphics.GraphicsDevice, fileStream);
            }
            catch (Exception ex)
            {
                Log.Async("[Skill Prestige Cooking Adapter] failed to load icon: " + ex);
                _cookingSkillTexture = new Texture2D(Game1.graphics.GraphicsDevice, 16, 16);
                _cookingSkillTexture.SetData(Enumerable.Range(0, 256).Select(i => new Color(225, 168, byte.MaxValue)).ToArray());
            }
            return _cookingSkillTexture;
        }

        private static string GetCookingSkillModPath()
        {
            IModHelper modHelper = SkillPrestigeCookingAdapterMod.Helper;

            // get Cooking Skill mod's implementation from SMAPI internals
            // (This is a terrible idea, please don't do this.)
            IMod cookingSkillMod = modHelper.Reflection
                .GetPrivateValue<List<IMod>>(modHelper.ModRegistry, "Mods")
                .FirstOrDefault(p => p.ModManifest.UniqueID == "CookingSkill");

            // get mod's directory path
            return cookingSkillMod?.Helper.DirectoryPath;
        }
    }
}
