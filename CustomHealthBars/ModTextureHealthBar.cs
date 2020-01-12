using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    /// <summary>
    /// Mod Call health bar with dynamic texture assignment
    /// </summary>
    public class ModTextureHealthBar : HealthBar
    {
        //TODO: fix the expert mode drawing

        #region Large
        public Texture2D fillTexture = null;
        public Texture2D leftBar = null;
        public Texture2D midBar = null;
        public Texture2D rightBar = null;
        public Texture2D leftBarEXP = null;
        public Texture2D midBarEXP = null;
        public Texture2D rightBarEXP = null;

        public int midBarOffsetX = -30;
        public int midBarOffsetY = 10;
        public int fillDecoOffsetX = 10;
        public int bossHeadCentreOffsetX = 80;
        public int bossHeadCentreOffsetY = 32;

        protected override Texture2D GetFillTexture()
        {
            return fillTexture == null ? defaultFill : fillTexture;
        }
        protected override Texture2D GetLeftBar()
        {
            Texture2D fallback = defaultSta, mainTex = leftBar;
            if (Main.expertMode)
            { fallback = leftBar; mainTex = leftBarEXP; if (fallback == null) fallback = defaultStaEXP; }
            return mainTex == null ? fallback : mainTex;
        }
        protected override Texture2D GetMidBar()
        {
            Texture2D fallback = defaultMid, mainTex = midBar;
            if (Main.expertMode)
            { fallback = midBar; mainTex = midBarEXP; if (fallback == null) fallback = defaultMidEXP; }
            return mainTex == null ? fallback : mainTex;
        }
        protected override Texture2D GetRightBar()
        {
            Texture2D fallback = defaultEnd, mainTex = rightBar;
            if (Main.expertMode)
            { fallback = rightBar; mainTex = rightBarEXP; if (fallback == null) fallback = defaultEndEXP; }
            return mainTex == null ? fallback : mainTex;
        }
        protected override int GetMidBarOffsetX() { return midBarOffsetX; }
        protected override int GetMidBarOffsetY() { return midBarOffsetY; }
        protected override int GetFillDecoOffsetX() { return fillDecoOffsetX; }
        protected override int GetBossHeadCentreOffsetX() { return bossHeadCentreOffsetX; }
        protected override int GetBossHeadCentreOffsetY() { return bossHeadCentreOffsetY; }

        #endregion

        #region Small
        public Texture2D fillTextureSM = null;
        public Texture2D leftBarSM = null;
        public Texture2D midBarSM = null;
        public Texture2D rightBarSM = null;
        public Texture2D leftBarSMEXP = null;
        public Texture2D midBarSMEXP = null;
        public Texture2D rightBarSMEXP = null;

        public int fillDecoOffsetXSM = 4;
        public int bossHeadCentreOffsetXSM = 14;
        public int bossHeadCentreOffsetYSM = 14;

        protected override Texture2D GetSmallFillTexture()
        {
            return fillTextureSM == null ? defaultFillSM : fillTextureSM;
        }
        protected override Texture2D GetSmallLeftBar()
        {
            Texture2D fallback = defaultStaSM, mainTex = leftBarSM;
            if (Main.expertMode)
            { fallback = leftBarSM; mainTex = leftBarSMEXP; if (fallback == null) fallback = defaultStaSMEXP; }
            return mainTex == null ? fallback : mainTex;
        }
        protected override Texture2D GetSmallMidBar()
        {
            Texture2D fallback = defaultMidSM, mainTex = midBarSM;
            if (Main.expertMode)
            { fallback = midBarSM; mainTex = midBarSMEXP; if (fallback == null) fallback = defaultMidSMEXP; }
            return mainTex == null ? fallback : mainTex;
        }
        protected override Texture2D GetSmallRightBar()
        {
            Texture2D fallback = defaultEndSM, mainTex = rightBarSM;
            if (Main.expertMode)
            { fallback = rightBarSM; mainTex = rightBarSMEXP; if (fallback == null) fallback = defaultEndSMEXP; }
            return mainTex == null ? fallback : mainTex;
        }
        protected override int GetSmallFillDecoOffsetX() { return fillDecoOffsetXSM; }
        protected override int GetSmallBossHeadCentreOffsetX() { return bossHeadCentreOffsetXSM; }
        protected override int GetSmallBossHeadCentreOffsetY() { return bossHeadCentreOffsetYSM; }

        #endregion

        public string displayName = "";
        protected override string GetBossDisplayNameNPC(NPC npc)
        {
            return displayName == "" ? npc.GivenOrTypeName : displayName;
        }
    }
}
