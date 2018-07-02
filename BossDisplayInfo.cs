using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace FKBossHealthBar
{
    /// <summary>
    /// Class used to hold the dictionary of npcs->healthbars
    /// </summary>
    public static class BossDisplayInfo
    {
        // Matches npc types to healthbar objects
        private static Dictionary<int, HealthBar> npchb;
        internal static Dictionary<int, HealthBar> NPCHealthBars
        {
            get
            {
                if (npchb == null)
                {
                    npchb = new Dictionary<int, HealthBar>();
                }
                return npchb;
            }
        }
        internal static void ResetNPCHealthBars()
        {
            npchb = null;
        }

        /// <summary>
        /// Register this health bar to this NPC for use in-game
        /// </summary>
        /// <param name="npcType"></param>
        /// <param name="healthBar"></param>
        public static void SetCustomHealthBar(int npcType, HealthBar healthBar)
        {
            //Set up value for out
            NPCHealthBars[npcType] = healthBar;
            healthBar.DisplayMode = HealthBar.DisplayType.Standard;
        }

        /// <summary>
        /// Register this health bar as a multiple npc bar, meaning it is shared between all npcs of the types
        /// </summary>
        /// <param name="healthBar">Same as usual</param>
        /// <param name="expectedMax">How many npcs are expected per healthbar?</param>
        /// <param name="npcTypes"></param>
        public static void SetCustomHealthBarMultiple(HealthBar healthBar, params int[] npcTypes)
        {
            foreach(int npcType in npcTypes)
            {
                SetCustomHealthBar(npcType, healthBar);
            }
            healthBar.DisplayMode = HealthBar.DisplayType.Multiple;
            healthBar.multiNPCType = npcTypes;
        }

        public static HealthBar GetHealthBarForNPCOrNull(int npcType)
        {
            HealthBar hb = null;
            NPCHealthBars.TryGetValue(npcType, out hb);
            return hb;
        }
    }
}
