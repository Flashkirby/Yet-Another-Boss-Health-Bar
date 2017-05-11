using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

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

        public static bool CanTrackNPCHealth(NPC npc)
        {
            // Cannot track non-active entities
            if (!npc.active) return false;

            // Don't track NPCs which are part of a shared life chain (except the head of course)
            if (npc.realLife >= 0 && npc.realLife != npc.whoAmI) return false;

            // Too far away to bother showing a health bar
            bool tooFar = (npc.position.X < Main.LocalPlayer.position.X - Config.HealthBarDrawDistance ||
                npc.position.X > Main.LocalPlayer.position.X + Config.HealthBarDrawDistance ||
                npc.position.Y < Main.LocalPlayer.position.Y - Config.HealthBarDrawDistance ||
                npc.position.Y > Main.LocalPlayer.position.Y + Config.HealthBarDrawDistance);

            HealthBar hb = GetHealthBarForNPCOrNull(npc.type);
            if (hb != null)
            {
                if (hb.DisplayMode == HealthBar.DisplayType.Disabled || hb.multiShowOnce) return false;
                if (hb.ShowHealthBarOverride(npc, tooFar)) return true;
            }

            // Cannot track npcs without a health bar
            if (npc.immortal || npc.dontTakeDamage || npc.dontTakeDamageFromHostiles || tooFar)
            {
                return false;
            }

            // The modded NPC doesn't want to show health?
            if (npc.modNPC != null)
            {
                float scale = 1f;
                Vector2 position = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.gfxOffY);
                bool? result = npc.modNPC.DrawHealthBar(Main.HealthBarDrawSettings, ref scale, ref position);
                if (result == false)
                {
                    return false;
                }
            }
            // Someone has specified a healthbar for this NPC?
            if (hb != null)
            {
                return true;
            }

            // Otherwise if it's a boss then also sure 
            if (npc.boss)
            {
                return true;
            }

            return false;
        }
    }
}
