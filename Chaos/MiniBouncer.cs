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
    public class MiniBouncer : Bouncer, IEnemy
    {

        private int typeNum = 0;
        //private int speed = 5;
        //private bool isMovingUp = true;

        public int upperBound{ get; set; }
        public int lowerBound{ get; set; }

        public bool hasSetNewPos { get; set; }

        //private int speed;

        //public int speed { get; set; }
        public int MyProperty { get; set; }

        public MiniBouncer(Texture2D tex, Rectangle rec) : base(tex, rec)
        {
            this.texture = tex;
            this.characterRec = rec;
            speed = 5;

            upperBound = characterRec.Top - 70;
            lowerBound = characterRec.Bottom + 70;



        }
        public override void setRecX(int newValue)
        {
            characterRec.X = newValue;
            hasSetNewPos = true;
        }

        public override void setRecY(int newValue)
        {
            characterRec.Y = newValue;
            hasSetNewPos = true;
        }

        public override void Move(Rectangle boundsRec)
        {
            if(hasSetNewPos)
            {
                upperBound = characterRec.Top - 70;
                lowerBound = characterRec.Bottom + 70;
                hasSetNewPos = false;
            }

            if (characterRec.Intersects(boundsRec) && isMoving)
            {
                if (isMovingUp)
                {
                    for (int i = 0; i < speed; i++)
                    {
                        if (characterRec.Bottom < lowerBound)
                        {
                            characterRec.Y += 1;
                        }
                        else
                        {
                            isMovingUp = false;
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < speed; i++)
                    {
                        if (characterRec.Top > upperBound)
                        {
                            characterRec.Y -= 1;
                        }
                        else
                        {
                            isMovingUp = true;
                        }

                    }
                }
            }
        }

        public override void drawCharater(SpriteBatch sb)
        {
            sb.Draw(texture, characterRec, Color.Purple);
        }
    }
}
