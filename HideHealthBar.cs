using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    public class HideHealthBar : HealthBar
    {
        public override bool HideHealthBarOverride(NPC npc, bool TooFarAway)
        { return true; }
    }
}
