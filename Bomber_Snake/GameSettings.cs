using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Bomber_Snake
{
    class GameSettings
    {
        private int currentRes = 0;
        private float scale = 1f;

        private List<Point> m_windowSize = new List<Point> ();

        public List<Point> WindowSize
        {
            get
            {
                return m_windowSize;
            }
            set
            {
                m_windowSize = value;
            }
        }

        public int CurrentRes
        {
            get
            {
                return currentRes;
            }
        }

        GraphicsDeviceManager m_graphics;

        public GameSettings(GraphicsDeviceManager graphics) 
        {
            m_graphics = graphics;
        }

        public void SetScreenRes(int index)
        {
            currentRes = index;

            if(currentRes >= m_windowSize.Count)
            {
                currentRes = 0;
            }
            if(currentRes < 0)
            {
                currentRes = m_windowSize.Count - 1;
            }

            m_graphics.PreferredBackBufferWidth = m_windowSize[currentRes].X;
            m_graphics.PreferredBackBufferHeight = m_windowSize[currentRes].Y;

            m_graphics.ApplyChanges();
        }

        public void UpdateMe()
        {

        }
    }
}
