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
        #region Large
        public Texture2D fillTexture = null;
        public Texture2D leftBar = null;
        public Texture2D midBar = null;
        public Texture2D rightBar = null;

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
            return leftBar == null ? defaultSta : leftBar;
        }
        protected override Texture2D GetMidBar()
        {
            return midBar == null ? defaultMid : midBar;

        }
        protected override Texture2D GetRightBar()
        {
            return rightBar == null ? defaultEnd : rightBar;
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
        
        public int fillDecoOffsetXSM = 4;
        public int bossHeadCentreOffsetXSM = 14;
        public int bossHeadCentreOffsetYSM = 14;

        protected override Texture2D GetSmallFillTexture()
        {
            return fillTextureSM == null ? defaultFillSM : fillTextureSM;
        }
        protected override Texture2D GetSmallLeftBar()
        {
            return leftBarSM == null ? defaultStaSM : leftBarSM;
        }
        protected override Texture2D GetSmallMidBar()
        {
            return midBar == null ? defaultMidSM : midBar;
        }
        protected override Texture2D GetSmallRightBar()
        {
            return rightBarSM == null ? defaultEndSM : rightBarSM;
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
