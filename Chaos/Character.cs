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
        public int speed { get; set; }
        public int lengthX { get; set; }
        public int lengthY { get; set; }
        public bool isMoving{ get; set; }

        public Character()
        {
            //texture = Content.Load<Texture2D>("buttonOutline");
        }

        public Character(Texture2D tex, Rectangle rec)
        {
            texture = tex;
            characterRec = rec;
            isMoving = false;
            lengthX = characterRec.Right - characterRec.Left;
            lengthY = characterRec.Bottom - characterRec.Top;
        }
        public void setRec(Rectangle newValue)
        {
            characterRec = newValue;
        }

        public void addToRecWidth(int numToAdd)
        {
            characterRec.Width += numToAdd;
        }
        public void setRecWidth(int newValue)
        {
            characterRec.Width = newValue;
        }
        public virtual void setRecX(int newValue)
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


        public virtual void setRecY(int newValue)
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
        public int getRecY()
        {
            return characterRec.Y;
        }
        public Rectangle getRec()
        {
            return characterRec;
        }
        public int getRecX()
        {
            return characterRec.X;
        }

        public void changeImage(Texture2D newTex)
        {
            texture = newTex;
        }

        public virtual void changeImage()
        {
            //texture = newTex;
        }

        public virtual void drawCharacter(SpriteBatch sb)
        {
            sb.Draw(texture, characterRec, Color.White);
        }

        public void drawCharacter(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, characterRec, color);
        }

    }
}
