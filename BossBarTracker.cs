using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace FKBossHealthBar
{
    /// <summary>
    /// Manage NPCs tracked
    /// </summary>
    public static class BossBarTracker
    {
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

            HealthBar hb = BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type);
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

        public static void UpdateNPCTracker()
        {

        }

        public static void DrawHealthBars(SpriteBatch spriteBatch)
        {
            float Alpha = 1f;
            if (HealthBar.MouseOver)
            {
                Alpha = Config.HealthBarUIFadeHover;
            }

            HealthBar.ResetStaticVars();

            int stack = 0;
            foreach (NPC npc in Main.npc)
            {
                if (CanTrackNPCHealth(npc))
                {
                    HealthBar hb = BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type);
                    if (hb == null) hb = new HealthBar();

                    hb.DrawHealthBarDefault(
                        spriteBatch, Alpha, stack,
                        npc.life, npc.lifeMax, npc);

                    stack++;
                }
            }
        }
    }
}
