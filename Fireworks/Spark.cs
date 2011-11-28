using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Fireworks
{
    public class Spark
    {
        /** The size and color of the spark **/
        private int _size = 5;
        private Color _color = Color.Black;
        private Point _position = new Point();
        private double _lifespan = 0;
        private const double _timeIncrement = 0.10d;

        //private int _redDarken = 1;
        //private int _blueDarken = 1;
        //private int _greenDarken = 1;

        private int _fadeDarken = 1;

        private bool _dead = false;
        private bool _paintLast = false; // paints a final black dot just before it dies
        Spark _nextSpark = null;

        public Spark()
        {
        }

        public Spark(Point position, Color color, int size, double lifespan)
        {
            _position = position;
            _size = size;
            _color = color;
            Lifespan = lifespan;
        }

        public void Update()
        {
            Lifespan -= _timeIncrement;

            if (_lifespan <= 0)
            {
                _paintLast = true;
            }
        }

        /////////////////
        // GET METHODS //
        /////////////////

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }

        //public Spark NextSpark
        //{
        //    get { return _nextSpark; }
        //    set { _nextSpark = value; }
        //}


        public double Lifespan
        {
            get { return _lifespan; }
            set
            {
                _lifespan = value;

                int fade = (int)(_lifespan / _timeIncrement);
                if (fade > 0)
                {
                    _fadeDarken = _color.A / fade;
                }

                //int darkenIncrement = (int)(_lifespan / _timeIncrement);

                //if (darkenIncrement > 0)
                //{
                //    _redDarken = _color.R / darkenIncrement;
                //    _greenDarken = _color.G / darkenIncrement;
                //    _blueDarken = _color.B / darkenIncrement;
                //}
            }
        }

        public bool Dead
        {
            get { return _dead; }
            set { _dead = value; }
        }

        public bool IsPaintLast
        {
            get { return _paintLast; }
            set { _paintLast = value; }
        }

        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public void CalculateNextColor()
        {
            try
            {

                int alpha = _color.A - _fadeDarken;
                int red = _color.R; // -_redDarken;
                int green = _color.G; // -_greenDarken;
                int blue = _color.B; // -_blueDarken;
                _color = Color.FromArgb(alpha, red, green, blue);

            }
            catch (Exception e)
            {
                //color = Color.BLACK;
            }
        }


        public void Draw(System.Drawing.Imaging.BitmapData bitmapData)
        {
        }

        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public void Paint(Graphics g)
        {
            int drawX;
            int drawY;
            
            try
            {
                if (_dead)
                {
                    return;
                }

                if (_paintLast)
                {
                    _size++;
                    _color = Color.Black;
                    _dead = true;

                    drawX = (int)_position.X;
                    drawY = (int)_position.Y;

                    
                    g.FillEllipse(new SolidBrush(_color), drawX, drawY, _size, _size);

                    return;
                }

                drawX = (int)_position.X;
                drawY = (int)_position.Y;

                g.FillEllipse(new SolidBrush(_color), drawX, drawY, _size, _size);

                Update();
            }
            catch (Exception e)
            {
            }
        }
    }
}
