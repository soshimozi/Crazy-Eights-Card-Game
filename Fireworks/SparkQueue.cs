using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Fireworks
{
    public class SparkQueue
    {
        [DllImport("Gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int
        width, int height);

        private List<Spark> _sparks = new List<Spark>();
        //private Spark _lastSpark = ;
        //private Size canvas = new Size();

        public SparkQueue()
        {
            //this.canvas = canvas;
        }

        public void UpdateState()
        {
            foreach (var spark in _sparks)
            { 
                spark.Update();
            }
        }

        public void Paint(Graphics g)
        {
            try
            {
                // This assumes that cullDeadSparks() has already been called
                // Thus all the sparks remaining are alive, and can be painted
                foreach(var spark in _sparks)
                {
                    spark.Paint(g);
                }

            }
            catch (Exception e)
            {
            }
        }

        public void Clear()
        {
            _sparks.Clear();
        }

        public void AddSpark(Spark spark)
        {
            try
            {
                _sparks.Add(spark);
            }
            catch (Exception e)
            {
            }
        }

        public void CullDeadSparks(bool useFading, bool useFullClear)
        {
            try
            {
                //while (_sparks.Count > 0)
                //{
                    var removeQueue = new List<Spark>();

                    // loop until we find the next alive spark, clipping out the finished sparks
                    foreach (var spark in _sparks)
                    {
                        if (spark.Dead || (useFullClear && spark.IsPaintLast))
                        {
                            removeQueue.Add(spark);
                            continue;
                        }

                        if (useFading)
                        {
                            spark.CalculateNextColor();
                        }
                    }

                    // cull the dead sparks
                    foreach (var spark in removeQueue)
                    {
                        _sparks.Remove(spark);
                    }

                //}
            }
            catch (Exception e)
            {
            }
        }
    }
}
