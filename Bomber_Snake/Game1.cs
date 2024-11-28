using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bomber_Snake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Vector2 snakePos = Vector2.Zero;

        List<Snake> snakeParts;
        List<Food> foodList;

        Texture2D testRender;
        RenderTarget2D playArea;

        KeyboardState kb_curr, kb_old;

        GameSettings settings;

#if DEBUG
        public static SpriteFont debugFont;
        public static Texture2D debugPixel;
#endif

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.SynchronizeWithVerticalRetrace = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
            IsFixedTimeStep = true;

            settings = new GameSettings(_graphics);

            settings.WindowSize.Add(new Point(800, 800));
            settings.WindowSize.Add(new Point(1024, 1024));
            settings.WindowSize.Add(new Point(1056, 1056));

            settings.SetScreenRes(0);

            snakeParts = new List<Snake>();
            foodList = new List<Food>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            testRender = Content.Load<Texture2D>("RenderTarget");
            playArea = new RenderTarget2D(_graphics.GraphicsDevice, 1024, 1024);

            snakeParts.Add(new Snake(Content.Load<Texture2D>("Textures/SnakeHead"), new Rectangle(0, 0, 32, 32)));

            snakeParts.Add(new Snake(Content.Load<Texture2D>("Textures/SnakeHead"), new Rectangle(0, 0, 32, 32)));
            snakeParts.Add(new Snake(Content.Load<Texture2D>("Textures/SnakeHead"), new Rectangle(0, 0, 32, 32)));

            snakeParts[0].SetUpBody(snakeParts);

            foodList.Add(new Food(Content.Load<Texture2D>("Textures/Food"), new Rectangle(64, 64, 32, 32)));

#if DEBUG
            debugFont = Content.Load<SpriteFont>("Fonts/DebugFont");
            debugPixel = Content.Load<Texture2D>("Textures/DebugPixel");
#endif
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kb_curr = Keyboard.GetState();

            // TODO: Add your update logic here

            settings.UpdateMe();

            foreach(Snake snake in snakeParts)
            {
                snake.UpdateMe(gameTime, kb_curr, kb_old, playArea);
            }

            if (snakeParts[0].MovementTrigger == true)
            {
                snakeParts[0].UpdateSnakePosition(gameTime, snakeParts);
            }

            if (foodList.Count > 0)
            {
                for (int i = 0; i < foodList.Count; i++)
                {
                    Food food = foodList[i];
                    food.UpdateMe(snakeParts, foodList);
                } 
            }
            else if(foodList.Count == 0)
            {
                foodList.Add(new Food(Content.Load<Texture2D>("Textures/Food"),
                    new Rectangle(GameSettings.RNG.Next(1, 32) * 32,
                    GameSettings.RNG.Next(1, 32) * 32,
                    32,
                    32)));

                snakeParts[0].AddPart = true;

                //snakeParts.Add(new Snake(Content.Load<Texture2D>("Textures/SnakeHead"),
                //        new Rectangle((int)snakeParts[snakeParts.Count - 1].Position.X,
                //        (int)snakeParts[snakeParts.Count - 1].Position.Y,
                //        32, 32)));
            }

            kb_old = kb_curr;

            //Debug.WriteLine(snakeParts.Count);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            GraphicsDevice.SetRenderTarget(playArea);
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(testRender, Vector2.Zero, Color.White);

            foreach (Snake part in snakeParts)
            {
                part.DrawMe(_spriteBatch);
            }

            foreach(Food food in foodList)
            {
                food.DrawMe(_spriteBatch);
            }

            //snakeParts.DrawMe(_spriteBatch);

#if DEBUG
            //_spriteBatch.DrawString(debugFont, snakeParts[0].Rect.Location.ToString(), Vector2.Zero, Color.White);
            //_spriteBatch.Draw(debugPixel, snakeParts.Collision, Color.White);
#endif

            _spriteBatch.End();

            _spriteBatch.Begin();

            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Draw(playArea, GraphicsDevice.Viewport.Bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
