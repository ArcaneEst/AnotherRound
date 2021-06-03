using System;
using System.Collections.Generic;
using System.Drawing;

namespace AnotherRound
{
    /// <summary>
    /// Класс персонажа игрока.
    /// </summary>
    public class Player : CircleObstacle, ICircle, ICanMove
    {
        public Weapon weapon = new Weapon(10, new Size(15, 15));
        public int HealthPoints { get; set; } = 3;
        public bool IsDead => HealthPoints <= 0;
        public bool IsCanBeHited { get; set; } = true;
        public int Speed { get; set; } = 5;

        public Player(Vector spawnPoint, Size size) : base(spawnPoint, size)
        {
            Location = spawnPoint;
            Size = size;
        }

        internal Projectail TryGenerateProjectile(Direction controlX, Direction controlY)
        {
            var projectile = weapon.TryGenerateProjectile(controlX, controlY);

            if (projectile is null)
                return null;

            projectile.Location = new Vector(Location.X, Location.Y);
            return projectile;
        }

        public void DoPlayerMove(
            Direction controlX, Direction controlY, MapObjectsVault obstacles, Size fieldSize)
        {
            var toMove = CalcMoveVectorByDirections(controlX, controlY, Speed);
            toMove = Movement.CalculateMoveWithCollision(this, obstacles, fieldSize, toMove);
            ApplyMove(toMove);
        }

        public void ApplyMove(Vector move)
        {
            Location.X += move.X;
            Location.Y += move.Y;
        }

        public Vector ReactOnCollision(Obstacle obstacle, Vector move)
        {
            if (obstacle is ICanDamage damageDiller)
                TryHit(damageDiller);

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

            var toGo = Math.Abs(deltaXForMin) < Math.Abs(deltaXForMax) ? deltaXForMin : deltaXForMax;

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

            var toGo = Math.Abs(deltaYForMin) < Math.Abs(deltaYForMax) ? deltaYForMin : deltaYForMax;

            return new Vector(move.X, toGo);
        }

        public Vector ReactOnCollishion(ISquare obstacle, Vector move)
        {
            var minX = obstacle.MinPoints.X - Size.Width / 2 - Location.X;
            var maxX = obstacle.MaxPoints.X + Size.Width / 2 - Location.X;
            var deltaX = Math.Abs(minX) < Math.Abs(maxX) ? minX : maxX;

            var minY = obstacle.MinPoints.Y - Location.Y - Size.Height / 2;
            var maxY = obstacle.MaxPoints.Y - Location.Y + Size.Height / 2;
            var deltaY =Math.Abs(minY) < Math.Abs(maxY) ? minY : maxY;

            if (Math.Abs(deltaX) > 5) deltaX = move.X;
            if (Math.Abs(deltaY) > 5) deltaY = move.Y;
            return new Vector(deltaX, deltaY);
        }

        public void DealDamage(int damage)
        {
            HealthPoints -= damage;
        }

        public void TryHit(ICanDamage damageDiller)
        {
            if (IsCanBeHited)
            {
                var damage = damageDiller.CalculateDamage(this);
                DealDamage(damage);
                Immortality(1000);
            }
        }

        public void Immortality(int time)
        {
            IsCanBeHited = false;

            var timer = new System.Timers.Timer(time);
            timer.Elapsed += (sender, args) => { IsCanBeHited = true; };
            timer.AutoReset = false;
            timer.Start();
        }

        /// <summary>
        /// Возвращает вектор движения игрока по обработанному вводу.
        /// </summary>
        /// <param name="controlX">Обработанный ввод по оси x</param>
        /// <param name="controlY">Обработанный ввод по оси y</param>
        /// <returns></returns>
        public static Vector CalcMoveVectorByDirections(Direction controlX, Direction controlY, int speed)
        {
            var xMove = controlX < 0 ? -1 : controlX > 0 ? 1 : 0;
            var yMove = controlY < 0 ? -1 : controlY > 0 ? 1 : 0;

            return new Vector(speed * xMove, speed * yMove);
        }

        public Vector GetMoveVector(MapObjectsVault MapObjects)
        {
            throw new NotImplementedException();
        }

        public ICanMove GetCopyWithNewLocation(Vector location)
        {
            return new Player(location, Size);
        }
    }
}
