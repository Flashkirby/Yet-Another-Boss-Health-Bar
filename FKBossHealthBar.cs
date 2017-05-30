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
        public bool LoadedFKTModSettings = false;

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

            try
            {
                LoadedFKTModSettings = true;
                FKTModSettings.ModSetting setting = FKTModSettings.ModSettingsAPI.CreateModSettingConfig(this);
                setting.AddBool("ShowBossHealthBars", "Enable boss health bars", false);
                setting.AddBool("SmallHealthBars", "Force small health bars", false);

                setting.AddComment("TODO: fancy graphical settings");

                setting.AddComment("Settings below are for tweaking the positioning of the health bars");

                setting.AddInt("HealthBarUIScreenOffset", "Offset from bottom of screen", 0, 100, false);
                setting.AddFloat("HealthBarUIScreenLength", "Bar length:screen", 0f, 1f, false);
                setting.AddInt("HealthBarUIStackOffset", "Distance between bars", 0, 100, false);
                setting.AddFloat("HealthBarUIDefaultAlpha", "Health bar transparency", 0f, 1f, false);
                setting.AddFloat("HealthBarUIMaxStackSize", "Maximum height of bars", 0f, 1f, false);
            }
            catch { }

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
            BossDisplayInfo.SetCustomHealthBar(NPCID.Everscream, hb);
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
            if (LoadedFKTModSettings)
            {
                FKTModSettings.ModSetting setting;
                if (FKTModSettings.ModSettingsAPI.TryGetModSetting(this, out setting))
                {
                    setting.Get("ShowBossHealthBars", ref Config.ShowBossHealthBars);
                    setting.Get("SmallHealthBars", ref Config.SmallHealthBars);

                    setting.Get("HealthBarUIScreenOffset", ref Config.HealthBarUIScreenOffset);
                    setting.Get("HealthBarUIScreenLength", ref Config.HealthBarUIScreenLength);
                    setting.Get("HealthBarUIStackOffset", ref Config.HealthBarUIStackOffset);

                    setting.Get("HealthBarUIDefaultAlpha", ref Config.HealthBarUIDefaultAlpha);
                    setting.Get("HealthBarUIMaxStackSize", ref Config.HealthBarUIMaxStackSize);
                }
            }

            if (!Main.gameInactive) BossBarTracker.UpdateNPCTracker();

            BossBarTracker.DrawHealthBars(spriteBatch);
        }
    }
}
