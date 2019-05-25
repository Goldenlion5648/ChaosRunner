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
    public class BaseEnemy : Character, IEnemy
    {


        public BaseEnemy(Texture2D tex, Rectangle rec) : base(tex, rec)
        {
            this.texture = tex;
            this.characterRec = rec;
            this.isMoving = false;


        }


        public bool IsCollidingTopOrBottom(Rectangle boundsRec)
        {
            bool isColliding = false;
            if(this.characterRec.Y <= boundsRec.Top)
            {
                isColliding = true;

            }

            return isColliding;

        }

        public virtual void Move(Rectangle boundsRec, ref int baseSpeed)
        {
            //characterRec.X -= 10;
        }
    }
}
