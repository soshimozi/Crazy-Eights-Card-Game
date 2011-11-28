using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Fireworks
{
    public class Rocket
    {

        /////////////
        // PHYSICS //
        /////////////

        /** The angle and speed of the rocket **/
        double angle = 90;
        double speed = 70;

        /** The initial x and y positions of the rocket **/
        int xPos = 200;
        int yPos = 400;

        /** The speed of the rocket in each direction, calculated from the angle and overall speed **/
        double xSpeed = 0;
        double ySpeed = 0;


        ////////////
        // TIMING //
        ////////////

        /** The time that has passed since this rocket was launched **/
        double time = 0;

        /** The amount of time that passes at each paint call **/
        private double _timeIncrement = 0.05d;

        /** The number of seconds that this spark stays alive **/
        double lifespan = 3;

        /** Time until the rocket is launched **/
        //double launchTime = 0;


        ////////////
        // STATUS //
        ////////////

        /** Is this spark still alive **/
        bool dead = false;
        //bool launched = false;




        /////////
        // NEW //
        /////////

        /** The next rocket in the queue **/
        Rocket nextRocket = null;

        double explodeTime = 3;
        bool lastRocket = false;
        bool exploded = false;

        double sparkLifespan = 0.5d;


        Color color = Color.Gray;
        int size = 1;



        ////////////////
        // SEQUENCING //
        ////////////////

        bool launched = false;
        double launchTime = 0;

        Random _rng; // = new Random();

        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public Rocket(Random rng)
        {
            _rng = rng;
            calculateSpeeds();

        }


        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public Rocket(Random rng, double angle, double speed)
        {
            _rng = rng;
            setAngle(angle);
            setSpeed(speed);

            if (lifespan <= explodeTime)
            {
                lifespan = explodeTime;
            }

        }


        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public Rocket(Random rng, double launchTime, double angle, double speed, int xPos, int yPos, double lifespan, double explodeTime, bool lastRocket, double sparkLifespan, Color color, int size)
        {
            _rng = rng;

            setAngle(angle);
            setSpeed(speed);

            this.launchTime = launchTime;
            this.xPos = xPos;
            this.yPos = yPos;
            this.lifespan = lifespan;
            this.explodeTime = explodeTime;
            this.lastRocket = lastRocket;
            this.sparkLifespan = sparkLifespan;
            this.color = color;
            this.size = size;

            if (lifespan <= explodeTime)
            {
                lifespan = explodeTime;
            }

            calculateSpeeds();

        }


        /**
        **********************************************************************************************
          Calculates and sets the xSpeed and ySpeed variables
        **********************************************************************************************
        **/
        public void calculateSpeeds()
        {
            // Calculation of fixed physical properties
            double angleRadian = AngleToRadians(angle);
            xSpeed = speed * Math.Cos(angleRadian);
            ySpeed = speed * Math.Sin(angleRadian);
        }

        private double AngleToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }



        /////////////////
        // GET METHODS //
        /////////////////

        public double getAngle()
        {
            return angle;
        }

        public double getSpeed()
        {
            return speed;
        }

        public int getXPos()
        {
            return xPos;
        }

        public int getYPos()
        {
            return yPos;
        }

        public double getXSpeed()
        {
            return xSpeed;
        }

        public double getYSpeed()
        {
            return ySpeed;
        }

        public double getTime()
        {
            return time;
        }

        public double getTimeIncrement()
        {
            return _timeIncrement;
        }

        public double getLifespan()
        {
            return lifespan;
        }

        public double getSparkLifespan()
        {
            return sparkLifespan;
        }

        public double getLaunchTime()
        {
            return launchTime;
        }

        public double getExplodeTime()
        {
            return explodeTime;
        }

        public Rocket getNextRocket()
        {
            return nextRocket;
        }

        public Color getColor()
        {
            return color;
        }

        public int getSize()
        {
            return size;
        }

        public bool isDead()
        {
            return dead;
        }

        public bool isLaunched()
        {
            return launched;
        }

        public bool isLastRocket()
        {
            return lastRocket;
        }


        /////////////////
        // SET METHODS //
        /////////////////

        public void setAngle(double angle)
        {
            this.angle = angle;
        }

        public void setSpeed(double speed)
        {
            this.speed = speed;

            if (xSpeed == 0 || ySpeed == 0)
            {
                calculateSpeeds();
            }
        }

        public void setXPos(int xPos)
        {
            this.xPos = xPos;
        }

        public void setYPos(int yPos)
        {
            this.yPos = yPos;
        }

        public void setXSpeed(double xSpeed)
        {
            this.xSpeed = xSpeed;
        }

        public void setYSpeed(double ySpeed)
        {
            this.ySpeed = ySpeed;
        }

        public void setTime(double time)
        {
            this.time = time;
        }

        public void setTimeIncrement(double timeIncrement)
        {
            _timeIncrement = timeIncrement;
        }

        public void setLifespan(double lifespan)
        {
            this.lifespan = lifespan;

            if (lifespan <= explodeTime)
            {
                lifespan = explodeTime;
            }
        }

        public void setSparkLifespan(double sparkLifespan)
        {
            this.sparkLifespan = sparkLifespan;
        }

        public void setLaunchTime(double launchTime)
        {
            this.launchTime = launchTime;
        }

        public void setExplodeTime(double explodeTime)
        {
            this.explodeTime = explodeTime;
            if (lifespan <= explodeTime)
            {
                lifespan = explodeTime;
            }
        }

        public void setNextRocket(Rocket nextRocket)
        {
            this.nextRocket = nextRocket;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public void setSize(int size)
        {
            this.size = size;
        }

        public void setDead(bool dead)
        {
            this.dead = dead;
        }

        public void setLaunched(bool launched)
        {
            this.launched = launched;
        }

        public void setLastRocket(bool lastRocket)
        {
            this.lastRocket = lastRocket;
        }

        public void setPosition(int xPos, int yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;
        }

        public void setPosition(Point point)
        {
            this.xPos = (int)point.X;
            this.yPos = (int)point.Y;
        }



        /**
        **********************************************************************************************
          Calculates the current position of the rocket based on the X/Y speeds and time counter
        **********************************************************************************************
        **/
        public Point calculatePosition()
        {
            try
            {

                double xDist = xSpeed * time;
                double yDist = ySpeed * time + 0.5 * -9.8 * (time * time);

                int drawX = (int)(xPos + xDist);
                int drawY = (int)(yPos - yDist);

                return new Point(drawX, drawY);

            }
            catch (Exception e)
            {
                return new Point(-1, -1);
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

                if (dead)
                {
                    return;
                }

                Point position = calculatePosition();
                //Color color = Color.WHITE;
                //int size = 5;

                double newLifeSpan = sparkLifespan + (_rng.NextDouble() - .5d);
                Spark spark = new Spark(position, color, size, newLifeSpan);
                sparkQueue.AddSpark(spark);

                if (time > lifespan)
                {
                    dead = true;
                }

                time += _timeIncrement;


            }
            catch (Exception e)
            {
            }
        }



        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public void checkExplode(RocketQueue rocketQueue)
        {
            try
            {

                if (lastRocket || exploded || dead)
                {
                    return;
                }

                if (time >= explodeTime)
                {

                    int[] numSparkChoice = new int[] { 5, 8, 10 };

                    int numSparks = numSparkChoice[_rng.Next(0, 2)];

                    //int numSparks = 20;
                    //System.out.println(numSparks);

                    double rotation = (360 / numSparks);
                    double sparkAngle = 5;
                    double sparkSpeed = _rng.NextDouble() * 20 + 10 ;
                    double lifespan = 2.0;
                    double sparkLifespan = 1.5;
                    int sparkSize = 2;

                    Color randomColor = makeRandomColor();

                    for (int i = 0; i < numSparks; i++)
                    {
                        Rocket rocket = new Rocket(_rng, sparkAngle, sparkSpeed);
                        rocket.setPosition(calculatePosition());
                        rocket.setSparkLifespan(sparkLifespan);
                        rocket.setLifespan(lifespan);
                        rocket.setLastRocket(true);
                        rocket.setColor(randomColor);
                        rocket.setSize(sparkSize);

                        rocketQueue.addRocket(rocket);

                        sparkAngle += rotation;
                    }

                    exploded = true;
                    dead = true;

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
        public Color makeRandomColor()
        {
            Color[] colors = new Color[] {Color.Red, Color.Blue, Color.BlueViolet, 
                Color.Violet, Color.WhiteSmoke, Color.YellowGreen, Color.Tomato, Color.Silver,
                Color.Plum, Color.OrangeRed, Color.Orange, Color.Navy, Color.MediumOrchid, Color.MediumBlue,
                Color.Maroon, Color.Magenta, Color.LimeGreen, Color.LightYellow};

            int randomIndex = _rng.Next(0, colors.Length);

            //_rng.NextBytes(randomValues);

            //int red = randomValues[0];
            //int green = randomValues[1];
            //int blue = randomValues[2];
            //if (red < 70)
            //{
            //    red += 180;
            //}

            //if (green < 70)
            //{
            //    green += 180;
            //}

            //if (blue < 70)
            //{
            //    blue += 180;
            //}

            return colors[randomIndex];

        }



        /**
        **********************************************************************************************

        **********************************************************************************************
        **/
        public bool canLaunch(double time)
        {

            if (!launched && launchTime <= time)
            {
                launched = true;
                return true;
            }

            return false;

        }


    }
}

