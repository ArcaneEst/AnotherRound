using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class MovingDamaging : CircleObstacle, ICanMove, ICanDamage
    {
        private readonly Vector firstPoint;
        private readonly Vector secondPoint;
        private bool moveToSecond = true;
        private readonly double angleFirstToSecond;
        public int Speed { get; set; } = 10;
        public new Size Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">Спавн</param>
        /// <param name="first">Первая точка-граница</param>
        /// <param name="second">Вторая точка-граница</param>
        /// <param name="size">Размер объекта</param>
        public MovingDamaging(Vector location, Vector first, Vector second,Size size) : base(location, size)
        {
            Location = location;
            Size = size;
            firstPoint = first;
            secondPoint = second;
            angleFirstToSecond = firstPoint.AngleTo(secondPoint);
        }

        public MovingDamaging(Vector first, Vector second, Size size, int speed = 10) 
             : base(new Vector((first.X + second.X) / 2, (first.Y + second.Y) / 2), size)
        {
            Location = new Vector((first.X + second.X) / 2, (first.Y + second.Y) / 2);
            firstPoint = first;
            secondPoint = second;
            Size = size;
            angleFirstToSecond = firstPoint.AngleTo(secondPoint);
            Speed = speed;
        }

        public Vector GetMoveVector(ObstaclesVault Obstacles)
        {

            var pointToMove = moveToSecond ?  firstPoint : secondPoint;

            if (Location.ModOfDistanceTo(pointToMove) > firstPoint.ModOfDistanceTo(secondPoint))
                moveToSecond = !moveToSecond;

            var yMove = (int)Math.Round(Speed * Math.Sin(angleFirstToSecond) * (moveToSecond ? 1 : -1));
            var xMove = (int)Math.Round(Speed * Math.Cos(angleFirstToSecond) * (moveToSecond ? 1 : -1));

            var moveVector = new Vector(xMove, yMove);

            return moveVector;
        }

        public int CalculateDamage(Player player)
        {
            return 1;
        }

        public ICanMove GetCopyWithNewLocation(Vector location)
        {
            return new MovingDamaging(location, firstPoint, secondPoint, Size);
        }

        public Vector ReactOnCollision(Obstacle obstacle, Vector move)
        {
            return move;
        }
    }
}
