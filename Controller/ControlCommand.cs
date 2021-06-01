using System;
using System.Collections.Generic;
using System.Text;

namespace AnotherRound
{
    public class ControlCommand
    {
        public Direction X { get; set; }
        public Direction Y { get; set; }

        public ControlCommand(Direction x, Direction y)
        {
            X = x;
            Y = y;
        }
    }
}
