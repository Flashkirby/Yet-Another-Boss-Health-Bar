using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    internal class SlimeRainHealthBar : HealthBar
    {
        
        public override bool HideHealthBarOverride(NPC npc, bool TooFarAway)
        {
            ForceSmall = true;

            // Do not show when slimerain is done, no slime king, or underground
            if (Main.slimeRainKillCount <= 0 ||
                !(Main.LocalPlayer.ZoneOverworldHeight || Main.LocalPlayer.ZoneSkyHeight) ||
                NPC.AnyNPCs(NPCID.KingSlime)) return true;

            return false;
        }

        protected override void ShowHealthBarLifeOverride(NPC npc, ref int life, ref int lifeMax)
        {
            lifeMax = 150;
            if (NPC.downedSlimeKing)
            {
                lifeMax /= 2;
            }
            life = lifeMax - Main.slimeRainKillCount;
        }

        protected override Color GetHealthColour(NPC npc, int life, int lifeMax)
        {
            if (npc.color.Equals(default(Color)))
            {
                new Color(0.2f, 0.5f, 1f);
            }
            return npc.color;
        }
    }
}

