using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    internal class MoonLordPhase2HealthBar : HealthBar
    {
        protected override NPC GetBossHeadSource(NPC npc)
        {
            int id = NPC.FindFirstNPC(NPCID.MoonLordHead);
            if (id > 0)
            { return Main.npc[id]; }
            return base.GetBossHeadSource(npc);
        }

        protected override Color GetHealthColour(NPC npc, int life, int lifeMax)
        {
            float percent = (float)life / lifeMax;
            float R = 1f, B = 1f;
            if (percent > 0.5f)
            {
                R = 1f - (percent - 0.5f) * 2;
            }
            else
            {
                B = 1f + ((percent - 0.5f) * 2f);
            }
            return new Color(R, 0f, B);
        }
    }
}
