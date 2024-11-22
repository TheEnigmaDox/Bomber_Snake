using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bomber_Snake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Vector2 snakePos = Vector2.Zero;

        List<Snake> snakeList;
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

            settings = new GameSettings(_graphics);

            settings.WindowSize.Add(new Point(800, 800));
            settings.WindowSize.Add(new Point(1024, 1024));
            settings.WindowSize.Add(new Point(1056, 1056));

            settings.SetScreenRes(0);

            snakeList = new List<Snake>();
            foodList = new List<Food>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            testRender = Content.Load<Texture2D>("RenderTarget");
            playArea = new RenderTarget2D(_graphics.GraphicsDevice, 1024, 1024);

            snakeList.Add(new Snake(Content.Load<Texture2D>("Textures/SnakeHead"), new Rectangle(0, 0, 32, 32)));

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

            foreach(Snake snake in snakeList)
            {
                snake.UpdateMe(gameTime, kb_curr, kb_old, playArea, snakeList);
            }
            //snakeList.UpdateMe(gameTime, kb_curr, kb_old, playArea);

            if (foodList.Count > 0)
            {
                for (int i = 0; i < foodList.Count; i++)
                {
                    Food food = foodList[i];
                    food.UpdateMe(snakeList, foodList);
                } 
            }
            else if(foodList.Count == 0)
            {
                foodList.Add(new Food(Content.Load<Texture2D>("Textures/Food"),
                    new Rectangle(GameSettings.RNG.Next(1, 33) * 32,
                    GameSettings.RNG.Next(1, 33) * 32,
                    32,
                    32)));

                switch (snakeList[0].Facing)
                {
                    case Direction.North:
                        snakeList.Add(new Snake(Content.Load<Texture2D>("Textures/SnakeHead"),
                            new Rectangle(snakeList[0].Rect.X, snakeList[0].Rect.Y + 32, 32, 32)));
                        break;
                }

                //snakeList.Add(new Snake(Content.Load<Texture2D>("Textures/SnakeHead"),
                //        new Rectangle(0, 0, 32, 32)));
            }

            kb_old = kb_curr;

            Debug.WriteLine(snakeList.Count);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            GraphicsDevice.SetRenderTarget(playArea);
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(testRender, Vector2.Zero, Color.White);

            foreach (Snake snake in snakeList)
            {
                snake.DrawMe(_spriteBatch);
            }

            foreach(Food food in foodList)
            {
                food.DrawMe(_spriteBatch);
            }

            //snakeList.DrawMe(_spriteBatch);

#if DEBUG
            //_spriteBatch.DrawString(debugFont, snakeList[0].Rect.Location.ToString(), Vector2.Zero, Color.White);
            //_spriteBatch.Draw(debugPixel, snakeList.Collision, Color.White);
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
