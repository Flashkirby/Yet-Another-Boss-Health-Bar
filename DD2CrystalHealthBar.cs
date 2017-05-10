using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace FKBossHealthBar
{
    public class DD2CrystalHealthBar : HealthBar
    {
        public DD2CrystalHealthBar()
        {
            ForceSmall = true;
        }

        protected override Color GetHealthColour(int life, int lifeMax)
        {
            float percent = (float)life / lifeMax;
            float R = 1f, G = 1f;
            if (percent > 0.5f)
            {
                R = 1f - (percent - 0.5f) * 2;
            }
            else
            {
                G = 1f + ((percent - 0.5f) * 2f);
            }
            return new Color(R * 0.75f, G, R);
        }
    }
}
