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
            

        }

        public void Move()
        {
            throw new NotImplementedException();
        }
    }
}
