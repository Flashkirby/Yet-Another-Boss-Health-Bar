﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKBossHealthBar
{
    public class ModSimpleHealthBarType1 : ModTextureHealthBar
    {
        public Color barColourFull = new Color(0f, 1f, 0f);
        public Color barColourMid = new Color(1f, 1f, 0f);
        public Color barColourEmpty = new Color(1f, 0f, 0f);
        public Texture2D customBossHeadIcon = null;

        protected override Color GetHealthColour(NPC npc, int life, int lifeMax)
        {
            float percent = (float)life / lifeMax;
            Color c = new Color();
            if (percent > 0.5f)
            {
                return LerpColour(barColourMid, barColourFull, (percent - 0.5f) * 2);
            }
            else
            {
                return LerpColour(barColourEmpty, barColourMid, percent * 2);
            }
        }
        private Color LerpColour(Color c1, Color c2, float amount)
        {
            return new Color(
                MathHelper.Lerp(c1.R / 255f, c2.R / 255f, amount),
                MathHelper.Lerp(c1.G / 255f, c2.G / 255f, amount),
                MathHelper.Lerp(c1.B / 255f, c2.B / 255f, amount));
        }

        public override Texture2D GetBossHeadTextureOrNull(NPC npc)
        {
            return customBossHeadIcon != null ? customBossHeadIcon : base.GetBossHeadTextureOrNull(npc);
        }
    }
}
