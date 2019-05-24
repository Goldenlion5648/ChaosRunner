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
    interface ICollectible
    {
        //int MyProperty { get; set; }

        bool OnIntersect(Rectangle boundsRec, ref int valueToChange);
        void animate();


    }
}
