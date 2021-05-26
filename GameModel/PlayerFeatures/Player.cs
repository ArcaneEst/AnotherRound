using System;
using System.Drawing;

namespace AnotherRound
{
    /// <summary>
    /// Класс персонажа игрока.
    /// </summary>
    public class Player : ICollishiable, IAbleToMove, ICircle
    {
        public Vector Location { get; set; }
        public Size Size { get; set; } = new Size(50, 50);

        public double Radius => Size.Width / 2;

        public Weapon weapon = new Weapon(10, new Size(15, 15));

        public Player(Vector spawnPoint)
        {
            Location = spawnPoint;
        }

        internal Projectail TryGenerateProjectile(Direction controlX, Direction controlY)
        {
            var projectile = weapon.TryGenerateProjectile(controlX, controlY);

            if (projectile is null)
                return null;

            projectile.Location = new Vector(Location.X, Location.Y);
            return projectile;
        }

        public void ApplyMove(Vector move)
        {
            Location.X += move.X;
            Location.Y += move.Y;
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
    }
}
