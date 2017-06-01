using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.UI.Chat;

namespace FKBossHealthBar
{
    public static class Config
    {
        //The file will be stored in "Terraria/ModLoader/Mod Configs/Example Mod.json"
        private static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", "YetAnotherBHB.json");
        private static Preferences config;
        private static int version = 1;
        public static void LoadConfig()
        {
            // Shamelessly 'borrowed' from WMITF, ty goldenapple
            // https://forums.terraria.org/index.php?threads/modders-guide-to-config-files-and-optional-features.48581/
            config = new Preferences(ConfigPath);
            config.AutoSave = true;
            if (config.Load())
            {
                // Set these values when successfully loaded
                config.Get("version", ref version);
                config.Get("ShowBossHealthBars", ref ShowBossHealthBars);
                config.Get("SmallHealthBars", ref SmallHealthBars);
                config.Get("HealthBarDrawDistance", ref HealthBarDrawDistance);
                config.Get("HealthBarUIScreenOffset", ref HealthBarUIScreenOffset);
                config.Get("HealthBarUIStackOffset", ref HealthBarUIStackOffset);
                config.Get("HealthBarUIDefaultAlpha", ref HealthBarUIDefaultAlpha);
                config.Get("HealthBarUIMaxStackSize", ref HealthBarUIMaxStackSize);
                config.Get("HealthBarUIScreenLength", ref HealthBarUIScreenLength);
                config.Get("HealthBarUIFadeTime", ref HealthBarUIFadeTimeINT);
                config.Get("HealthBarUIFadeHover", ref HealthBarUIFadeHover);
                config.Get("HealthBarFXFillUp", ref HealthBarFXFillUp);
                config.Get("HealthBarFXShake", ref HealthBarFXShake);
                config.Get("HealthBarFXShakeIntensity", ref HealthBarFXShakeIntensity);
                config.Get("HealthBarFXChip", ref HealthBarFXChip);
                config.Get("HealthBarFXChipWaitTime", ref HealthBarFXChipWaitTime);
                config.Get("HealthBarFXChipSpeed", ref HealthBarFXChipSpeed);
                config.Get("HealthBarFXChipNumbers", ref HealthBarFXChipNumbers);
            }
            else
            {
                // Put in these values if new
                config.Put("version", version);
                config.Put("ShowBossHealthBars", ShowBossHealthBars);
                config.Put("SmallHealthBars", SmallHealthBars);
                config.Put("HealthBarDrawDistance", HealthBarDrawDistance);
                config.Put("HealthBarUIScreenOffset", HealthBarUIScreenOffset);
                config.Put("HealthBarUIStackOffset", HealthBarUIStackOffset);
                config.Put("HealthBarUIDefaultAlpha", HealthBarUIDefaultAlpha);
                config.Put("HealthBarUIMaxStackSize", HealthBarUIMaxStackSize);
                config.Put("HealthBarUIScreenLength", HealthBarUIScreenLength);
                config.Put("HealthBarUIFadeTime", HealthBarUIFadeTime);
                config.Put("HealthBarUIFadeHover", HealthBarUIFadeHover);
                config.Put("HealthBarFXFillUp", HealthBarFXFillUp);
                config.Put("HealthBarFXShake", HealthBarFXShake);
                config.Put("HealthBarFXShakeIntensity", HealthBarFXShakeIntensity);
                config.Put("HealthBarFXChip", HealthBarFXChip);
                config.Put("HealthBarFXChipWaitTime", HealthBarFXChipWaitTime);
                config.Put("HealthBarFXChipSpeed", HealthBarFXChipSpeed);
                config.Put("HealthBarFXChipNumbers", HealthBarFXChipNumbers);
                config.Save();
            }
        }

        /// <summary>Show health bars, in case you want to disable it...</summary>
        public static bool ShowBossHealthBars = true;
        /// <summary>Show small health bars</summary>
        public static bool SmallHealthBars = false;

        /// <summary>Distance past which boss health bars are typically ignored</summary>
        public static int HealthBarDrawDistance = 5000;
        /// <summary>Offset from edge of screen</summary>
        public static int HealthBarUIScreenOffset = 16;
        /// <summary>Y Distance between bars</summary>
        public static int HealthBarUIStackOffset = 6;
        /// <summary>Default bar alpha</summary>
        public static float HealthBarUIDefaultAlpha = 1f;
        /// <summary>Max number screen to allow bars</summary>
        public static float HealthBarUIMaxStackSize = 0.15f;
        /// <summary>Offset from edge of screen</summary>
        public static float HealthBarUIScreenLength = 0.5f;

        /// <summary>How long to fade in/out healthbars</summary>
        public static ushort HealthBarUIFadeTime
        {
            get { return (ushort)HealthBarUIFadeTimeINT; }
            set { HealthBarUIFadeTimeINT = value; }
        }
        internal static int HealthBarUIFadeTimeINT = 30; // not the used value, but here for modsettings support
        /// <summary>Alpha to set to when mouse is covering</summary>
        public static float HealthBarUIFadeHover = 0.25f;

        /// <summary>Should the healthbar dramatically fill up on entry (tied to alpha)</summary>
        public static bool HealthBarFXFillUp = false;
        /// <summary>Should the healthbar shake when depleted</summary>
        public static bool HealthBarFXShake = false;
        /// <summary>Pixel shake for shake effect</summary>
        public static int HealthBarFXShakeIntensity = 3;
        /// <summary>Should the healthbar show damage being chipped away LIKE DARK SOULS</summary>
        public static bool HealthBarFXChip = false;
        /// <summary>Wait before starting the chip drain</summary>
        public static int HealthBarFXChipWaitTime = 60;
        /// <summary>Percent speed for chip draining speed</summary>
        public static float HealthBarFXChipSpeed = 0.2f;
        /// <summary>Should health bar chipping also display damage numbers A BIT LIKE DARK SOULS</summary>
        public static bool HealthBarFXChipNumbers = false;
    }
}
