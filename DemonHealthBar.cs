using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    public class DemonHealthBar : HealthBar
    {
        internal static Texture2D DemonBarL; // Left side of bar frame
        internal static Texture2D DemonBarM; // Middle of bar frame (gets stretched)
        internal static Texture2D DemonBarR; // Right side of bar frame

        protected override Texture2D GetLeftBar()
        { return DemonBarL; }
        protected override Texture2D GetMidBar()
        { return DemonBarM; }
        protected override Texture2D GetRightBar()
        { return DemonBarR; }

        protected override int GetBossHeadCentreOffsetX() { return 56; }
        protected override int GetBossHeadCentreOffsetY() { return 30; }

        protected override int GetMidBarOffsetX() { return -4; }
    }
}
