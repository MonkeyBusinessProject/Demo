using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DemoGame
{
    class MovableObject : Sprite
    {
        float counts = -1;


        public MovableObject(Texture2D texture, Vector2 position) : base (texture, position){}

        public MovableObject(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity) { }
        public void Pushed(float gameTime, float pushTime, Vector2 velocity)
        {
            counts = pushTime / gameTime;
            this.Velocity = velocity;
        }

        public void Update(float time)
        {
            this.Position += this.Velocity * time;
            if (counts-- <= 0)
            {
                this.Velocity = Vector2.Zero;
            }
        }
    }
}
