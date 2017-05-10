using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI.Chat;
using Terraria.ModLoader;

namespace FKBossHealthBar
{
    class FKBossHealthBar : Mod
    {
        public Texture2D TextureBarFill;
        public Texture2D TextureBarSta;
        public Texture2D TextureBarMid;
        public Texture2D TextureBarEnd;
        public Texture2D TextureBarStaEXP;
        public Texture2D TextureBarMedEXP;
        public Texture2D TextureBarEndEXP;

        public const int endXOffset = 30; // texture inset of bar for ends
        public const int endYOffset = 10; // texture offset between ends and bar
        public const int endYBotOffset = 16; // offset from bottom of screen

        public FKBossHealthBar()
        {
            Properties = new ModProperties()
            {
                Autoload = true
            };
        }

        public override void Load()
        {
            // Servers don't load textures
            if (Main.dedServ) return;

            HealthBar.Initialise(this);
        }

        public int[] npcsTracked = new int[1000];

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
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
        

    }
}
