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

namespace DemoGame
{
    public class Game2 : Microsoft.Xna.Framework.Game
    
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouseStateCurrent;//, mouseStatePrevious;
        KeyboardState keyboardStateCurrent;
        // Sprites
        Sprite playeridledown;
        Sprite playeridleup;
        Sprite playeridleleft;
        Sprite playeridleright;
        Sprite playerwalkup;
        Sprite playerwalkdown;
        Sprite playerwalkleft;
        Sprite playerwalkright;
        Sprite trashcan;
        Sprite trashdemo;
        List<Sprite> trash = new List<Sprite>();
        //end sprites
        Vector2 target;

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
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Random rnd = new Random();
            //loading idle sprites
            playeridledown = new Sprite(Content.Load<Texture2D>("Game2 assets/idle/idle down"), new Vector2((rnd.Next(0, 800)), rnd.Next(0, 600)));
            playeridleup = new Sprite(Content.Load<Texture2D>("Game2 assets/idle/idle up"), new Vector2(playeridledown.Position.X, playeridledown.Position.Y));
            playeridleleft = new Sprite(Content.Load<Texture2D>("Game2 assets/idle/idle left"), new Vector2(playeridledown.Position.X, playeridledown.Position.Y));
            playeridleright = new Sprite(Content.Load<Texture2D>("Game2 assets/idle/idle right"), new Vector2(playeridledown.Position.X, playeridledown.Position.Y));
            //end idle

            //loading walking sprites
            playerwalkup = new Sprite(Content.Load<Texture2D>("Game2 assets/walking/walk up"), new Vector2(playeridleup.Position.X, playeridleup.Position.Y));
            playerwalkdown = new Sprite(Content.Load<Texture2D>("Game2 assets/walking/walk down"), new Vector2(playeridledown.Position.X, playeridledown.Position.Y));
            playerwalkleft = new Sprite(Content.Load<Texture2D>("Game2 assets/walking/walk left"), new Vector2(playeridleleft.Position.X, playeridleleft.Position.Y));
            playerwalkright = new Sprite(Content.Load<Texture2D>("Game2 assets/walking/walk right"), new Vector2(playeridleright.Position.X, playeridleright.Position.Y));
            //end walking
            trashcan = new Sprite(Content.Load<Texture2D>("trashcan"), new Vector2(400, 300));

            
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
         protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //*****************************************************************************
            HandleInput();

            playeridledown.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            playeridleleft.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            playeridleup.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            playeridleright.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            //*****************************************************************************

            //****************************
            base.Update(gameTime);
        }
         private void HandleInput()
         {
             mouseStateCurrent = Mouse.GetState();
             keyboardStateCurrent = Keyboard.GetState();

             

             if (keyboardStateCurrent.IsKeyDown(Keys.Left))
             {
                 playerwalkleft.AnimateSprite(1, 3);
                 target.X -= 2;
                 
             }

             if (keyboardStateCurrent.IsKeyDown(Keys.Right))
             {
                 playerwalkright.AnimateSprite(1, 3);
                 target.X += 2;
             }
             if (keyboardStateCurrent.IsKeyDown(Keys.Down))
             {
                 playerwalkdown.AnimateSprite(1, 3);
                 target.Y += 2;
             }
             if (keyboardStateCurrent.IsKeyDown(Keys.Up))
             {
                 playerwalkup.AnimateSprite(1, 3);
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
    }
}
