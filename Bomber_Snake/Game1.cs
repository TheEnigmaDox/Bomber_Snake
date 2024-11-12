using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Bomber_Snake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //public static Point windowSize = new Point(1056, 1056);

        Vector2 snakePos = Vector2.Zero;

        Texture2D snake;

        Texture2D testRender;
        RenderTarget2D playArea;

        KeyboardState kb_curr, kb_old;

        GameSettings settings;

#if DEBUG
        SpriteFont debugFont;
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            testRender = Content.Load<Texture2D>("RenderTarget");
            playArea = new RenderTarget2D(_graphics.GraphicsDevice, 1024, 1024);

            snake = Content.Load<Texture2D>("Textures/SnakeHead");

#if DEBUG
            debugFont = Content.Load<SpriteFont>("Fonts/DebugFont");
#endif
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kb_curr = Keyboard.GetState();

            // TODO: Add your update logic here

            settings.UpdateMe();

            if (kb_curr.IsKeyDown(Keys.Up) && kb_old.IsKeyUp(Keys.Up))
            {
                snakePos.Y -= 32;
            }

            if (kb_curr.IsKeyDown(Keys.Down) && kb_old.IsKeyUp(Keys.Down))
            {
                snakePos.Y += 32;
            }

            if (kb_curr.IsKeyDown(Keys.Right) && kb_old.IsKeyUp(Keys.Right))
            {
                snakePos.X += 32;
            }

            if (kb_curr.IsKeyDown(Keys.Left) && kb_old.IsKeyUp(Keys.Left))
            {
                snakePos.X -= 32;
            }

            snakePos.X = MathHelper.Clamp(snakePos.X, 0, playArea.Width - snake.Width);
            snakePos.Y = MathHelper.Clamp(snakePos.Y, 0, playArea.Height - snake.Height);

            kb_old = kb_curr;

            Debug.WriteLine(snakePos);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            GraphicsDevice.SetRenderTarget(playArea);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(testRender, Vector2.Zero, Color.White);

            _spriteBatch.Draw(snake, snakePos, Color.White);

#if DEBUG
            _spriteBatch.DrawString(debugFont, snakePos.ToString(), Vector2.Zero, Color.White);
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
