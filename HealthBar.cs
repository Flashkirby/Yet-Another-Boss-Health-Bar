using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI.Chat;
using Terraria.ModLoader;

namespace FKBossHealthBar
{
    /// <summary>
    /// Class handles the drawing and effects of the health bar, which is in turn manipulated elsewhere.
    /// One per type of NPC, see BossDisplayInfo
    /// </summary>
    public class HealthBar
    {
        public enum DisplayType
        {
            Standard, // Uses npc.life and npc.realLife
            Multiple, // Counts all NPCs of a type and uses collective life vs lifeMax
            Disabled, // Just don't show
        }

        /// <summary>
        /// Global variable for checking if mouse is over bars, in which case fade out
        /// </summary>
        public static bool MouseOver = false;

        /// <summary>
        /// Always call the small bar textures when drawing. Typically reserved for minibosses
        /// </summary>
        public bool ForceSmall = false;

        public DisplayType DisplayMode = DisplayType.Standard;

        #region Default Textures
        internal static Texture2D defaultFill; // Fill for health bar
        internal static Texture2D defaultSta; // Left side of bar frame
        internal static Texture2D defaultMid; // Middle of bar frame (gets stretched)
        internal static Texture2D defaultEnd; // Right side of bar frame
        internal static Texture2D defaultStaEXP; // Expert mode left
        internal static Texture2D defaultMidEXP; // Expert mode middle
        internal static Texture2D defaultEndEXP; // Expert mode right
        internal static Texture2D defaultFillSM; // Fill for health bar
        internal static Texture2D defaultStaSM; // Left for small bar
        internal static Texture2D defaultMidSM; // Middle for small bar
        internal static Texture2D defaultEndSM; // Right for small bar
        public const int defaultFillEdgeX = 2; // Start X of the bar fill edge
        public const int defaultEndXOffset = 30; // texture inset of bar for ends
        public const int defaultEndYOffset = 10; // texture offset between ends and bar
        public static void Initialise(Mod mod)
        {
            // Back texture
            defaultFill = mod.GetTexture("HealthBarFill");
            defaultFillSM = mod.GetTexture("SmBarFill");
            // Standard health bars
            defaultSta = mod.GetTexture("HealthBarStart");
            defaultMid = mod.GetTexture("HealthBarMiddle");
            defaultEnd = mod.GetTexture("HealthBarEnd");
            // Expert Mode health bars
            defaultStaEXP = mod.GetTexture("HealthBarStart_Exp");
            defaultMidEXP = mod.GetTexture("HealthBarMiddle_Exp");
            defaultEndEXP = mod.GetTexture("HealthBarEnd_Exp");
            // Small health bars
            defaultStaSM = mod.GetTexture("SmBarStart");
            defaultMidSM = mod.GetTexture("SmBarMiddle");
            defaultEndSM = mod.GetTexture("SmBarEnd");
        }
        #endregion
        
        #region Virtual Methods
        protected virtual Texture2D GetFillTexture()
        {
            return defaultFill;
        }
        protected virtual Texture2D GetLeftBar()
        {
            if (!Main.expertMode)
            { return defaultSta; }
            else
            { return defaultStaEXP; }
        }
        protected virtual Texture2D GetMidBar()
        {
            if (!Main.expertMode)
            { return defaultMid; }
            else
            { return defaultMidEXP; }
        }
        protected virtual Texture2D GetRightBar()
        {
            if (!Main.expertMode)
            { return defaultEnd; }
            else
            { return defaultEndEXP; }
        }
        /// <summary>Pixel distance between top of Side bars and top of Centre bar. 
        /// Default 10 is because middle bar must be moved 10 pixels down to match up with the sides.
        /// <para>Small health bars MUST all be the same height, and ignores this. </para></summary>
        protected virtual int GetMidBarOffsetY() { return 10; }
        /// <summary>Pixel distance between right of left side bar and left of centre bar, vice versa for right side.
        /// Default is -30 because the fill texture starts inset into the side bar frames.
        /// <para>Small health bars side frames DO NOT overflow, and ignores this. </para></summary>
        protected virtual int GetMidBarOffsetX() { return -30; }
        /// <summary>
        /// X position of the start of the fill texture decoration. Assumes 1 pixel empty beforehand.
        /// </summary>
        /// <returns></returns>
        protected virtual int GetFillDecoOffsetX() { return 10; }
        protected virtual int GetBossHeadCentreOffsetX() { return 80; }
        protected virtual int GetBossHeadCentreOffsetY() { return 32; }

        protected virtual Texture2D GetSmallFillTexture()
        {
            return defaultFillSM;
        }
        protected virtual Texture2D GetSmallLeftBar()
        {
            return defaultStaSM;
        }
        protected virtual Texture2D GetSmallMidBar()
        {
            return defaultMidSM;
        }
        protected virtual Texture2D GetSmallRightBar()
        {
            return defaultEndSM;
        }
        protected virtual int GetSmallFillDecoOffsetX() { return 4; }
        protected virtual int GetSmallBossHeadCentreOffsetX() { return 14; }
        protected virtual int GetSmallBossHeadCentreOffsetY() { return 14; }

        /// <summary>
        /// The NPC in Main.npc to use as the source of the head icon. 
        /// Most likely Main.npc[NPC.FindFirstNPC( some_npc_type )];
        /// </summary>
        /// <returns></returns>
        protected virtual NPC GetBossHeadSource(NPC npc)
        {
            return npc;
        }
        protected virtual string GetBossDisplayNameNPCType(NPC npc)
        {
            return npc.displayName;
        }

        /// <summary>
        /// The colour of the health bar.
        /// </summary>
        /// <param name="life"></param>
        /// <param name="lifeMax"></param>
        /// <returns></returns>
        protected virtual Color GetHealthColour(int life, int lifeMax)
        {
            float percent = (float)life / lifeMax;
            float R = 1f, G = 1f;
            if (percent > 0.5f)
            {
                R = 1f - (percent - 0.5f) * 2;
            }
            else
            {
                G = 1f + ((percent - 0.5f) * 2f);
            }
            return new Color(R, G, 0f);
        }
        #endregion

        public Texture2D GetBossHeadTextureOrNull(NPC npc)
        {
            int headSlot = GetBossHeadSource(npc).GetBossHeadTextureIndex();
            if (headSlot > -1)
            {
                try
                {
                    return Main.npcHeadBossTexture[headSlot];
                }
                catch { return null; }
            }
            return null;
        }

        /// <summary>
        /// Draw the health bar in a default position, using Config setup and values
        /// </summary>
        /// <param name="spriteBatch">Pass the spritebatch pls</param>
        /// <param name="Alpha">Alpha between 1f and 0f</param>
        /// <param name="stackPosition">Array position of the bar to be stacked.</param>
        /// <param name="life">npc.life</param>
        /// <param name="lifeMax">npc.lifeMax</param>
        /// <param name="npc">npc itself for handling certain internal methods</param>
        public void DrawHealthBarDefault(SpriteBatch spriteBatch, float Alpha, int stackPosition, int life, int lifeMax, NPC npc)
        {
            bool SMALLMODE = Config.SmallHealthBars || ForceSmall;
            Texture2D barM;
            int x, y, width;

            width = (int)(Main.screenWidth * Config.HealthBarUIScreenLength);

            x = Main.screenWidth / 2 - (width / 2);
            
            int midYOffset = 0;
            if (!SMALLMODE) { midYOffset = GetMidBarOffsetY(); }

            if (SMALLMODE)
            { barM = GetSmallMidBar(); }
            else
            { barM = GetMidBar(); }

            // Get the bottom of the screen and offset x pixels
            y = Main.screenHeight - Config.HealthBarUIScreenOffset;
            // Get the height of the side bars as origin
            y -= midYOffset;
            // Using the centre as reference, add offset per bar based on its postiion in the stack
            y -= (barM.Height + Config.HealthBarUIStackOffset) * (stackPosition + 1);

            DrawHealthBar(spriteBatch, x, y, width, Alpha, life, lifeMax, npc);
        }

        /// <summary>
        /// Draw the health bar. Don't forget to override the methods if you want to change settings.
        /// </summary>
        /// <param name="spriteBatch">Pass the spritebatch pls</param>
        /// <param name="XLeft">The left of the bar, NOT the bar side frame.</param>
        /// <param name="yTop">Top of the bar frame (including sides).</param>
        /// <param name="BarLength">Length of the middle section of the bar.</param>
        /// <param name="Alpha">Alpha between 1f and 0f</param>
        /// <param name="life">npc.life</param>
        /// <param name="lifeMax">npc.lifeMax</param>
        /// <param name="npc">npc itself for handling certain internal methods</param>
        public void DrawHealthBar(SpriteBatch spriteBatch, int XLeft, int yTop, int BarLength, float Alpha, int life, int lifeMax, NPC npc)
        {
            bool SMALLMODE = Config.SmallHealthBars || ForceSmall;

            // Get variables
            Color frameColour = new Color(1f, 1f, 1f);
            Color barColour = GetHealthColour(life, lifeMax);
            frameColour *= Alpha;
            barColour *= Alpha;
            Texture2D bossHead = GetBossHeadTextureOrNull(npc);
            Texture2D fill, barL, barM, barR;
            if (SMALLMODE)
            {
                fill = GetSmallFillTexture();
                barL = GetSmallLeftBar();
                barM = GetSmallMidBar();
                barR = GetSmallRightBar();
            }
            else
            {
                fill = GetFillTexture();
                barL = GetLeftBar();
                barM = GetMidBar();
                barR = GetRightBar();
            }

            // Length of bar is set relative to the screen
            // Centre, - bar length, eg. 500 * (1f - 0.4f or 0.6f)

            int midXOffset = 0;
            int midYOffset = 0;
            if (!SMALLMODE)
            {
                midXOffset = GetMidBarOffsetX();
                midYOffset = GetMidBarOffsetY();
            }

            // The very far left where the side frames start
            Vector2 FrameTopLeft = new Vector2(XLeft - barL.Width, yTop);

            // Draw Fill
            drawHealthBarFill(spriteBatch, life, lifeMax, barColour, fill, BarLength, XLeft, midXOffset, midYOffset, yTop, SMALLMODE);

            // Draw Frame
            drawHealthBarFrame(spriteBatch, frameColour, barL, barM, barR, BarLength, XLeft, midYOffset, yTop, FrameTopLeft);

            //Draw NPC Icon
            if (bossHead != null)
            {
                // Pixel align the icons
                int headOffsetX = (bossHead.Width % 4 != 0) ? 1 : 0;
                int headOffsetY = (bossHead.Height % 4 != 0) ? 1 : 0;

                int centreOffsetX, centreOffsetY;
                if (SMALLMODE)
                {
                    centreOffsetX = GetSmallBossHeadCentreOffsetX();
                    centreOffsetY = GetSmallBossHeadCentreOffsetY();
                }
                else
                {
                    centreOffsetX = GetBossHeadCentreOffsetX();
                    centreOffsetY = GetBossHeadCentreOffsetY();
                }
                spriteBatch.Draw(
                    bossHead,
                    FrameTopLeft + new Vector2( // Center head, and use snapping check to keep pixel aligned
                        centreOffsetX - bossHead.Width / 2 - headOffsetX,
                        centreOffsetY - bossHead.Height / 2 - headOffsetY
                        ),
                    frameColour
                    );
            }
            
            string text = string.Concat(GetBossDisplayNameNPCType(npc), ": ", life, "/", lifeMax);
            spriteBatch.DrawString(
                Main.fontMouseText,
                text,
                new Vector2(XLeft + BarLength / 2, yTop + midYOffset + barM.Height / 2),
                frameColour, 0f,
                ChatManager.GetStringSize(Main.fontMouseText, text, Vector2.One, BarLength) / 2,
                SMALLMODE ? 0.6f : 1.1f, SpriteEffects.None, 0f);

            // Check for mouse position
            if(!MouseOver)
            {
                if (Main.mouseY > yTop - Config.HealthBarUIScreenOffset - 30 && 
                    Main.mouseY < yTop + barM.Height + midYOffset + 30 + Config.HealthBarUIScreenOffset)
                {
                    if (Main.mouseX > XLeft + midXOffset - 100 && 
                        Main.mouseX < XLeft + BarLength + 100 - midXOffset)
                    {
                        MouseOver = true;
                    }
                }
            }
        }

        private void drawHealthBarFill(SpriteBatch spriteBatch, int life, int lifeMax, Color barColour, Texture2D fill, int barLength, int XLeft, int fillXOffset, int fillYOffset, int yTop, bool SMALLMODE)
        {
            int decoOffset;
            if (SMALLMODE)
            {
                decoOffset = GetSmallFillDecoOffsetX();
            }else
            {
                decoOffset = GetFillDecoOffsetX();
            }
            int decoWidth = fill.Width - decoOffset;
            // real length is the screen size for bars, plus any extra inset by side frame graphics
            // friendly reminder fillXOffset is usually negative
            int realLength = 1 + (int)((barLength - fillXOffset * 2 - 1) * ((float)life / lifeMax));
            if (realLength > 0)
            {
                // Calculate the scale factor to stretch the featureless side of the bar
                int fillBarLength = realLength - decoWidth;
                if (fillBarLength > 0)
                {
                    float fillXStretch = 1f / (decoOffset - 1) * (realLength - decoWidth);

                    // Draw stretched bar
                    spriteBatch.Draw(
                        fill,
                        new Vector2(XLeft + fillXOffset,
                            yTop + fillYOffset),
                        new Rectangle(0, 0, decoOffset - 1, fill.Height),
                        barColour,
                        0f,
                        Vector2.Zero,
                        new Vector2(fillXStretch, 1f),
                        SpriteEffects.None,
                        0f);
                }

                if (fillBarLength > 0) fillBarLength = 0;
                try
                {
                    // Draw the decoartion side of the bar
                    spriteBatch.Draw(
                        fill,
                        new Vector2(XLeft + fillXOffset + realLength - decoWidth - fillBarLength,
                            yTop + fillYOffset),
                        new Rectangle(decoOffset - fillBarLength, 0, decoWidth + fillBarLength, fill.Height),
                        barColour,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        0f);
                }
                catch { }
            }
        }

        private static void drawHealthBarFrame(SpriteBatch spriteBatch, Color frameColour, Texture2D barL, Texture2D barM, Texture2D barR, int barLength, int XLeft, int midYOffset, int yTop, Vector2 FrameTopLeft)
        {
            spriteBatch.Draw(
                barM,
                new Vector2(XLeft, yTop + midYOffset),
                null,
                frameColour,
                0f,
                Vector2.Zero,
                new Vector2(1f / barM.Width * barLength, 1f),
                SpriteEffects.None,
                0f);
            //Draw side frames
            spriteBatch.Draw(
                barL,
                FrameTopLeft,
                frameColour
                );
            spriteBatch.Draw(
                barR,
                new Vector2(XLeft + barLength, yTop),
                frameColour
                );
        }

    }
}
