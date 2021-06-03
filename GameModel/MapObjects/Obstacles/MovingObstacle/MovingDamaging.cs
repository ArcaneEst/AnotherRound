using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class MovingDamaging : CircleObstacle, ICanMove, ICanDamage
    {
        private Vector UpBound = new Vector(600, 200);
        private Vector DownBound = new Vector(600, 400);
        private bool MoveUp = true;
        public new Size Size { get; set; }
        public MovingDamaging(Vector location, Size size) : base(location, size)
        {
            Location = location;
            Size = size;
        }

        public int Speed { get; set; } = 10;

        public Vector GetMoveVector(MapObjectsVault Obstacles)
        {
            if (Location.Y - UpBound.Y < 0)
                MoveUp = false;

            if (Location.Y - DownBound.Y > 0)
                MoveUp = true;

            var moveVector = new Vector(0, Speed * (MoveUp ? -1 : 1));

            return moveVector;
        }

        public int CalculateDamage(Player player)
        {
            return 1;
        }

        public ICanMove GetCopyWithNewLocation(Vector location)
        {
            throw new NotImplementedException();
        }

        public Vector ReactOnCollision(Obstacle obstacle, Vector move)
        {
            throw new NotImplementedException();
        }
    }
}
