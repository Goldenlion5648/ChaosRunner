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
    public class Character
    {
        public Texture2D texture;
        public Rectangle characterRec;

        public Character()
        {

        }

        public Character(Texture2D tex, Rectangle rec)
        {
            texture = tex;
            characterRec = rec;
        }
        public void setRec(Rectangle newValue)
        {
            characterRec = newValue;
        }
        public void setRecX(int newValue)
        {
            characterRec.X = newValue;
        }
        public void addToRecX(int newValue)
        {
            characterRec.X += newValue;
        }
        public void addToRecX()
        {
            characterRec.X += 1;
        }

        public void setRecY(int newValue)
        {
            characterRec.Y = newValue;
        }
        public void addToRecY(int newValue)
        {
            characterRec.Y += newValue;
        }
        public void addToRecY()
        {
            characterRec.Y += 1;
        }
        public Rectangle getRec()
        {
            return characterRec;
        }
        public int getRecX()
        {
            return characterRec.X;
        }

        public virtual void drawCharater(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(texture, characterRec, Color.White);
            sb.End();
        }

        public void drawCharater(SpriteBatch sb, Color color)
        {
            sb.Begin();
            sb.Draw(texture, characterRec, color);
            sb.End();
        }

    }
}
