using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace Bomber_Snake
{
    enum Direction
    {
        North,
        East,
        South,
        West
    }

    internal class Snake : MotionGraphic
    {
        private float m_moveTimer = 0f;

        private Texture2D m_snakeTxr;

        private Rectangle m_colRect;
        private Rectangle m_snakeRect;

        private Direction m_direction;

        private List<Direction> m_directionList;

        public Rectangle Collision
        {
            get
            {
                return m_colRect;
            }
        }

        public Direction Facing
        {
            get
            {
                return m_direction;
            }
            set
            {
                m_direction = value;
            }
        }

        public List<Direction> Directions
        {
            get
            {
                return m_directionList;
                //m_directionList = value;
            }
        }

        public Snake(Texture2D txrImage, Rectangle rect)
            : base(txrImage, rect)
        {
            m_snakeTxr = txrImage;
            m_snakeRect = rect;

            m_colRect = new Rectangle((int)m_position.X + 4, (int)m_position.Y + 4, m_snakeTxr.Width - 8, m_snakeTxr.Height - 8);

            m_directionList = new List<Direction>();    
        }

        public void UpdateMe(GameTime gt, KeyboardState kb_curr, KeyboardState kb_old, RenderTarget2D playArea,
            List<Snake> snakeList)
        {
            Direction newHeadPos = m_direction;

            UpdateCollision();

            MoveSnake(kb_curr, kb_old);

            if (m_moveTimer > 0)
            {
                m_moveTimer -= (float)gt.ElapsedGameTime.TotalSeconds;

                switch (m_direction)
                {
                    case Direction.North:
                        m_position.Y -= 32;
                        break;
                    case Direction.East:
                        m_position.X += 32;
                        break;
                    case Direction.South:
                        m_position.Y += 32;
                        break;
                    case Direction.West:
                        m_position.X -= 32;
                        break;
                }
            }

            m_position.X = MathHelper.Clamp(m_position.X, 0, playArea.Width - m_snakeRect.Width);
            m_position.Y = MathHelper.Clamp(m_position.Y, 0, playArea.Height - m_snakeRect.Height);

            for(int i = snakeList.Count - 1; i > 0; i--)
            {
                snakeList[i].Facing = snakeList[i - 1].Facing;
            }

            snakeList[0].Facing = newHeadPos;

            for(int i = 0; i < snakeList.Count; i++)
            {
                Debug.WriteLine("Snake part: " + snakeList[i] + "Facing: " + snakeList[i].Facing);
            }

            Debug.WriteLine("Direction List Count: " + m_directionList.Count);
        }

        void UpdateCollision()
        {
            m_colRect = new Rectangle((int)m_position.X + 4, (int)m_position.Y + 4, m_snakeTxr.Width - 8, m_snakeTxr.Height - 8);
        }

        void MoveSnake(KeyboardState kb_curr, KeyboardState kb_old)
        {
            if (kb_curr.IsKeyDown(Keys.W) && kb_old.IsKeyUp(Keys.W))
            {
                m_moveTimer = 0.001f;
                m_direction = Direction.North;
            }

            if (kb_curr.IsKeyDown(Keys.S) && kb_old.IsKeyUp(Keys.S))
            {
                m_moveTimer = 0.001f;
                m_direction = Direction.South;
            }

            if (kb_curr.IsKeyDown(Keys.D) && kb_old.IsKeyUp(Keys.D))
            {
                m_moveTimer = 0.001f;
                m_direction = Direction.East;
            }

            if (kb_curr.IsKeyDown(Keys.A) && kb_old.IsKeyUp(Keys.A))
            {
                m_moveTimer = 0.001f;
                m_direction = Direction.West;
            }
        }
    }

    class Food : StaticGraphic
    {
        Rectangle m_colRect;

        public Food(Texture2D foodTxr, Rectangle rect)
            : base(foodTxr, rect)
        {
            m_colRect = new Rectangle(rect.X + 4, rect.Y + 4, 16, 16);
        }

        public void UpdateMe(List<Snake> snake, List<Food> food)
        {
            if (snake[0].Collision.Intersects(m_colRect))
            {
                food.Remove(this);
                snake[0].Directions.Add(snake[0].Facing);
            }
        }
    }
}
