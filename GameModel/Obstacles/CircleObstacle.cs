using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class CircleObstacle : Obstacle, ICircle
    {
        public new Vector Location { get ; set ; }
        public new Size Size { get ; set ; }

        public double Radius => Size.Width / 2;

        public CircleObstacle(Vector location, Size size) : base(location, size)
        {
            Location = location;
            Size = size;
        }
    }
}
