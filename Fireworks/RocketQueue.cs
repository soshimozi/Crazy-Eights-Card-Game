using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Fireworks
{
    public class RocketQueue
    {
        Rocket firstRocket = null;
        Rocket lastRocket = null;
        Size _canvas = new Size();

        Random _rng;

        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public RocketQueue(Random rng, Size canvas)
        {
            _rng = rng;
            _canvas = canvas;
        }


        public void Clear()
        {
            firstRocket = lastRocket = null;
        }

        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public void addRocket(Rocket rocket)
        {
            try
            {

                if (firstRocket == null)
                {
                    firstRocket = rocket;
                    lastRocket = rocket;
                }
                else if (firstRocket == lastRocket)
                {
                    firstRocket.setNextRocket(rocket);
                    lastRocket = rocket;
                }
                else
                {
                    lastRocket.setNextRocket(rocket);
                    lastRocket = rocket;
                }

            }
            catch (Exception e)
            {
            }
        }


        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public void addRandomRocket()
        {
            try
            {

                int width = _canvas.Width;
                int height = _canvas.Height;

                int widthDivision = 100;
                int widthInner = width - (widthDivision * 2);
                int height4 = (int)(height / 4);

                double angle = (_rng.NextDouble() * 30.0d) + 75.0d; ;
                double speed = (_rng.NextDouble() * 45.0) + (double)height4;
                double explode = (_rng.NextDouble() * 2.0) + 1.5d;

                int xPos = (int)(widthDivision + ((double)_rng.NextDouble() * widthInner));
                int yPos = height;
                Point position = new Point(xPos, yPos);

                Rocket rocket = new Rocket(_rng, angle, speed);
                rocket.setExplodeTime(explode);
                rocket.setPosition(position);

                //double sparkLifespan = 1.5;
                //rocket.setSparkLifespan(sparkLifespan);

                //rocket.setColor(Color.FromArgb(213, 141, 27));
                rocket.setColor(Color.WhiteSmoke);

                addRocket(rocket);

            }
            catch (Exception e)
            {
            }
        }


        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public void makeSparks(SparkQueue sparkQueue)
        {
            try
            {

                if (firstRocket == null)
                {
                    return;
                }
                else if (firstRocket.getNextRocket() == null && firstRocket.isDead())
                {
                    firstRocket = null;
                    lastRocket = null;
                    return;
                }

                Rocket rocket = firstRocket;
                bool hasMoreRockets = true;
                while (hasMoreRockets)
                {

                    // if the rocket is ready to explode, add the new rockets
                    // to the end of the queue for painting
                    rocket.checkExplode(this);
                    rocket.makeSparks(sparkQueue);

                    // loop until we find the next alive rocket, clipping out the finished rockets
                    Rocket nextRocket = rocket.getNextRocket();
                    bool finished = false;
                    while (!finished)
                    {
                        if (nextRocket == null)
                        {
                            // the end of the queue
                            rocket.setNextRocket(null);
                            lastRocket = rocket;
                            hasMoreRockets = false;
                            finished = true;
                        }
                        else if (nextRocket.isDead())
                        {
                            nextRocket = nextRocket.getNextRocket();
                        }
                        else
                        {
                            rocket.setNextRocket(nextRocket);
                            rocket = nextRocket;
                            finished = true;
                        }
                    }

                    // This must be done at the end of the loop incase the last
                    // rocket explodes and adds more rockets to the queue
                    if (rocket == lastRocket)
                    {
                        //hasMoreRockets = false;
                    }

                }

            }
            catch (Exception e)
            {
            }
        }


        public void setCanvas(Size canvas)
        {
            _canvas = canvas;
        }
    }
}

