using System;
using System.ComponentModel;
using System.Runtime.Serialization;

using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.Config;

using Newtonsoft.Json;

namespace FKBossHealthBar
{
    /// <summary>
    /// Mod Configs for the in-game configuration panel introduced in v0.11
    /// Adapter class to integrate the original Config.cs
    /// </summary>
    public class FKBossHealthBarConfig : ModConfig
    {
        public override bool Autoload(ref string name)
        {
            name = "YetAnotherBHB";
            return true;
        }

        // Client only customisation
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("General Control")]

        [DefaultValue(true)]
        [Label("[i:29] Health Bars Enabled")]
        [Tooltip("Toggles health bar visibility, in case you need to disable it")]
        public bool ShowBossHealthBars
        {
            get { return Config.ShowBossHealthBars; }
            set { Config.ShowBossHealthBars = value; }
        }

        [DefaultValue(false)]
        [Label("[i:58] Small Bars Enabled")]
        [Tooltip("Forces all health bars to use their small variant")]
        public bool SmallHealthBars
        {
            get { return Config.SmallHealthBars; }
            set { Config.SmallHealthBars = value; }
        }

        [DefaultValue(5000)]
        [Range(0, 10000)]
        [Label("[i:395] Maximum Boss Distance [px]")]
        [Tooltip("The maximum distance a boss can be and still show its health bar; 1920px is 120 blocks")]
        public int HealthBarDrawDistance
        {
            get { return Config.HealthBarDrawDistance; }
            set { Config.HealthBarDrawDistance = value; }
        }

        [DefaultValue(false)]
        [Label("[i:560] Slime Rain Bar Enabled")]
        [Tooltip("[Experimental] Show Slime Rain progression as a health bar")]
        public bool SlimeRainBar
        {
            get { return Config.SlimeRainBar; }
            set { Config.SlimeRainBar = value; }
        }


        [Header("UI Tweaks")]

        [DefaultValue(16)]
        [Range(0, 64)]
        [Label("[i:486] Anchor Offset [px]")]
        [Tooltip("Pixel offset from the bottom of the screen")]
        public int HealthBarUIScreenOffset
        {
            get { return Config.HealthBarUIScreenOffset; }
            set { Config.HealthBarUIScreenOffset = value; }
        }

        [DefaultValue(6)]
        [Range(0, 64)]
        [Label("[i:486] Stacked Offset [px]")]
        [Tooltip("Pixel offset between health bars, when multiple are present")]
        public int HealthBarUIStackOffset
        {
            get { return Config.HealthBarUIStackOffset; }
            set { Config.HealthBarUIStackOffset = value; }
        }

        [DefaultValue(100f)]
        [Range(0f, 100f)]
        [Label("[i:38] Opacity [%]")]
        [Tooltip("Opacity of health bars")]
        public float HealthBarUIDefaultAlpha
        {
            get { return Config.HealthBarUIDefaultAlpha * 100f; }
            set { Config.HealthBarUIDefaultAlpha = value / 100f; }
        }

        [DefaultValue(25f)]
        [Range(0f, 100f)]
        [Label("[i:38] Opacity over Cursor [%]")]
        [Tooltip("Opacity of health bars when the cursor is hovering over it")]
        public float HealthBarUIFadeHover
        {
            get { return Config.HealthBarUIFadeHover * 100f; }
            set { Config.HealthBarUIFadeHover = value / 100f; }
        }

        [DefaultValue(15f)]
        [Range(0f, 100f)]
        [Label("[i:2799] Screen Max Height [%]")]
        [Tooltip("Percentage height of the screen that health bars can occupy")]
        public float HealthBarUIMaxStackSize
        {
            get { return Config.HealthBarUIMaxStackSize * 100f; }
            set { Config.HealthBarUIMaxStackSize = value / 100f; }
        }

        [DefaultValue(50f)]
        [Range(0f, 80f)]
        [Label("[i:2799] Screen Width Occupied [%]")]
        [Tooltip("Percentage width of the screen that health bars can occupy")]
        public float HealthBarUIScreenLength
        {
            get { return Config.HealthBarUIScreenLength * 100f; }
            set { Config.HealthBarUIScreenLength = value / 100f; }
        }

        [DefaultValue(0.5f)]
        [Range(0f, 3f)]
        [Label("[i:3099] Fade transition time [s]")]
        [Tooltip("Transition time when a health bar appears/disappears")]
        public float HealthBarUIFadeTimeINT
        {
            get { return Config.HealthBarUIFadeTimeINT / 60f; }
            set { Config.HealthBarUIFadeTimeINT = (int)(value * 60f); }
        }


        [Header("Special FX Options")]

        [DefaultValue(false)]
        [Label("[i:352] Bar Filling Enabled")]
        [Tooltip("Show the health bar filling up as it fades in")]
        public bool HealthBarFXFillUp
        {
            get { return Config.HealthBarFXFillUp; }
            set { Config.HealthBarFXFillUp = value; }
        }

        [DefaultValue(false)]
        [Label("[i:901] Damage Shake Enabled")]
        [Tooltip("WWWW")]
        public bool HealthBarFXShake
        {
            get { return Config.HealthBarFXShake; }
            set { Config.HealthBarFXShake = value; }
        }

        [DefaultValue(2)]
        [Range(0, 10)]
        [Label("[i:901] Damage Shake Intensity [px]")]
        [Tooltip("How much the bar shakes when damage occurs")]
        public int HealthBarFXShakeIntensity
        {
            get { return Config.HealthBarFXShakeIntensity; }
            set { Config.HealthBarFXShakeIntensity = value; }
        }

        [DefaultValue(false)]
        [Label("[i:901] Damage Shake Sideways Enabled")]
        [Tooltip("Shake the bar sideways in addition to up and down")]
        public bool HealthBarFXShakeHorizontal
        {
            get { return Config.HealthBarFXShakeHorizontal; }
            set { Config.HealthBarFXShakeHorizontal = value; }
        }

        [DefaultValue(true)]
        [Label("[i:3119] Chip Damage Enabled")]
        [Tooltip("Show health chipping away in chunks when damage is dealt")]
        public bool HealthBarFXChip
        {
            get { return Config.HealthBarFXChip; }
            set { Config.HealthBarFXChip = value; }
        }

        [DefaultValue(1f)]
        [Range(0f, 10f)]
        [Label("[i:3119] Chip Damage Drain Delay [s]")]
        [Tooltip("Delay after taking damage before chip visual begins to drain")]
        public float HealthBarFXChipWaitTime
        {
            get { return Config.HealthBarFXChipWaitTime / 60f; }
            set { Config.HealthBarFXChipWaitTime = (int)(value * 60f); }
        }

        [DefaultValue(0.5f)]
        [Range(0, 5f)]
        [Label("[i:3119] Chip Damage Drain Speed [%]")]
        [Tooltip("Amont of chip damage drained per frame when draining")]
        public float HealthBarFXChipSpeed
        {
            get { return Config.HealthBarFXChipSpeed; }
            set { Config.HealthBarFXChipSpeed = value; }
        }

        [DefaultValue(false)]
        [Label("[i:3119] Chip Damage Numbers")]
        [Tooltip("Show damage dealt whilst chip damage is being shown")]
        public bool HealthBarFXChipNumbers
        {
            get { return Config.HealthBarFXChipNumbers; }
            set { Config.HealthBarFXChipNumbers = value; }
        }


        [Header("Meta")]

        [JsonIgnore]
        [Label("Version")]
        [Tooltip("The current version of the mod config file. ")]
        public int Version => Config.version;

        [Label("Client Side Only")]
        [Tooltip("This mod is client side only and does not need to be installed by the server. ")]
        public bool ClientSideOnly => true;

        [JsonIgnore]
        [Label("Deprecated Config Notice")]
        [Tooltip("The current version of the mod config file. " +
            "\nThis is located in the 'My Games\\Terraria\\ModLoader\\Mod Configs' folder." +
            "\nDue to compatibility issues, this mod uses both 'YetAnotherBHB.json' and 'FKBossHealthBar_YetAnotherBHB.json'")]
        public bool ConfigNotice => true;
    }
}
