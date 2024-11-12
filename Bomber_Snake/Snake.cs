using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomber_Snake
{
    internal class Snake : MotionGraphic
    {
        Texture2D snakeTxr;

        Rectangle colRect;
        Rectangle snakeRect;

        public Rectangle Collision
        {
            get
            {
                return colRect;
            }
        }

        public Snake(Texture2D txrImage, Rectangle rect)
            : base(txrImage, rect)
        {
            snakeTxr = txrImage;
            snakeRect = rect;

            colRect = new Rectangle((int)m_position.X + 4, (int)m_position.Y + 4, snakeTxr.Width - 8, snakeTxr.Height - 8);
        }

        public void UpdateMe(GameTime gt, KeyboardState kb_curr, KeyboardState kb_old, RenderTarget2D playArea)
        {
            UpdateCollision();

            if (kb_curr.IsKeyDown(Keys.Up) && kb_old.IsKeyUp(Keys.Up))
            {
                m_position.Y -= 32;
            }

            if (kb_curr.IsKeyDown(Keys.Down) && kb_old.IsKeyUp(Keys.Down))
            {
                m_position.Y += 32;
            }

            if (kb_curr.IsKeyDown(Keys.Right) && kb_old.IsKeyUp(Keys.Right))
            {
                m_position.X += 32;
            }

            if (kb_curr.IsKeyDown(Keys.Left) && kb_old.IsKeyUp(Keys.Left))
            {
                m_position.X -= 32;
            }

            m_position.X = MathHelper.Clamp(m_position.X, 0, playArea.Width - snakeRect.Width);
            m_position.Y = MathHelper.Clamp(m_position.Y, 0, playArea.Height - snakeRect.Height);
        }

        void UpdateCollision()
        {
            colRect = new Rectangle((int)m_position.X + 4, (int)m_position.Y + 4, snakeTxr.Width - 8, snakeTxr.Height - 8);
        }
    }
}
