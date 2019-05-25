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

        public int topBound{ get; set; }
        public int bottomBound{ get; set; }

        public bool hasSetNewPos { get; set; }

        //private int speed;

        //public int speed { get; set; }
        public int MyProperty { get; set; }

        public MiniBouncer(Texture2D tex, Rectangle rec) : base(tex, rec)
        {
            this.texture = tex;
            this.characterRec = rec;
            speed = 5;

            topBound = characterRec.Top - 70;
            bottomBound = characterRec.Bottom + 70;



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

        public override void Move(Rectangle boundsRec, ref int baseSpeed)
        {

            speed = baseSpeed - 2;
            if(hasSetNewPos)
            {
                topBound = characterRec.Top - 70;
                bottomBound = characterRec.Bottom + 70;
                hasSetNewPos = false;
            }

            if (characterRec.Intersects(boundsRec) && isMoving)
            {
                if (isMovingUp)
                {
                    for (int i = 0; i < speed; i++)
                    {
                        if (characterRec.Bottom < bottomBound)
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
                        if (characterRec.Top > topBound)
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

        public override void drawCharacter(SpriteBatch sb)
        {
            sb.Draw(texture, characterRec, Color.Purple);
        }
    }
}
