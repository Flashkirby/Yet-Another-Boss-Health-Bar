using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    public class MechHealthBar : HealthBar
    {
        internal static Texture2D MechBarL; // Left side of bar frame
        internal static Texture2D MechBarM; // Middle of bar frame (gets stretched)
        internal static Texture2D MechBarR; // Right side of bar frame

        protected override Texture2D GetLeftBar()
        { return MechBarL; }
        protected override Texture2D GetMidBar()
        { return MechBarM; }
        protected override Texture2D GetRightBar()
        { return MechBarR; }

        protected override int GetBossHeadCentreOffsetX() { return 30; }
        protected override int GetBossHeadCentreOffsetY() { return 28; }

        protected override int GetMidBarOffsetY() { return 2; }
        protected override int GetMidBarOffsetX() { return -14; }
    }
}
