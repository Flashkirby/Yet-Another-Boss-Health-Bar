using System;
using System.Reflection;
using System.Collections.Generic;

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

        public static HealthBar GetHealthBarForNPC(int npcType)
        {
            HealthBar hb = null;
            if (NPCHealthBars.TryGetValue(npcType, out hb))
            {
                return hb;
            }
            else
            {
                // A default healthbar
                return new HealthBar(npcType);
            }
        }
    }
}
