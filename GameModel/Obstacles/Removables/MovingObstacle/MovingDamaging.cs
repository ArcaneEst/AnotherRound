using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class MovingDamaging : CircleObstacle, ICanMove, ICanDamage
    {
        public new Vector Location { get; set; }
        public new Size Size { get; set; }
        public MovingDamaging(Vector location, Size size) : base(location, size)
        {

        }

        public int Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Vector GetMoveVector()
        {
            throw new NotImplementedException();
        }

        public int CalculateDamage(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
