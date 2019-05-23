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
    class Bouncer : BaseEnemy, IEnemy
    {

        private int typeNum = 0;
        //private int speed = 5;
        //private bool isMovingUp = true;

        public bool isMovingUp { get; set; }
        //private int speed;

        //public int speed { get; set; }

        public Bouncer(Texture2D tex, Rectangle rec) : base(tex, rec)
        {
            this.texture = tex;
            this.characterRec = rec;
            speed = 5;


        }

        public override void Move(Rectangle boundsRec)
        {
            if (isMovingUp)
            {
                for (int i = 0; i < speed; i++)
                {
                    if (characterRec.Bottom < boundsRec.Bottom)
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
                    if (characterRec.Y > boundsRec.Top)
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

        public override void drawCharater(SpriteBatch sb)
        {
            sb.Draw(texture, characterRec, Color.Green);
        }
    }
}
