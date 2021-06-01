using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class Obstacle : IForm
    {
        public Vector Location { get; set; }
        public Size Size { get; set; }

        public Obstacle(Vector location, Size size)
        {
            Location = location;
            Size = size;
        }
    }
}
