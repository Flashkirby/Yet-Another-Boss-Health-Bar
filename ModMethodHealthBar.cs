using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace FKBossHealthBar
{
    /// <summary>
    /// Mod Call health bar with function based texture assignment
    /// </summary>
    public class ModMethodHealthBar : HealthBar
    {
        public Func<Texture2D> getFillTexture;
        protected override Texture2D GetFillTexture()
        { return getFillTexture == null ? base.GetFillTexture() : getFillTexture(); }

        public Func<Texture2D> getLeftBar;
        protected override Texture2D GetLeftBar()
        { return getLeftBar == null ? base.GetLeftBar() : getLeftBar(); }

        public Func<Texture2D> getMidBar;
        protected override Texture2D GetMidBar()
        { return getMidBar == null ? base.GetMidBar() : getMidBar(); }

        public Func<Texture2D> getRightBar;
        protected override Texture2D GetRightBar()
        { return getRightBar == null ? base.GetRightBar() : getRightBar(); }


        public Func<Texture2D> getSmallFillTexture;
        protected override Texture2D GetSmallFillTexture()
        { return getSmallFillTexture == null ? base.GetSmallFillTexture() : getSmallFillTexture(); }

        public Func<Texture2D> getSmallLeftBar;
        protected override Texture2D GetSmallLeftBar()
        { return getSmallLeftBar == null ? base.GetSmallLeftBar() : getSmallLeftBar(); }

        public Func<Texture2D> getSmallMidBar;
        protected override Texture2D GetSmallMidBar()
        { return getSmallMidBar == null ? base.GetSmallMidBar() : getSmallMidBar(); }

        public Func<Texture2D> getSmallRightBar;
        protected override Texture2D GetSmallRightBar()
        { return getSmallRightBar == null ? base.GetSmallRightBar() : getSmallRightBar(); }

        #region Deco
        public int midBarOffsetX = -30;
        public int midBarOffsetY = 10;
        public int fillDecoOffsetX = 10;
        public int bossHeadCentreOffsetX = 80;
        public int bossHeadCentreOffsetY = 32;
        protected override int GetMidBarOffsetX() { return midBarOffsetX; }
        protected override int GetMidBarOffsetY() { return midBarOffsetY; }
        protected override int GetFillDecoOffsetX() { return fillDecoOffsetX; }
        protected override int GetBossHeadCentreOffsetX() { return bossHeadCentreOffsetX; }
        protected override int GetBossHeadCentreOffsetY() { return bossHeadCentreOffsetY; }

        public int fillDecoOffsetXSM = 4;
        public int bossHeadCentreOffsetXSM = 14;
        public int bossHeadCentreOffsetYSM = 14;
        protected override int GetSmallFillDecoOffsetX() { return fillDecoOffsetXSM; }
        protected override int GetSmallBossHeadCentreOffsetX() { return bossHeadCentreOffsetXSM; }
        protected override int GetSmallBossHeadCentreOffsetY() { return bossHeadCentreOffsetYSM; }
        #endregion

        public Func<NPC, string> getBossDisplayNameNPC;
        protected override string GetBossDisplayNameNPC(NPC npc)
        { return getBossDisplayNameNPC == null ? base.GetBossDisplayNameNPC(npc) : getBossDisplayNameNPC(npc); }

        public Func<NPC, int, int, Color> getHealthColour;
        protected override Color GetHealthColour(NPC npc, int life, int lifeMax)
        { return getHealthColour == null ? base.GetHealthColour(npc, life, lifeMax) : getHealthColour(npc, life, lifeMax); }
    }
}
