using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace FKBossHealthBar
{
    /// <summary>
    /// Manage NPCs tracked, called by FKBossHealthBar through UpdateNPCTracker and ResetTracker
    /// </summary>
    public static class BossBarTracker
    {
        private const bool DEBUG_TRACKER = false;

        // Tracks all NPCs that should be drawing healthbars
        private static Dictionary<NPC, int> trackedNpcs;
        internal static Dictionary<NPC, int> TrackedNPCs
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

        private static Dictionary<NPC, int> trackedNPCOldLife;
        internal static Dictionary<NPC, int> TrackedNPCOldLife
        {
            get
            {
                if (trackedNPCOldLife == null)
                {
                    trackedNPCOldLife = new Dictionary<NPC, int>(255);
                }
                return trackedNPCOldLife;
            }
        }
        private static Dictionary<NPC, float> trackedNPCChipLife;
        internal static Dictionary<NPC, float> TrackedNPCChipLife
        {
            get
            {
                if (trackedNPCChipLife == null)
                {
                    trackedNPCChipLife = new Dictionary<NPC, float>(255);
                }
                return trackedNPCChipLife;
            }
        }
        private static Dictionary<NPC, int> trackedNPCChipTime;
        internal static Dictionary<NPC, int> TrackedNPCChipTime
        {
            get
            {
                if (trackedNPCChipTime == null)
                {
                    trackedNPCChipTime = new Dictionary<NPC, int>(255);
                }
                return trackedNPCChipTime;
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
            if (npc.timeLeft <= 0) return false;
            if (npc.life <= 0) return false;

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
                if (hb.DisplayMode == HealthBar.DisplayType.Disabled || 
                    hb.HideHealthBarOverride(npc, tooFar)) return false;
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
            if (DEBUG_TRACKER && Main.time % 60 == 0)
            {
                string tracked = "list: ";
                foreach (NPC npc in TrackedNPCs.Keys)
                {
                    tracked += string.Concat("[", npc.whoAmI, "]");//(char)(65 + npc.whoAmI);
                    Main.NewText(string.Concat(npc.GivenOrTypeName + ":", npc.active, "|", npc.timeLeft, "|", npc.life, "|", npc.lifeMax, "|trackable?", CanTrackNPCHealth(npc)));
                }
                Main.NewTextMultiline(tracked);
            }

            // Add and remove NPCs on the list
            foreach (NPC npc in Main.npc)
            {
                // Is NPC still trackable
                if (CanTrackNPCHealth(npc))
                {
                    // Not in list yet?
                    if (!TrackedNPCs.ContainsKey(npc))
                    {
                        if (DEBUG_TRACKER) Main.NewText(npc.GivenOrTypeName + " [" + npc.whoAmI + "] added");
                        TrackedNPCs.Add(npc, (short)Config.HealthBarUIFadeTime);
                    }
                }
                else
                {
                    // But in was in the list before?
                    //if (TrackedNPCs.ContainsKey(npc))
                    foreach (NPC tracked in TrackedNPCs.Keys)
                    {
                        if (npc == tracked)
                        {
                            if (DEBUG_TRACKER) Main.NewText(npc.GivenOrTypeName + " [" + npc.whoAmI + "] flagged for removal");

                            RemoveTrackedNPC(npc);
                            break;
                        }
                        // Look for clashing npcs caused by npc spawning on same slot as defeated npc
                        else if (npc.whoAmI == tracked.whoAmI)
                        {
                            if (DEBUG_TRACKER) Main.NewText("Clash! [" + npc.whoAmI + "] " + npc.GivenOrTypeName + " over " + tracked.GivenOrTypeName);
                            
                            RemoveTrackedNPC(tracked);
                            break;
                        }
                    }
                }

                if(Config.HealthBarFXShake)
                {
                    // Not got it
                    if (TrackedNPCs.ContainsKey(npc) && !TrackedNPCOldLife.ContainsKey(npc))
                    {
                        TrackedNPCOldLife.Add(npc, 0);
                    }
                    // Shouldn't have it
                    else if (!TrackedNPCs.ContainsKey(npc) && TrackedNPCOldLife.ContainsKey(npc))
                    {
                        TrackedNPCOldLife.Remove(npc);
                    }
                }

                if(Config.HealthBarFXChip)
                {
                    // Not got it
                    if (TrackedNPCs.ContainsKey(npc) && !TrackedNPCChipLife.ContainsKey(npc))
                    {
                        TrackedNPCChipLife.Add(npc, 0);
                        TrackedNPCChipTime.Add(npc, 0);
                    }
                    // Shouldn't have it
                    else if (!TrackedNPCs.ContainsKey(npc) && TrackedNPCChipLife.ContainsKey(npc))
                    {
                        TrackedNPCChipLife.Remove(npc);
                        TrackedNPCChipTime.Remove(npc);
                    }
                }
            }

            // Sort the timers
            foreach (NPC tracked in TrackedNPCs.Keys.ToList())
            {
                var hb = BossDisplayInfo.NPCHealthBars[tracked.type];
                bool disableFadeIn = hb.DisableFadeInFor(tracked.type);
                bool disableFadeOut = hb.DisableFadeOutFor(tracked.type);
                // FADE IN
                if (TrackedNPCs[tracked] > 0)
                {
                    // Fade to 0
                    TrackedNPCs[tracked]--;
                    
                    if (disableFadeIn) // No fade in, immediate
                    { TrackedNPCs[tracked] = 0; }
                }
                // FADE OUT
                else if (TrackedNPCs[tracked] <= -1)
                {
                    if (TrackedNPCs[tracked] == -1 ||
                        disableFadeOut) // No fade out, immediate
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

        private static void RemoveTrackedNPC(NPC npc)
        {
            // First check if it's a multi-bar NPC
            HealthBar hb = BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type);
            if (hb != null)
            {
                if (hb.DisplayMode == HealthBar.DisplayType.Multiple && hb.multiShowCount > 1)
                {
                    // Just remove it if there's more than 1 left
                    TrackedNPCs.Remove(npc);
                    return;
                }
            }

            // Make a temp copy that won't be changing
            // Do this to keep info of dead NPCs for a while
            NPC temp = new NPC();
            temp = (NPC)npc.Clone();
            temp.whoAmI = -1 - npc.whoAmI;
            if (temp.life <= 0 || (temp.dontTakeDamage && npc.life == npc.lifeMax)) temp.life = 0;
            TrackedNPCs.Add(temp, -Config.HealthBarUIFadeTime - 1);
            TrackedNPCs.Remove(npc);

            if (DEBUG_TRACKER) Main.NewText(npc.GivenOrTypeName + " [" + npc.whoAmI + "] removed");
        }

        private static float GetAlpha(NPC npc)
        {
            float Alpha = Config.HealthBarUIDefaultAlpha;

            try
            {
                // time will count from X to 0
                float time = TrackedNPCs[npc];
                if (time < 0) time += Config.HealthBarUIFadeTime + 1;
                if (Config.HealthBarUIFadeTime > 0)
                {
                    Alpha = Config.HealthBarUIDefaultAlpha
                        * (1f - (time / Config.HealthBarUIFadeTime));
                }
                if (HealthBar.MouseOver > 0 || 
                    Main.playerInventory ||
                    Main.InReforgeMenu ||
                    Main.InGuideCraftMenu)
                {
                    Alpha = MathHelper.Min(Alpha, Config.HealthBarUIDefaultAlpha * Config.HealthBarUIFadeHover);
                }
                return Alpha;
            }
            catch
            {
                return 0f;
            }
        }
        internal static double GetLifeFillNormal(NPC npc)
        {
            if (Config.HealthBarFXFillUp)
            {
                try
                {
                    // time will count from X to 0
                    float time = TrackedNPCs[npc];
                    if (time >= 0)
                    {
                        double normal = 1d - (double)time / Config.HealthBarUIFadeTime;
                        return normal;
                    }
                }
                catch { }
                // No life found or time < 0
                return 1d;
            }
            else
            {
                // Default behaviour
                return 1d;
            }
        }
        public static void DrawHealthBars(SpriteBatch spriteBatch)
        {

            // Reset static vars such as collective healthbars
            HealthBar.ResetStaticVars();

            // The bottom of thhe screen with some offset
            int maxYStack = (int)(Main.screenHeight * (1f - Config.HealthBarUIMaxStackSize));
            int stackY = Main.screenHeight - Config.HealthBarUIScreenOffset;
            foreach (NPC npc in TrackedNPCs.Keys)
            {
                // Get the healthbar for this tracked NPC
                HealthBar hb = BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type);
                if (hb == null) hb = new HealthBar();
                
                stackY = hb.DrawHealthBarDefault(
                    spriteBatch, GetAlpha(npc), stackY, maxYStack,
                    npc.life, npc.lifeMax, npc);

                if (stackY < maxYStack - Config.HealthBarUIMaxStackSize) break;
            }
        }
    }
}
