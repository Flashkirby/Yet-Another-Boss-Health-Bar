using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    public class DD2HealthBar : HealthBar
    {
        internal static Texture2D BarL; // Left side of bar frame
        internal static Texture2D BarM; // Middle of bar frame (gets stretched)
        internal static Texture2D BarR; // Right side of bar frame
        internal static Texture2D BarF; // Fill for health bar
        internal static Texture2D BarFsm; // Fill for health bar

        protected override Texture2D GetLeftBar()
        { return BarL; }
        protected override Texture2D GetMidBar()
        { return BarM; }
        protected override Texture2D GetRightBar()
        { return BarR; }

        protected override int GetBossHeadCentreOffsetX() { return 0; }
        protected override int GetBossHeadCentreOffsetY() { return 34; }

        protected override int GetMidBarOffsetY() { return 10; }
        protected override int GetMidBarOffsetX() { return -38; }

        protected override Texture2D GetFillTexture()
        { return BarF; }
        protected override int GetFillDecoOffsetX()
        { return 8; }
        protected override Texture2D GetSmallFillTexture()
        { return BarFsm; }
        protected override int GetSmallFillDecoOffsetX()
        { return 8; }

        protected override Color GetHealthColour(NPC npc, int life, int lifeMax)
        {
            ForceNoChip = true;
            return Color.White;
        }
    }
}
