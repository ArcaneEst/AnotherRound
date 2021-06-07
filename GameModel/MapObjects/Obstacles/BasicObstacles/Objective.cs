using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    class Objective : SquareObstacle
    {
        public Objective(Vector location, Size size) : base(location, size)
        {
            Location = location;
            Size = size;
        } 
    }
}
