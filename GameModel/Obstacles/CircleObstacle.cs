using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class CircleObstacle :ICircle
    {
        public Vector Location { get ; set ; }
        public Size Size { get ; set ; }

        public double Radius => Size.Width / 2;

        public CircleObstacle(Vector location, Size size)
        {
            Location = location;
            Size = size;
        }
    }
}
