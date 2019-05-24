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
    public class BaseCollectible : Character, ICollectible
    {
        public int totalFrames { get; set; }
        public int currentFrame { get; set; }
        public Texture2D[] texturesArray{ get; set; }

        public BaseCollectible(Texture2D tex, Rectangle rec, Texture2D[] newTextures) : base(tex, rec)
        {
            this.texture = tex;
            this.characterRec = rec;
            this.isMoving = false;
            texturesArray = newTextures;


        }

        public virtual void OnIntersect(Rectangle boundsRec, ref int valueToChange)
        {

        }

        public override void changeImage()
        {
            //currentFrame=
        }

        public void animate()
        {
            if(currentFrame < totalFrames - 1)
            {
                currentFrame++;
            }
            else
            {
                currentFrame = 0;
            }
        }


    }
}
