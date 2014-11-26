using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DemoGame
{
    class Sprite
    {
        /*
         * Add Sprite To The Game:
         * 1.   Add new Sprite in "// Sprites" Section.
         * 2.   Add constractor in "LoadContent()" Function.
         * 3.   Add draw in "Draw()" Function.
         * 4.   Add Update Routine.
        */
        Texture2D texture;
        public Vector2 Position;
        public Vector2 Velocity;

        public int rows { get; set; }
        public int columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private bool isAnimated = false;
        private float animateDelta = 0;


        private int width = 1;
        private int height;
        private int currentRow;
        private int currentColumn;
        //Get
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    this.Width,
                    this.Height);
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }
        //!Get

        // Constractors
        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.Position = position;
            setup();
        }

        public Sprite(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture = texture;
            this.Position = position;
            this.Velocity = velocity;
            setup();
        }

        private void setup(){
            width = texture.Width;
            height = texture.Height;
        }
        // !Constractors

        //
        public void AnimateSprite(int rows, int columns)
        {
            this.isAnimated = true;
            this.rows = rows;
            this.columns = columns;
            currentFrame = 0;
            totalFrames = this.rows * this.columns;
            width = texture.Width / columns;
            height = texture.Height / rows;
        }
        //

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isAnimated) animate(spriteBatch); else spriteBatch.Draw(texture, Position, Color.White);
        }
        private void animate(SpriteBatch spriteBatch)
        {
            currentRow = (int)((float)currentFrame / (float)columns);
            currentColumn = currentFrame % columns;


            Rectangle sourceRectangle = new Rectangle(width * currentColumn, height * currentRow, width, height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }

        //!Draw


        //Movement
        public void GoToTarget(Vector2 target)
        {
            float dx = target.X - this.BoundingBox.Center.X;
            float dy = target.Y - this.BoundingBox.Center.Y;

            float t = (float)(Math.Sqrt(dx * dx + dy * dy) / Globals.speed);

            float Vx = dx / t;
            float Vy = dy / t;

            this.Velocity = new Vector2(Vx, Vy);
            if (Math.Abs(dx) < 10 && Math.Abs(dy) < 10)
                this.Velocity = Vector2.Zero;
        }
        
            // Update Position
            public void Update(float time)
            {
                this.Position += this.Velocity * time;
                animateDelta += time;
                if (isAnimated && Globals.animateSpeed <= animateDelta)
                {
                    animateDelta = 0;
                    currentFrame++;
                    if (currentFrame == totalFrames)
                        currentFrame = 0;
                }
            }
            // !Update Position

        //!Movement
    }
}
