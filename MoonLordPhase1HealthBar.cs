﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    public class MoonLordPhase1HealthBar : HealthBar
    {
        public MoonLordPhase1HealthBar()
        {
            // First phase is scythes
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

        protected override Color GetHealthColour(int life, int lifeMax)
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
