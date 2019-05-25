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
    class Missile : BaseEnemy, IEnemy
    {


        public Missile(Texture2D tex, Rectangle rec) : base(tex, rec)
        {
            this.texture = tex;
            this.characterRec = rec;
            speed = 10;


        }



        public override void Move(Rectangle boundsRec, ref int baseSpeed)
        {
            speed = baseSpeed + 3;

            if (isMoving)
            {
                for (int i = 0; i < speed; ++i)
                {
                    if (characterRec.Right > boundsRec.Left)
                    {
                        addToRecX(-1);
                    }
                }
            }
        }

        public override void drawCharacter(SpriteBatch sb)
        {
            sb.Draw(texture, characterRec, Color.Black);
        }
    }
}
