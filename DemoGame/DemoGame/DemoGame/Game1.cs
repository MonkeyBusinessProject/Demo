using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DemoGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouseStateCurrent;//, mouseStatePrevious;
        KeyboardState keyboardStateCurrent;
        // Sprites
        Sprite player;
        List<Sprite> dollars = new List<Sprite>();
        // !Sprites
        Vector2 target;
        //float dx = 0; float dy = 0;
        
        // Actions
        private List<Sprite> hideDollars(int number, List<Sprite> dollars, float minX, float maxX, float minY, float maxY)
        {
            Random rnd = new Random();
            for (int i = 0; i < number; i++)
            {
                dollars.Add(new Sprite(Content.Load<Texture2D>("dollar"), new Vector2(rnd.Next((int)(minX), (int)(maxX)), rnd.Next((int)(minY), (int)(maxY)))));
            }
            return dollars;
        }
        // !Actions

        // Main Functions
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Random rnd = new Random();
            player = new Sprite(Content.Load<Texture2D>("monkeywalk"), new Vector2((rnd.Next(0, 800)),rnd.Next(0,600)));
            player.AnimateSprite(1, 5);

            hideDollars(5, dollars, player.Width, GraphicsDevice.Viewport.Bounds.Width, player.Height, GraphicsDevice.Viewport.Bounds.Height);
            
            target = new Vector2(player.Position.X + (float)(0.5 * player.Width), player.Position.Y + (float)(0.5 * player.Height));
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //*****************************************************************************
            HandleInput();

            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            //*****************************************************************************

            //****************************
            List<Sprite> toRemove = new List<Sprite>();

            foreach (Sprite dollar in dollars)
            {
                if (player.BoundingBox.Intersects(dollar.BoundingBox))
                {
                    Globals.Score += 100;
                    toRemove.Add(dollar);
                }
            }

            if (toRemove.Count > 0)
            {
                foreach (Sprite dollar in toRemove)
                {
                    dollars.Remove(dollar);
                }
            }
            //****************************

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        private void HandleInput()
        {
            mouseStateCurrent = Mouse.GetState();
            keyboardStateCurrent = Keyboard.GetState();

            if (mouseStateCurrent.LeftButton == ButtonState.Pressed)
            {
                target.X = mouseStateCurrent.X;
                target.Y = mouseStateCurrent.Y;
            }

            player.GoToTarget(new Vector2(target.X, target.Y));

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
            if (player.Position.X > 710)
                player.Position = new Vector2(710, player.Position.Y);
            if (player.Position.Y < 0)
                player.Position = new Vector2(player.Position.X, 0);
            if (player.Position.Y > 410)
                player.Position = new Vector2(player.Position.X, 410);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            SpriteFont scoreFont = Content.Load<SpriteFont>("Score"); ;
            spriteBatch.DrawString(scoreFont, Globals.Score.ToString(), new Vector2(10, 10), Color.White);

            player.Draw(spriteBatch);
            foreach(Sprite dollar in dollars){
                dollar.Draw(spriteBatch);
            }
           

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
