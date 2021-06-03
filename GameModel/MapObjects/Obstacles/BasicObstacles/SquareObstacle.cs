using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class SquareObstacle : Obstacle, ISquare
    {
        public Vector MinPoints => new Vector(Location.X - (Size.Width / 2), Location.Y - (Size.Height / 2));
        public Vector MaxPoints => new Vector(Location.X + (Size.Width / 2), Location.Y + (Size.Height / 2));

        public SquareObstacle(Vector location, Size size) : base(location, size)
        {
            Location = location;
            Size = size;
        }
    }
}
