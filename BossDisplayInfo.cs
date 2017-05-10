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
        
        public static void SetCustomHealthBar(int npcType, HealthBar healthBar)
        {
            //Set up value for out
            HealthBar hb = null;
            if (NPCHealthBars.TryGetValue(npcType, out hb))
            {
                // Remove old entry and replace
                NPCHealthBars.Remove(npcType);
                NPCHealthBars.Add(npcType, healthBar);
            }
            else
            {
                //  Add the entry
                NPCHealthBars.Add(npcType, healthBar);
            }
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
            if (npc.realLife >= 0) return false;

            // Cannot track npcs without a health bar
            if(npc.immortal || npc.dontTakeDamage)
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
            if (GetHealthBarForNPCOrNull(npc.type) != null)
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
