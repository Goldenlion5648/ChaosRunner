using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChaosRunner
{
    public class HealthPack : BaseCollectible, ICollectible
    {
        public HealthPack(Texture2D tex, Rectangle rec, Texture2D[] newTexture) : base(tex, rec, newTexture)
        {
            this.texture = tex;
            this.characterRec = rec;

        }

        public override bool OnIntersect(Rectangle boundsRec, ref int valueToChange)
        {
            if (characterRec.Intersects(boundsRec))
            {
                if (valueToChange < 100)
                {
                    valueToChange += 40;

                    if(valueToChange > 100)
                    {
                        valueToChange = 100;
                    }

                    isOnScreen = false;
                    return true;
                }
            }

            return false;
        }

        public override void drawCharacter(SpriteBatch sb)
        {
            sb.Draw(texture, characterRec, Color.White);
        }
    }
}
