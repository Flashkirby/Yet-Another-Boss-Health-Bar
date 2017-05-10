using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKBossHealthBar
{
    public static class Config
    {
        /// <summary>Show health bars, in case you want to disable it...</summary>
        public static bool ShowBossHealthBars = true;
        /// <summary>Show small health bars</summary>
        public static bool SmallHealthBars = false;
        /// <summary>Offset from edge of screen</summary>
        public static int HealthBarUIScreenOffset = 16;
        /// <summary>Y Distance between bars</summary>
        public static int HealthBarUIStackOffset = 6;
        /// <summary>Max number of bars to show</summary>
        public static int HealthBarUIMaxStack = 3;
        /// <summary>How long to fade in/out healthbars</summary>
        public static int HealthBarUIFadeTime = 60;
        /// <summary>Factor of fade time to drop to when mouse is covering</summary>
        public static float HealthBarUIFadeHover = 0.75f;
        /// <summary>Offset from edge of screen</summary>
        public static float HealthBarUIScreenLength = 0.5f;
    }
}
