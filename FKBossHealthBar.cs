using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace FKBossHealthBar
{
    class FKBossHealthBar : Mod
    {
        public FKBossHealthBar()
        {
            Properties = new ModProperties()
            {
                Autoload = true
            };
        }

        public override void Load()
        {
            // Servers don't bother
            if (Main.dedServ) return;

            HealthBar hb;
            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBarMultiple(hb,
                NPCID.EaterofWorldsHead,
                NPCID.EaterofWorldsBody,
                NPCID.EaterofWorldsTail);

            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBarMultiple(hb,
                NPCID.BrainofCthulhu, NPCID.Creeper);

            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBarMultiple(hb,
                NPCID.Golem, NPCID.GolemHead, NPCID.GolemFistLeft, NPCID.GolemFistRight);

            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBarMultiple(hb,
                NPCID.MartianSaucerCore, NPCID.MartianSaucerCannon, NPCID.MartianSaucerTurret);

            #region Invasions
            // Minibosses use small bars
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.GoblinSummoner, hb);
            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBarMultiple(hb,
               NPCID.PirateShip, NPCID.PirateShipCannon);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.PirateCaptain, hb);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.Mothron, hb);

            // Pumpkin Moon
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.MourningWood, hb);
            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.Pumpking, hb);

            // Frost Moon
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.MourningWood, hb);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.SantaNK1, hb);
            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.IceQueen, hb);
            #endregion

            // Pillars
            hb = new CelestialTowerHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.LunarTowerSolar, hb);
            hb = new CelestialTowerHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.LunarTowerVortex, hb);
            hb = new CelestialTowerHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.LunarTowerNebula, hb);
            hb = new CelestialTowerHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.LunarTowerStardust, hb);

            // Moon Lord custom example
            hb = new MoonLordPhase1HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.MoonLordHead, hb);
            BossDisplayInfo.SetCustomHealthBar(NPCID.MoonLordHand, hb);
            hb = new MoonLordPhase2HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.MoonLordCore, hb);

            #region DD2
            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2DarkMageT1, hb);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2DarkMageT3, hb);

            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2OgreT2, hb);
            hb = new HealthBar();
            hb.ForceSmall = true;
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2OgreT3, hb);

            hb = new HealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2Betsy, hb);

            hb = new DD2CrystalHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.DD2Crystal, hb);
            #endregion

            HealthBar.Initialise(this);
        }

        public override void UpdateMusic(ref int music)
        {
            if (Main.gameMenu)
            {
                BossBarTracker.ResetTracker();
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (!Main.gameInactive) BossBarTracker.UpdateNPCTracker();

            BossBarTracker.DrawHealthBars(spriteBatch);
        }
    }
}
