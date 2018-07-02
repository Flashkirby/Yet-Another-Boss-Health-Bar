using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    internal class MoonLordPhase1HealthBar : HealthBar
    {
        public MoonLordPhase1HealthBar()
        {
            DisplayMode = DisplayType.Multiple;
            ForceSmall = true;
        }

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
            float B = 1f, G = 1f;
            if (percent > 0.5f)
            {
                B = 1f - (percent - 0.5f) * 2;
            }
            else
            {
                G = 1f + ((percent - 0.5f) * 2f);
            }
            return new Color(0f, G, B);
        }
    }
}
