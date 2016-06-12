using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using contract;
using System.ComponentModel.Composition;
using System.Drawing.Drawing2D;

namespace drawservice
{
    [Export(typeof(IDrawClockFace))]
    public class DrawClockFaceImpl : IDrawClockFace
    {
        int cx, cy;
        int secHAND = 70, minHAND = 55, hrHAND = 40;

        //coord for minute and second hand
        private int[] msCoord(int val, int hlen)
        {
            int[] coord = new int[2];
            val *= 6;   //each minute and second make 6 degree

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        //coord for hour hand
        private int[] hrCoord(int hval, int mval, int hlen)
        {
            int[] coord = new int[2];

            //each hour makes 30 degree
            //each min makes 0.5 degree
            int val = (int)((hval * 30) + (mval * 0.5));

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        public Bitmap DrawClockFace(DateTime time, int width, int height)
        {
            var tempBitmap = new Bitmap(width, height);

            using (var g = Graphics.FromImage(tempBitmap))
            {
                // Declare the pens we will use to draw the clock hands:

                Pen blackPen = new Pen(Color.Black, 5);
                blackPen.StartCap = LineCap.RoundAnchor;
                blackPen.EndCap = LineCap.ArrowAnchor;

                Pen bluePen = new Pen(Color.Blue, 3);
                bluePen.StartCap = LineCap.RoundAnchor;
                bluePen.EndCap = LineCap.ArrowAnchor;

                Pen redPen = new Pen(Color.DarkRed, 1.5f);
                redPen.StartCap = LineCap.RoundAnchor;
                redPen.EndCap = LineCap.ArrowAnchor;


                // Create location and size of ellipse.

                float x = 0.0F;

                float y = 10.0F;

                float _width = width * 0.8F;

                float _height = height * 0.8F;

                cx = (int)_width / 2;

                cy = (int)_height / 2;

                g.DrawString("12", new Font("Arial", 12), Brushes.Black, new PointF(cx - 10, 0));
                g.DrawString("3", new Font("Arial", 12), Brushes.Black, new PointF(_width - 10, cy - 10));
                g.DrawString("6", new Font("Arial", 12), Brushes.Black, new PointF(cx - 7, _height - 10));
                g.DrawString("9", new Font("Arial", 12), Brushes.Black, new PointF(0, cy - 10));

                //second hand
                int[] handCoord = msCoord(DateTime.Now.Second, secHAND);
                g.DrawLine(redPen, new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

                //minute hand
                handCoord = msCoord(DateTime.Now.Minute, minHAND);
                g.DrawLine(bluePen, new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

                //hour hand
                handCoord = hrCoord(DateTime.Now.Hour % 12, DateTime.Now.Minute, hrHAND);
                g.DrawLine(blackPen, new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

                return tempBitmap;

            }

        }
    }
}
