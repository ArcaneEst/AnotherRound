using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    class CircleDamage : CircleObstacle, ICanDamage
    {
        public CircleDamage(Vector location, Size size) : base(location, size)
        {
            Location = location;
            Size = size;
        }

        public int CalculateDamage(Player player)
        {
            return 1;
        }
    }
}
