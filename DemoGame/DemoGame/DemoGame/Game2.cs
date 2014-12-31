#region using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion
namespace DemoGame
{
    public class Game2 : Microsoft.Xna.Framework.Game
        #region fields
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouseStateCurrent;//, mouseStatePrevious;
        KeyboardState keyboardStateCurrent;
        // Sprites
        Sprite player;
        Sprite trashcan;
        Sprite trashdemo;
        List<MovableObject> trashes = new List<MovableObject>();
        //end sprites
        Vector2 target;

        Random rnd;
    #endregion
        public Game2()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();

        }
        #region content loading
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            rnd = new Random();
            //loading idle sprites
            player = new Sprite(Content.Load<Texture2D>("Game2 assets/idle/idle down"), new Vector2((rnd.Next(0, 800)), rnd.Next(0, 600)));
            
            //
            CreateTrash(10, player.Width, GraphicsDevice.Viewport.Bounds.Width, player.Height, GraphicsDevice.Viewport.Bounds.Height);
            
            trashcan = new Sprite(Content.Load<Texture2D>("Game2 assets/trashcan"), new Vector2(400, 300));
            target = new Vector2(player.Position.X + (float)(0.5 * player.Width), player.Position.Y + (float)(0.5 * player.Height));
            
        }
        #endregion
        #region trash creation
        private void CreateTrash(int number, float minX, float maxX, float minY, float maxY)
        {
            for (int i = 0; i < number; i++)
            {
                trashes.Add(new MovableObject(Content.Load<Texture2D>("Game2 assets/trashdemo"), new Vector2(rnd.Next((int)(minX), (int)(maxX)), rnd.Next((int)(minY), (int)(maxY)))));
            }
        }
        #endregion
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #region update method
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //*****************************************************************************
            HandleInput();
            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            player.GoToTarget(target);
            //*****************************************************************************
            #region collision management
            List<Sprite> toRemove = new List<Sprite>();
            
             foreach (MovableObject trash in trashes)
            {
                if (player.BoundingBox.Intersects(trash.BoundingBox))
                {
                    trash.Pushed((float)gameTime.ElapsedGameTime.TotalSeconds, (float)0.5, player.Velocity * 2);
                }

                if(trashcan.BoundingBox.Intersects(trash.BoundingBox))
                {
                    Globals.Score += 100;
                    toRemove.Add(trash);
                }
                trash.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            }
            #endregion 
             #region trash to trashcan
             if (toRemove.Count > 0)
            {
                foreach (MovableObject trash in toRemove)
                {
                    trashes.Remove(trash);
                }
            }

             #endregion
             base.Update(gameTime);
        }
        #endregion
        #region handle method
        private void HandleInput()
         {
             mouseStateCurrent = Mouse.GetState();
             keyboardStateCurrent = Keyboard.GetState();

             

             if (keyboardStateCurrent.IsKeyDown(Keys.Left))
             {
                
                 target.X -= 2;
             }

             if (keyboardStateCurrent.IsKeyDown(Keys.Right))
             {
                 
                 target.X += 2;
             }
             if (keyboardStateCurrent.IsKeyDown(Keys.Down))
             {
               
                 target.Y += 2;
             }
             if (keyboardStateCurrent.IsKeyDown(Keys.Up))
             {
               
                 target.Y -= 2;
             }
             if (player.Position.X < 0)
                 player.Position = new Vector2(0, player.Position.Y);
             if (player.Position.X > 767)
                 player.Position = new Vector2(767, player.Position.Y);
             if (player.Position.Y < 0)
                 player.Position = new Vector2(player.Position.X, 0);
             if (player.Position.Y > 444)
                 player.Position = new Vector2(player.Position.X, 444);
#endregion
         }
        #region draw method
         protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            SpriteFont scoreFont = Content.Load<SpriteFont>("Score"); ;
            spriteBatch.DrawString(scoreFont, Globals.Score.ToString(), new Vector2(10, 10), Color.White);

            player.Draw(spriteBatch);
            trashcan.Draw(spriteBatch);
            spriteBatch.DrawString(scoreFont, Globals.Score.ToString(), new Vector2(10, 10), Color.White);
            foreach (MovableObject trash in trashes)
            {
                trash.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
#endregion
    }
}
