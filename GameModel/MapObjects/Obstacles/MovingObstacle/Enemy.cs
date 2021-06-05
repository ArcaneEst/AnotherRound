using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class Enemy : CircleObstacle, IMapObject, ICanMove, ICanDamage, ICanBeDamaged
    {
        public int Speed { get; set; } = 3;
        public int HealthPoints { get; set; } = 3;
        public bool IsDead => HealthPoints <= 0;

        public Enemy(Vector location, Size size) : base(location, size)
        {
            Location = location;
            Size = size;
        }

        public Vector GetMoveVector(ObstaclesVault mapObjects)
        {
            var angleToPlayer = Location.AngleTo(mapObjects.Player.Location);

            return new Vector((int)Math.Ceiling(Math.Cos(angleToPlayer) * Speed), 
                (int)Math.Ceiling(Math.Sin(angleToPlayer) * Speed));
        }

        public ICanMove GetCopyWithNewLocation(Vector location)
        {
            return new Enemy(location, Size);
        }

        public Vector ReactOnCollision(Obstacle obstacle, Vector move)
        {
            if (obstacle is Player player)
                player.TryHit(this);

            if (obstacle is ISquare squareObstacle)
                return ReactOnCollishion(squareObstacle, move);
            else if (obstacle is ICircle circleObstacle)
                return ReactOnCollishion(circleObstacle, move);
            else throw new ArgumentException();
        }

        public Vector ReactOnCollishion(ICircle obstacle, Vector move)
        {
            if (move.X == 0)
                return CircleSlipX(obstacle, move);
            if (move.Y == 0)
                return CircleSlipY(obstacle, move);

            return Vector.Zero;
        }

        public Vector CircleSlipX(ICircle obstacle, Vector move)
        {
            var spaceBetweenCentres = Size.Width / 2 + obstacle.Size.Width / 2;

            var newY = Location.Y + move.Y;
            var newDeltaY = obstacle.Location.Y - newY;

            var newXMod = (int)Math.Sqrt(spaceBetweenCentres * spaceBetweenCentres - newDeltaY * newDeltaY) + 1;
            var deltaXForMin = obstacle.Location.X - newXMod - Location.X + move.X;
            var deltaXForMax = obstacle.Location.X + newXMod - Location.X + move.X;
            var toGo = 0;

            try
            {
                toGo = Math.Abs(deltaXForMin) < Math.Abs(deltaXForMax) ? deltaXForMin : deltaXForMax;
            } catch (OverflowException) { }

            return new Vector(toGo, move.Y);
        }

        public Vector CircleSlipY(ICircle obstacle, Vector move)
        {
            var spaceBetweenCentres = Size.Width / 2 + obstacle.Size.Width / 2;

            var newX = Location.X + move.X;
            var newDeltaX = obstacle.Location.X - newX;

            var newXMod = (int)Math.Sqrt(spaceBetweenCentres * spaceBetweenCentres - newDeltaX * newDeltaX) + 1;
            var deltaYForMin = obstacle.Location.Y - newXMod - Location.Y + move.Y;
            var deltaYForMax = obstacle.Location.Y + newXMod - Location.Y + move.Y;

            var toGo = 0;

            try { 
                toGo = Math.Abs(deltaYForMin) < Math.Abs(deltaYForMax) ? deltaYForMin : deltaYForMax;
            } catch (OverflowException) { }

            return new Vector(move.X, toGo);
        }

        public Vector ReactOnCollishion(ISquare obstacle, Vector move)
        {
            var minX = obstacle.MinPoints.X - Size.Width / 2 - Location.X;
            var maxX = obstacle.MaxPoints.X + Size.Width / 2 - Location.X;
            var deltaX = Math.Abs(minX) < Math.Abs(maxX) ? minX : maxX;

            var minY = obstacle.MinPoints.Y - Location.Y - Size.Height / 2;
            var maxY = obstacle.MaxPoints.Y - Location.Y + Size.Height / 2;
            var deltaY = Math.Abs(minY) < Math.Abs(maxY) ? minY : maxY;

            if (Math.Abs(deltaX) > 1) deltaX = move.X;
            if (Math.Abs(deltaY) > 1) deltaY = move.Y;
            return new Vector(deltaX, deltaY);
        }

        public int CalculateDamage(Player player)
        {
            return 1;
        }

        public void GetHit()
        {
            HealthPoints--;
        }
    }
}
