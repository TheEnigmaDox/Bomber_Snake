using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomber_Snake
{
    class StaticGraphic
    {
        protected Rectangle m_rect;
        protected Texture2D m_txr;

        public StaticGraphic(Texture2D txrImage, Rectangle rectPosition)
        {
            m_rect = rectPosition;
            m_txr = txrImage;
        }

        public StaticGraphic(Texture2D txrImage, int xPos, int yPos, int width, int height)
            : this(txrImage, new Rectangle(xPos, yPos, width, height))
        {

        }

        public virtual void DrawMe(SpriteBatch sBatch)
        {
            sBatch.Draw(m_txr, m_rect, Color.White);
        }
    }

    class MotionGraphic : StaticGraphic
    {
        protected Vector2 m_position;
        protected Vector2 m_velocity;

        public Vector2 Position
        {
            get
            {
                return m_position;
            }
        }

        public Rectangle Rect
        {
            get
            {
                return m_rect;
            }
        }

        public MotionGraphic(Texture2D txr, Rectangle rect)
            : base(txr, rect)
        {
            m_position = new Vector2(rect.X, rect.Y);
            m_velocity = Vector2.Zero;
        }

        public override void DrawMe(SpriteBatch sBatch)
        {
            m_rect.X = (int)m_position.X;
            m_rect.Y = (int)m_position.Y;

            sBatch.Draw(m_txr, m_rect, Color.White);
        }
    }
}
