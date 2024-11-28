using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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
        private int m_gridSize = 32;

        private bool m_move = false;
        private bool m_addPart = false;

        private float m_moveTimer = 0f;

        private Texture2D m_snakeTxr;

        private Rectangle m_colRect;
        private Rectangle m_snakeRect;

        private Vector2 m_direction;

        public Rectangle Collision
        {
            get
            {
                return m_colRect;
            }
        }

        public bool MovementTrigger
        {
            get
            {
                return m_move;
            }
            set
            {
                m_move = value;
            }
        }

        public bool AddPart
        {
            set
            {
                m_addPart = value;
            }
        }

        public Snake(Texture2D txrImage, Rectangle rect)
            : base(txrImage, rect)
        {
            m_snakeTxr = txrImage;
            m_snakeRect = rect;

            //m_direction = new Vector2(1, 0);

            m_colRect = new Rectangle((int)m_position.X + 4, (int)m_position.Y + 4, m_snakeTxr.Width - 8, m_snakeTxr.Height - 8);
            
        }

        public void UpdateMe(GameTime gt, KeyboardState kb_curr, KeyboardState kb_old, RenderTarget2D playArea)
        {
            UpdateCollision();

            if(kb_curr.GetPressedKeyCount() > 0)
            {
                Keys[] pressedKeys = kb_curr.GetPressedKeys();

                for(int i = 0; i < pressedKeys.Length; i++)
                {
                    if(m_moveTimer > 0)
                    {
                        m_move = false;
                    }
                    else if (pressedKeys[i] == Keys.A && m_moveTimer <= 0 ||
                        pressedKeys[i] == Keys.D && m_moveTimer <= 0 ||
                        pressedKeys[i] == Keys.W && m_moveTimer <= 0 ||
                        pressedKeys[i] == Keys.S && m_moveTimer <= 0)
                    {
                        m_move = true;
                        m_moveTimer = 0.001f;
                        break;
                    }
                }
            }
            else
            {
                m_moveTimer -= (float)gt.ElapsedGameTime.TotalSeconds;
            }

            CheckInputs(kb_curr, kb_old);

            m_position.X = MathHelper.Clamp(m_position.X, 0, playArea.Width - m_snakeRect.Width);
            m_position.Y = MathHelper.Clamp(m_position.Y, 0, playArea.Height - m_snakeRect.Height);

            //Debug.WriteLine("Direction List Count: " + m_directionList.Count);
        }

        public void SetUpBody(List<Snake> parts)
        {
            //Create a position for the head to move to.
            Vector2 newHeadPosition = m_position + m_direction;

            //Loop through the list of parts backwards...
            for (int i = parts.Count - 1; i > 0; i--)
            {
                //...Set each part to the position of the one before it...
                parts[i].m_position = parts[i - 1].m_position;
            }

            //Move the head to the new position.
            parts[0].m_position = newHeadPosition;
        }

        //Check the inputs of the keyboard and only change the direction if a viable direction is chosen.
        void CheckInputs(KeyboardState kb_curr, KeyboardState kb_old)
        {
            //If W is pressed and the direction is not equal to down...
            if (kb_curr.IsKeyDown(Keys.W) && kb_old.IsKeyUp(Keys.W) && m_direction != new Vector2(0, 1))
            {
                //...Set direction to up.
                m_direction = new Vector2(0, -1);
            }
            //If S is pressed and direction is not equal to up...
            else if (kb_curr.IsKeyDown(Keys.S) && kb_old.IsKeyUp(Keys.S) && m_direction != new Vector2(0, -1))
            {
                //...Set the direction to down.
                m_direction = new Vector2(0, 1);
            }
            //If A is pressed and the direction is not equal to right...
            else if (kb_curr.IsKeyDown(Keys.A) && kb_old.IsKeyUp(Keys.A) && m_direction != new Vector2(1, 0))
            {
                //...Set the direction to left.
                m_direction = new Vector2(-1, 0);
            }
            //If D is pressed and direction is not equal to to left...
            else if (kb_curr.IsKeyDown(Keys.D) && kb_old.IsKeyUp(Keys.D) && m_direction != new Vector2(-1, 0))
            {
                //... Set direction to right.
                m_direction = new Vector2(1, 0);
            }
        }

        //Function to update the snakes position.
        public void UpdateSnakePosition(GameTime gt, List<Snake> parts) 
        {
            //Create a position for the head to move to.
            Vector2 newHeadPosition = m_position + (m_direction * m_gridSize);

            if (m_addPart)
            {
                parts.Add(new Snake(m_snakeTxr,
                        new Rectangle((int)parts[parts.Count - 1].Position.X,
                        (int)parts[parts.Count - 1].Position.Y,
                        32, 32)));

                m_addPart = false;
            }

            //Loop through the list of parts backwards...
            for(int i = parts.Count - 1; i > 0; i--)
            {
                //...Set each part to the position before it.
                parts[i].m_position = parts[i - 1].m_position;
            }

            //Move the head to the new position.
            parts[0].m_position = newHeadPosition;
        }

        void UpdateCollision()
        {
            m_colRect = new Rectangle((int)m_position.X + 4, (int)m_position.Y + 4, m_snakeTxr.Width - 8, m_snakeTxr.Height - 8);
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
            }
        }
    }
}
