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

            // Back texture
            TextureBarFill = GetTexture("HealthBarFill");

            // Standard health bars
            TextureBarSta = GetTexture("HealthBarStart");
            TextureBarMid = GetTexture("HealthBarMiddle");
            TextureBarEnd = GetTexture("HealthBarEnd");

            // Expert Mode health bars
            TextureBarStaEXP = GetTexture("HealthBarStart_Exp");
            TextureBarMedEXP = GetTexture("HealthBarMiddle_Exp");
            TextureBarEndEXP = GetTexture("HealthBarEnd_Exp");
        }

        public int[] npcsTracked = new int[1000];

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
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

                int barLength = Main.screenWidth / 2;
                int maxStack = Main.screenHeight / 6 / middle.Height;

                int stack = 0;
                // Add NPCs to the list
                foreach (NPC npc in Main.npc)
                {
                    if (stack >= maxStack) continue;
                    bool bossHead = npc.GetBossHeadTextureIndex() != -1;
                    if (npc.active && (npc.boss || bossHead))
                    {
                        //count down tracking time
                        if (npcsTracked[npc.whoAmI] > 0) npcsTracked[npc.whoAmI]--;

                        NPC check = npc;

                        bool noHit = check.immortal || check.dontTakeDamage;
                        if (noHit && !bossHead) continue; // Don't display immune bosses except when they have an icon

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
                        npcsTracked[check.whoAmI] = 60;
                        stack++;
                    }
                    else
                    {
                        // Reset tracking time
                        npcsTracked[npc.whoAmI] = 0;
                    }
                }

                stack = 0;
                // Draw health bars of tracked NPCs
                for (int i = 0; i < npcsTracked.Length; i++)
                {
                    if(npcsTracked[i] > 0)
                    {
                        drawHealthBar(spriteBatch, start, middle, end, barLength, 
                            Main.npc[i], stack);
                        stack++;
                    }
                }
            }
            catch (System.Exception e)
            {
                Main.NewTextMultiline(e.ToString());
            }
        }

        private void drawHealthBar(SpriteBatch spriteBatch, Texture2D bar1, Texture2D bar2, Texture2D bar3, int barLength, NPC npc, int stack)
        {
            Color barColour = getHealthColour(npc.life, npc.lifeMax);

            int xOrigin = Main.screenWidth / 4;
            int yTop = Main.screenHeight - endYBotOffset - endYOffset - (bar2.Height + 6) * (stack + 1);

            // Draw Fill
            int fillEndDecoWidth = TextureBarFill.Width - 2;
            float realLength = (barLength + endXOffset * 2) * ((float)npc.life / npc.lifeMax);
            if (realLength > 0f)
            {
                float fillDrawWidthScale = 1f / 2 * realLength;
                spriteBatch.Draw(
                    TextureBarFill,
                    new Vector2(xOrigin - endXOffset, yTop + endYOffset),
                    new Rectangle(0, 0, 2, TextureBarFill.Height),
                    barColour,
                    0f,
                    Vector2.Zero,
                    new Vector2(fillDrawWidthScale, 1f),
                    SpriteEffects.None,
                    0f);

                spriteBatch.Draw(
                    TextureBarFill,
                    new Vector2(xOrigin - endXOffset + realLength - fillEndDecoWidth, yTop + endYOffset),
                    new Rectangle(2, 0, fillEndDecoWidth, TextureBarFill.Height),
                    barColour,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f);
            }

            // Draw stretched center frame
            spriteBatch.Draw(
                bar2,
                new Vector2(xOrigin, yTop + endYOffset),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                new Vector2(1f / bar2.Width * barLength, 1f),
                SpriteEffects.None,
                0f);

            //Draw side frames
            Vector2 TopLeft = new Vector2(xOrigin - bar1.Width, yTop);
            spriteBatch.Draw(
                bar1,
                TopLeft,
                Color.White
                );
            spriteBatch.Draw(
                bar3,
                new Vector2(xOrigin + barLength, yTop),
                Color.White
                );

            //Draw NPC Icon
            int headSlot = npc.GetBossHeadTextureIndex();
            if(headSlot != -1)
            {
                Texture2D npcHead = Main.npcHeadBossTexture[headSlot];
                int headOffsetX = (npcHead.Width % 4 != 0) ? 1 : 0;
                int headOffsetY = (npcHead.Height % 4 != 0) ? 1 : 0;
                spriteBatch.Draw(
                    npcHead,
                    TopLeft + new Vector2( // Center head, and use snapping check to keep pixel aligned
                        80 - npcHead.Width / 2 - headOffsetX,
                        32 - npcHead.Height / 2 - headOffsetY
                        ),
                    Color.White
                    );
            }

            // Draw text
            string text = string.Concat(npc.displayName, ": ", npc.life, "/", npc.lifeMax);
            spriteBatch.DrawString(
                Main.fontMouseText,
                text,
                new Vector2(xOrigin + barLength / 2, yTop + endYOffset + bar2.Height / 2),
                Color.White, 0f,
                ChatManager.GetStringSize(Main.fontMouseText, text, Vector2.One, barLength) / 2,
                1f, SpriteEffects.None, 0f);
        }

        private Color getHealthColour(int life, int lifeMax)
        {
            float percent = (float)life / lifeMax;
            float R = 1f, G = 1f;
            if(percent > 0.5f)
            {
                R = 1f - (percent - 0.5f) * 2;
            }
            else
            {
                G = 1f + ((percent - 0.5f) * 2f);
            }
            return new Color(R, G, 0f);
        }
    }
}
