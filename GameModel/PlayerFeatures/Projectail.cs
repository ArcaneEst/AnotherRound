using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class Projectail : ICircle
    {
        public int Speed { get; set; }
        public Size Size { get; set; }
        public double DirectionAngle { get; set; }
        public Vector Location { get; set; }

        public double Radius => Size.Width / 2;

        public Projectail(int speed, Size size)
        {
            Speed = speed;
            Size = size;
        }

        public Vector CalculateNewLocation()
        {
            var deltaX = (int)Math.Round(Speed * Math.Cos(DirectionAngle));
            var deltaY = (int)Math.Round(Speed * Math.Sin(DirectionAngle));

            return new Vector(Location.X + deltaX, Location.Y + deltaY);
        }
    }
}
