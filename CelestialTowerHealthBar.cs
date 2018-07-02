using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    internal class CelestialTowerHealthBar : HealthBar
    {
        public override bool ShowHealthBarOverride(NPC npc, bool TooFarAway)
        {
            if (TooFarAway) return false;

            // Show the bar for the NPC
            if (npc.type == NPCID.LunarTowerSolar)
            {
                return NPC.ShieldStrengthTowerSolar > 0;
            }
            else if (npc.type == NPCID.LunarTowerVortex)
            {
                return NPC.ShieldStrengthTowerVortex > 0;
            }
            else if (npc.type == NPCID.LunarTowerNebula)
            {
                return NPC.ShieldStrengthTowerNebula > 0;
            }
            else if (npc.type == NPCID.LunarTowerStardust)
            {
                return NPC.ShieldStrengthTowerStardust > 0;
            }
            return false;
        }

        protected override void ShowHealthBarLifeOverride(NPC npc, ref int life, ref int lifeMax)
        {
            bool isShieldForm = false;
            // Show the bar as shield
            if (npc.type == NPCID.LunarTowerSolar && NPC.ShieldStrengthTowerSolar > 0)
            {
                life = NPC.ShieldStrengthTowerSolar;
                isShieldForm = true;
            }
            else if (npc.type == NPCID.LunarTowerVortex && NPC.ShieldStrengthTowerVortex > 0)
            {
                life = NPC.ShieldStrengthTowerVortex;
                isShieldForm = true;
            }
            else if (npc.type == NPCID.LunarTowerNebula && NPC.ShieldStrengthTowerNebula > 0)
            {
                life = NPC.ShieldStrengthTowerNebula;
                isShieldForm = true;
            }
            else if (npc.type == NPCID.LunarTowerStardust && NPC.ShieldStrengthTowerStardust > 0)
            {
                life = NPC.ShieldStrengthTowerStardust;
                isShieldForm = true;
            }

            ForceSmall = isShieldForm;
            if (isShieldForm)
            {
                lifeMax = Main.expertMode ? NPC.LunarShieldPowerExpert : NPC.LunarShieldPowerNormal;
            }
        }

        protected override Color GetHealthColour(NPC npc, int life, int lifeMax)
        {
            // Is in shield mode?
            if(lifeMax == NPC.LunarShieldPowerNormal || lifeMax == NPC.LunarShieldPowerExpert)
            {
                return new Color(1f, 1f, 1f);
            }
            return base.GetHealthColour(npc, life, lifeMax);
        }
    }
}

