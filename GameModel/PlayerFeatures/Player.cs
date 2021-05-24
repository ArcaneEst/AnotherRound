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
    }
}
