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
    public class EnemyDecreaser : BaseCollectible, ICollectible
    {
        public EnemyDecreaser(Texture2D tex, Rectangle rec) : base(tex, rec)
        {
            this.texture = tex;
            this.characterRec = rec;

        }

        public override void OnIntersect(Rectangle boundsRec, ref int valueToChange)
        {
            if (characterRec.Intersects(boundsRec))
            {
                if (valueToChange >= 4)
                {
                    valueToChange -= 3;
                }
            }
        }
    }
}
