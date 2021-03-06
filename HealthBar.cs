﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
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
            Multiple, // Counts all NPCs of a typelist and uses collective life vs lifeMax
            Phase, // Checks for NPCs in the typelist in descending order, using lifeMax as a total.
            Disabled, // Just don't show
        }

        /// <summary>
        /// Global variable for checking if mouse is over bars, in which case fade out
        /// </summary>
        public static int MouseOver = 0;

        /// <summary> Always call the small bar textures when drawing. Typically reserved for minibosses </summary>
        public bool ForceSmall = false;
        /// <summary> Never show the chip bar graphics (numbers are still shown) </summary>
        public bool ForceNoChip = false;
        public bool LoopMidBar = false;
        /*
        /// <summary> Only allow one of these bars to show regardless of how many are active </summary>
        public bool ForceUnique = false;
        */

        /// <summary> Check if the provided bar fill texture has some kind of transparency 
        /// on its right edge, this determines how the damage display bar is drawn. </summary>
        protected bool IsSlanted
        {
            get
            {
                try
                {
                    Texture2D barFill = GetFillTexture();
                    Color[] barColour1D = new Color[barFill.Width * barFill.Height];
                    barFill.GetData(barColour1D);

                    int x = 0;
                    int yStart = -1;
                    int yEnd = barFill.Height - 1;
                    Color c;
                    // Check along left side of texture to see where the bar is
                    for (int y = 0; y < barFill.Height; y++)
                    {
                        c = barColour1D[x + y * barFill.Width];
                        // Look for first left side pixel
                        if (yStart == -1)
                        {
                            if (c.A > 0) // Has a colour, this is the start
                            {
                                yStart = y;
                            }
                        }
                        // Look for last bottom left pixel
                        else
                        {
                            if (c.A == 0) // Has no colour, go back 1 row
                            {
                                yEnd = y - 1;
                                break;
                            }
                        }
                    }

                    x = barFill.Width - 1;
                    // Check endpoints on right side of texture for any transparency
                    c = barColour1D[x + yStart * barFill.Width];
                    if (c.A < 255) { return true; }
                    c = barColour1D[x + yEnd * barFill.Width];
                    if (c.A < 255) { return true; }
                }
                catch // Something went wrong? go to default (false)
                { }
                return false;
            }
        }

        public DisplayType DisplayMode = DisplayType.Standard;

        /// <summary>
        /// All NPC types collected in this health bar, see DisplayType.Multiple
        /// </summary>
        internal int[] multiNPCType = null;
        internal int multiNPCLifeMax = 0;
        internal bool multiNPCLIfeMaxRecordedOnExpert = false;
        /// <summary>
        /// A multi NPC has drawn this bar already? Also used to count.
        /// </summary>
        internal ushort multiShowCount = 0;

        internal static void ResetStaticVars()
        {
            if(MouseOver > 0) MouseOver--;
            // Turn off multishow again for this frame
            foreach(KeyValuePair<int, HealthBar> kvp in BossDisplayInfo.NPCHealthBars)
            {
                kvp.Value.multiShowCount = 0;
                //kvp.Value.drawnUnique = false;
            }
            
        }

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
        internal static Texture2D defaultStaSMEXP; // Expert mode left for small bar
        internal static Texture2D defaultMidSMEXP; // Expert mode middle for small bar
        internal static Texture2D defaultEndSMEXP; // Expert mode right for small bar
        public const int defaultFillEdgeX = 2; // Start X of the bar fill edge
        public const int defaultEndXOffset = 30; // texture inset of bar for ends
        public const int defaultEndYOffset = 10; // texture offset between ends and bar
        public static void Initialise(Mod mod)
        {
            // Back texture
            defaultFill = mod.GetTexture("UI/HealthBarFill");
            defaultFillSM = mod.GetTexture("UI/SmBarFill");
            // Standard health bars
            defaultSta = mod.GetTexture("UI/HealthBarStart");
            defaultMid = mod.GetTexture("UI/HealthBarMiddle");
            defaultEnd = mod.GetTexture("UI/HealthBarEnd");
            // Expert Mode health bars
            defaultStaEXP = mod.GetTexture("UI/HealthBarStart_Exp");
            defaultMidEXP = mod.GetTexture("UI/HealthBarMiddle_Exp");
            defaultEndEXP = mod.GetTexture("UI/HealthBarEnd_Exp");
            // Small health bars
            defaultStaSM = mod.GetTexture("UI/SmBarStart");
            defaultMidSM = mod.GetTexture("UI/SmBarMiddle");
            defaultEndSM = mod.GetTexture("UI/SmBarEnd");
            // Expert Mode small health bars
            defaultStaSMEXP = mod.GetTexture("UI/SmBarStart_Exp");
            defaultMidSMEXP = mod.GetTexture("UI/SmBarMiddle_Exp");
            defaultEndSMEXP = mod.GetTexture("UI/SmBarEnd_Exp");
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
            if (!Main.expertMode)
            { return defaultStaSM; }
            else
            { return defaultStaSMEXP; }
        }
        protected virtual Texture2D GetSmallMidBar()
        {
            if (!Main.expertMode)
            { return defaultMidSM; }
            else
            { return defaultMidSMEXP; }
        }
        protected virtual Texture2D GetSmallRightBar()
        {
            if (!Main.expertMode)
            { return defaultEndSM; }
            else
            { return defaultEndSMEXP; }
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
        protected virtual string GetBossDisplayNameNPC(NPC npc)
        {
            return npc.GivenOrTypeName;
        }

        /// <summary>
        /// The colour of the health bar.
        /// </summary>
        /// <param name="life"></param>
        /// <param name="lifeMax"></param>
        /// <returns></returns>
        protected virtual Color GetHealthColour(NPC npc, int life, int lifeMax)
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

        /// <summary>
        /// Just in case you REALLY want to override standard behaviour and draw this health bar.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="TooFarAway"></param>
        /// <returns>true to show when normally it might not</returns>
        public virtual bool ShowHealthBarOverride(NPC npc, bool TooFarAway)
        {
            return false;
        }

        /// <summary>
        /// Just in case you REALLY want to hide the health bar no matter what. This overrides EVERYTHING else.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="TooFarAway"></param>
        /// <returns>true to hide always</returns>
        public virtual bool HideHealthBarOverride(NPC npc, bool TooFarAway)
        {
            return false;
        }

        /// <summary>
        /// Just in case you want to change up the values displayed for some reason.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="life"></param>
        /// <param name="lifeMax"></param>
        protected virtual void ShowHealthBarLifeOverride(NPC npc, ref int life, ref int lifeMax) { }

        public virtual bool DisableFadeInFor(int type) { return false; }
        public virtual bool DisableFadeOutFor(int type) { return false; }

        /// <summary> Called after a healthbar has been registered, in case you need to initialise some final things. </summary>
        public virtual void OnRegister() { }
        #endregion

        public virtual Texture2D GetBossHeadTextureOrNull(NPC npc)
        {
            int headSlot = GetBossHeadSource(npc).GetBossHeadTextureIndex();

            // No slot, but this is a multi bar?
            if(headSlot < 0 && DisplayMode == DisplayType.Multiple)
            {
                // Search through npcTypes for a head that may match
                foreach(NPC n in Main.npc)
                {
                    foreach (int type in multiNPCType)
                    {
                        if (n.type == type)
                        {
                            headSlot = n.GetBossHeadTextureIndex();
                            if (headSlot > -1) break;
                        }
                    }
                    if (headSlot > -1) break;
                }
            }

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
        /// <returns>Top Y value, for the stack</returns>
        public int DrawHealthBarDefault(SpriteBatch spriteBatch, float Alpha, int stackY, int maxStackY, int life, int lifeMax, NPC npc)
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
            y = stackY;
            // Get the height of the side bars as origin
            y -= midYOffset;
            // Using the centre as reference, add offset per bar based on its postiion in the stack
            y -= (barM.Height + Config.HealthBarUIStackOffset);

            return DrawHealthBar(spriteBatch, x, y, width, Alpha, life, lifeMax, npc);
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
        public int DrawHealthBar(SpriteBatch spriteBatch, int XLeft, int yTop, int BarLength, float Alpha, int life, int lifeMax, NPC npc)
        {
            if (multiShowCount > 0 && 
                (DisplayMode == DisplayType.Multiple || DisplayMode == DisplayType.Phase))
            {
                multiShowCount++;
                return yTop; // We don't show multiple NPC healthbars for a collective boss. ever.
            }

            // Don't draw more than 1 unique per frame
            /*
            if (drawnUnique && ForceUnique) return yTop;
            if (!drawnUnique) drawnUnique = true;
            */

            bool SMALLMODE = Config.SmallHealthBars || ForceSmall;
            string displayName = "";
            ManageMultipleNPCVars(ref life, ref lifeMax, ref displayName);
            ShowHealthBarLifeOverride(npc, ref life, ref lifeMax);
            // Fill up FX
            life = (int)(life * BossBarTracker.GetLifeFillNormal(npc));

            #region calculate shake
            int shakeIntensity = 0;
            if (Config.HealthBarFXShake)
            {
                // Run updates
                if (BossBarTracker.TrackedNPCOldLife.ContainsKey(npc))
                {
                    // Life dropped?
                    if (BossBarTracker.TrackedNPCOldLife[npc] > life)
                    {
                        shakeIntensity = Main.rand.Next(Config.HealthBarFXShakeIntensity) + 1;
                    }
                    BossBarTracker.TrackedNPCOldLife[npc] = life;
                }
            }
            if (Config.HealthBarFXShakeHorizontal) XLeft += shakeIntensity * (Main.rand.Next(2) * 2 - 1);
            shakeIntensity = shakeIntensity * (Main.rand.Next(2) * 2 - 1);
            yTop += shakeIntensity;
            #endregion

            #region calculate chip
            float chipLife = 0f;
            if (Config.HealthBarFXChip)
            {
                if(BossBarTracker.TrackedNPCChipLife.TryGetValue(npc, out chipLife))
                {
                    // Can start chipping condition once damage is taken
                    if (BossBarTracker.TrackedNPCChipLife[npc] > life)
                    {
                        // Wait until it reaches the wait time
                        if (BossBarTracker.TrackedNPCChipTime[npc] < Config.HealthBarFXChipWaitTime)
                        {
                            BossBarTracker.TrackedNPCChipTime[npc]++;
                        }
                        else
                        {
                            BossBarTracker.TrackedNPCChipLife[npc] -= (lifeMax * Config.HealthBarFXChipSpeed * 0.0167f);
                        }
                    }

                    // Limit up again, and reset wait time
                    if (BossBarTracker.TrackedNPCChipLife[npc] < life)
                    {
                        BossBarTracker.TrackedNPCChipLife[npc] = life;
                        BossBarTracker.TrackedNPCChipTime[npc] = 0;
                    }
                }
            }
            #endregion


            // Get variables
            Color frameColour = new Color(1f, 1f, 1f);
            Color blackColour = new Color(0f, 0f, 0f);
            Color barColour = GetHealthColour(npc, life, lifeMax);
            frameColour *= Alpha;
            blackColour *= Alpha * Alpha;
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
            int realLength = drawHealthBarFill(spriteBatch, life, lifeMax, barColour, fill, BarLength, XLeft, midXOffset, midYOffset, yTop, SMALLMODE);
            
            // Draw Chip
            if (Config.HealthBarFXChip && !ForceNoChip)
            {
                int slantOffsetFill = 0;
                if (IsSlanted)
                {
                    slantOffsetFill = midXOffset;
                }
                drawHealthBarFill(spriteBatch, (int)(chipLife) - life, lifeMax, barColour * 0.5f, fill, 
                    BarLength, XLeft + realLength, midXOffset, midYOffset, yTop, SMALLMODE, slantOffsetFill);
            }

            // Draw Frame
            drawHealthBarFrame(spriteBatch, frameColour, barL, barM, barR, BarLength, XLeft, midYOffset, yTop, FrameTopLeft, LoopMidBar);

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

            if(DisplayMode != DisplayType.Multiple)
            {
                displayName = GetBossDisplayNameNPC(npc);
            }

            #region Draw text
            string text = string.Concat(displayName, ": ", life, "/", lifeMax);
            Vector2 position = new Vector2(XLeft + BarLength / 2, 4 + yTop + midYOffset + barM.Height / 2);
            Vector2 origin = ChatManager.GetStringSize(Main.fontMouseText, text, Vector2.One, BarLength) / 2;
            float scale = SMALLMODE ? 0.6f : 1.1f;

            // Draw border
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    DynamicSpriteFontExtensionMethods.DrawString(
                        spriteBatch,
                        Main.fontMouseText,
                        text,
                        position + new Vector2(x, y),
                        blackColour, 0f,
                        origin,
                        scale, SpriteEffects.None, 0f);
                }
            }

            // Main text
            DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                Main.fontMouseText,
                text,
                position,
                frameColour, 0f,
                origin,
                scale, SpriteEffects.None, 0f);
            #endregion

            // Display chip damage number
            if (Config.HealthBarFXChip && Config.HealthBarFXChipNumbers && (int)chipLife - life > 0)
            {
                int chipDamage = (int)chipLife - life;
                DynamicSpriteFontExtensionMethods.DrawString(
                    spriteBatch,
                    Main.fontMouseText,
                    "" + chipDamage,
                    new Vector2(XLeft + BarLength - 8, yTop + midYOffset + barM.Height / 2),
                    frameColour, 0f,
                    ChatManager.GetStringSize(Main.fontMouseText, "" + chipDamage, Vector2.One, BarLength) * new Vector2(1f, 0.5f),
                    SMALLMODE ? 0.6f : 0.8f, SpriteEffects.None, 0f);
            }

            // Check for mouse position
            if (MouseOver < 2)
            {
                if (Main.mouseY > yTop - Config.HealthBarUIScreenOffset - 30 &&
                    Main.mouseY < yTop + barM.Height + midYOffset + 30 + Config.HealthBarUIScreenOffset)
                {
                    if (Main.mouseX > XLeft + midXOffset - 100 &&
                        Main.mouseX < XLeft + BarLength + 100 - midXOffset)
                    {
                        MouseOver = 2;
                    }
                }
            }

            yTop -= shakeIntensity;
            return yTop;
        }

        /// <summary>
        /// Replaces life and life max values to try and collect together multi npc bosses under 1 lifebar
        /// </summary>
        /// <param name="life"></param>
        /// <param name="lifeMax"></param>
        private void ManageMultipleNPCVars(ref int life, ref int lifeMax, ref string displayName)
        {
            if (DisplayMode == DisplayType.Multiple && multiNPCType != null)
            {
                life = 0; lifeMax = 0;

                // Reset when the life max would change (pretty much only during switching to expert mode)
                if (multiNPCLIfeMaxRecordedOnExpert != Main.expertMode)
                {
                    multiNPCLIfeMaxRecordedOnExpert = Main.expertMode;
                    multiNPCLifeMax = 0;
                }

                // First run, only include active
                foreach (int type in multiNPCType)
                {
                    foreach (NPC n in Main.npc)
                    {
                        if (n.type == type)
                        {
                            // Get the names in order of priority
                            if (displayName == "")
                            {
                                displayName = GetBossDisplayNameNPC(n);
                            }
                            if (!n.active) continue;
                            life += n.life;
                            lifeMax += n.lifeMax;
                        }
                    }
                }
                // Get the highest recorded value
                if (multiNPCLifeMax < lifeMax) multiNPCLifeMax = lifeMax;
                lifeMax = multiNPCLifeMax;

                // Set to true to prevent further draws of the same thing this frame (see BossDisplayInfo)
                multiShowCount++;
            }
        }

        private int drawHealthBarFill(SpriteBatch spriteBatch, int life, int lifeMax, Color barColour, Texture2D fill, int barLength, int XLeft, int fillXOffset, int fillYOffset, int yTop, bool SMALLMODE, int insetX = 0)
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
            int realLength = 1 + (int)((barLength - fillXOffset * 2 - 1) * ((float)life / lifeMax)) - insetX;
            if (life <= 0) realLength = 0;
            if (realLength > 0)
            {
                fillXOffset += insetX; // Move bar origin further left by inset
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

            return realLength;
        }

        private static void drawHealthBarFrame(SpriteBatch spriteBatch, Color frameColour, Texture2D barL, Texture2D barM, Texture2D barR, int barLength, int XLeft, int midYOffset, int yTop, Vector2 FrameTopLeft, bool midLoop)
        {
            if (midLoop)
            {
                int XRight = XLeft + barLength;
                // loop draws from the right bar, to the left
                for (int i = 1; i <= (barLength + barM.Width) / barM.Width; i++)
                {
                    spriteBatch.Draw(
                        barM,
                        new Vector2(XRight - barM.Width * i, yTop + midYOffset),
                        null,
                        frameColour,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        0f);
                }
            }
            else
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
            }

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
