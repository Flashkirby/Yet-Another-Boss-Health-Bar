using System.Linq;
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
        // Tracks all NPCs that should be drawing healthbars
        private static Dictionary<NPC, int> trackedNpcs;
        public static Dictionary<NPC, int> TrackedNPCs
        {
            get
            {
                if (trackedNpcs == null)
                {
                    trackedNpcs = new Dictionary<NPC, int>(255);
                }
                return trackedNpcs;
            }
        }

        public static void ResetTracker()
        {
            trackedNpcs = null;
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

            HealthBar hb = BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type);
            if (hb != null)
            {
                if (hb.DisplayMode == HealthBar.DisplayType.Disabled) return false;
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
            // Add and remove NPCs on the list
            foreach (NPC npc in Main.npc)
            {
                // Is NPC still trackable
                if (CanTrackNPCHealth(npc))
                {
                    // Not in list yet?
                    if (!TrackedNPCs.ContainsKey(npc))
                    {
                        // We tracking now
                        TrackedNPCs.Add(npc, (short)Config.HealthBarUIFadeTime);
                    }
                }
                else
                {
                    // But in was in the list before?
                    if (TrackedNPCs.ContainsKey(npc))
                    {
                        // First check if it's a multi-bar NPC
                        HealthBar hb = BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type);
                        if (hb != null)
                        {
                            if(hb.DisplayMode == HealthBar.DisplayType.Multiple && hb.multiShowCount > 1)
                            {
                                // Just remove it if there's more than 1 left
                                TrackedNPCs.Remove(npc);
                                break;
                            }
                        }

                        // Make a temp copy that won't be changing
                        // Do this to keep info of dead NPCs fpr a while
                        NPC temp = new NPC();
                        temp = (NPC)npc.Clone();
                        if(temp.life < 0) temp.life = 0;
                        TrackedNPCs.Add(temp, -Config.HealthBarUIFadeTime - 1);
                        TrackedNPCs.Remove(npc);
                    }
                }
            }

            // Sort the timers
            foreach (NPC tracked in TrackedNPCs.Keys.ToList())
            {
                if (TrackedNPCs[tracked] > 0)
                {
                    // Fade to 0
                    TrackedNPCs[tracked]--;
                }
                else if (TrackedNPCs[tracked] <= -1)
                {
                    if (TrackedNPCs[tracked] == -1)
                    {
                        // If reached -1, delete
                        TrackedNPCs.Remove(tracked);
                    }
                    else
                    {
                        // otherwise count UP to -1
                        TrackedNPCs[tracked]++;
                    }
                }
            }
        }

        private static float GetAlpha(NPC npc)
        {
            float Alpha = 1f;
            if (HealthBar.MouseOver)
            {
                Alpha = Config.HealthBarUIFadeHover;
            }

            try
            {
                // time will count from X to 0
                float time = TrackedNPCs[npc];
                if (time < 0) time += Config.HealthBarUIFadeTime + 1;
                Alpha = 1f - (time / Config.HealthBarUIFadeTime);
                return Alpha;
            }
            catch
            {
                return 0f;
            }
        }
        public static void DrawHealthBars(SpriteBatch spriteBatch)
        {

            // Reset static vars such as collective healthbars
            HealthBar.ResetStaticVars();

            int stack = 0;
            foreach(NPC npc in TrackedNPCs.Keys)
            {
                // Get the healthbar for this tracked NPC
                HealthBar hb = BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type);
                if (hb == null) hb = new HealthBar();

                hb.DrawHealthBarDefault(
                    spriteBatch, GetAlpha(npc), stack,
                    npc.life, npc.lifeMax, npc);

                stack++;
            }
        }
    }
}
