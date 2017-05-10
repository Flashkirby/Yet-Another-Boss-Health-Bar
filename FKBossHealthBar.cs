using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace FKBossHealthBar
{
    class FKBossHealthBar : Mod
    {
        public FKBossHealthBar()
        {
            Properties = new ModProperties()
            {
                Autoload = true
            };
        }

        public override void Load()
        {
            // Servers don't bother
            if (Main.dedServ) return;

            HealthBar hb;

            #region Invasions
            // Minibosses use small bars
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.GoblinSummoner, hb);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.PirateCaptain, hb);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.Mothron, hb);

            // Pumpkin Moon
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.MourningWood, hb);
            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.Pumpking, hb);

            // Frost Moon
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.MourningWood, hb);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.SantaNK1, hb);
            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.IceQueen, hb);
            #endregion

            // Moon Lord custom example
            hb = new MoonLordPhase1HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.MoonLordHead, hb);
            BossDisplayInfo.SetCustomHealthBar(NPCID.MoonLordHand, hb);
            hb = new MoonLordPhase2HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.MoonLordCore, hb);

            #region DD2
            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2DarkMageT1, hb);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2DarkMageT3, hb);

            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2OgreT2, hb);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2OgreT3, hb);

            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2Betsy, hb);

            hb = new DD2CrystalHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2Crystal, hb);
            #endregion

            HealthBar.Initialise(this);
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            float Alpha = 1f;
            if (HealthBar.MouseOver)
            {
                Alpha = Config.HealthBarUIFadeHover;
                HealthBar.MouseOver = false;
            }
            int stack = 0;
            foreach (NPC npc in Main.npc)
            {
                if (BossDisplayInfo.CanTrackNPCHealth(npc))
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
            /*
            int errorLine = 43;
            try
            {
                // Reset each time
                npcsTracked = new int[Main.npc.Length];

                // Get bar graphics to use
                Texture2D start, middle, end;
                if (Main.expertMode)
                {
                    start = TextureBarStaEXP;
                    middle = TextureBarMedEXP;
                    end = TextureBarEndEXP;
                }
                else
                {
                    start = TextureBarSta;
                    middle = TextureBarMid;
                    end = TextureBarEnd;
                }
                errorLine = 63;
                int barLength = Main.screenWidth / 2;
                int maxStack = Config.HealthBarUIMaxStack;

                int stack = 0;
                // Add NPCs to the list
                foreach (NPC npc in Main.npc)
                {
                    if (stack >= maxStack) break;
                    if (!npc.active)
                    {
                        npcsTracked[npc.whoAmI] = Config.HealthBarUIFadeTime;
                        continue;
                    }
                    errorLine = 74;
                    if (npc.boss)
                    {
                        //count down tracking time
                        if (npcsTracked[npc.whoAmI] < Config.HealthBarUIFadeTime) npcsTracked[npc.whoAmI]++;

                        NPC check = npc;

                        bool noHit = check.immortal || check.dontTakeDamage;
                        if (noHit) continue; // Don't display immune bosses

                        if (check.realLife >= 0)
                        {
                            // Redirect to actual boss core if not done yet
                            if (npcsTracked[check.realLife] <= 0)
                            {
                                check = Main.npc[check.realLife];
                            }
                            else { continue; } // Done already, EXIT
                        }
                        // Track this NPC
                        npcsTracked[check.whoAmI] = 0;
                        stack++;
                    }
                    else
                    {
                        // Reset tracking time
                        npcsTracked[npc.whoAmI] = Config.HealthBarUIFadeTime;
                    }
                }
                errorLine = 102;
                stack = 0;
                // Draw health bars of tracked NPCs
                for (int i = 0; i < npcsTracked.Length; i++)
                {
                    if(npcsTracked[i] < Config.HealthBarUIFadeTime)
                    {
                        float Alpha = 0.5f;// 1f - (float)(npcsTracked[i] / Config.HealthBarUIFadeTime);
                        NPC npc = Main.npc[i];
                        errorLine = 112;
                        BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type).DrawHealthBarDefault(
                            spriteBatch, Alpha, stack,
                            npc.life, npc.lifeMax, npc);
                        //drawHealthBar(spriteBatch, start, middle, end, barLength, 
                        //    Main.npc[i], stack);
                        stack++;
                    }
                }
            }
            catch (System.Exception e)
            {
                Main.NewTextMultiline(e.ToString() + ", line " + errorLine);
            }
        }
        */
    }
}
