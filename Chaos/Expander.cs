﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChaosRunner
{
    public class Expander : BaseCollectible, ICollectible
    {
        public Expander(Texture2D tex, Rectangle rec, Texture2D[] newTexture) : base(tex, rec, newTexture)
        {
            this.texture = tex;
            this.characterRec = rec;

        }

        public override bool OnIntersect(Rectangle boundsRec, ref int valueToChange)
        {
            if (characterRec.Intersects(boundsRec))
            {
                
                    return true;
                
            }

            return false;
        }

        public override void drawCharacter(SpriteBatch sb)
        {
            sb.Draw(texture, characterRec, Color.White);
        }
    }
}
