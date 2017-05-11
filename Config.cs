namespace FKBossHealthBar
{
    public static class Config
    {
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
        /// <summary>Max number screen to allow bars</summary>
        public static float HealthBarUIMaxStackSize = 0.15f;
        /// <summary>Offset from edge of screen</summary>
        public static float HealthBarUIScreenLength = 0.5f;

        /// <summary>How long to fade in/out healthbars</summary>
        public static ushort HealthBarUIFadeTime = 30;
        /// <summary>Alpha to set to when mouse is covering</summary>
        public static float HealthBarUIFadeHover = 0.25f;

        /// <summary>Should the healthbar dramatically fill up on entry</summary>
        public static bool HealthBarFXFillUp = false;
        /// <summary>Should the healthbar shake when depleted</summary>
        public static bool HealthBarFXShake = false;
        /// <summary>Should the healthbar show damage being chipped away</summary>
        public static bool HealthBarFXChip = false;
    }
}
