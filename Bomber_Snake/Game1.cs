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

        Vector2 snakePos = Vector2.Zero;

        Snake snake;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            testRender = Content.Load<Texture2D>("RenderTarget");
            playArea = new RenderTarget2D(_graphics.GraphicsDevice, 1024, 1024);

            snake = new Snake(Content.Load<Texture2D>("Textures/SnakeHead"), new Rectangle(0, 0, 32, 32));

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

            snake.UpdateMe(gameTime, kb_curr, kb_old, playArea);

            kb_old = kb_curr;

            Debug.WriteLine(snakePos);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            GraphicsDevice.SetRenderTarget(playArea);
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(testRender, Vector2.Zero, Color.White);

            snake.DrawMe(_spriteBatch);

#if DEBUG
            _spriteBatch.DrawString(debugFont, snake.Rect.Location.ToString(), Vector2.Zero, Color.White);
            _spriteBatch.Draw(debugPixel, snake.Collision, Color.White);
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
