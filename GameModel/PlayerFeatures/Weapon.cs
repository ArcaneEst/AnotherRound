using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace AnotherRound
{
    public class Weapon
    {
        int Speed;
        Size Size;
        int cooldown = 100;
        bool isAbleToShoot = true;
        
        public Weapon(int speed, Size size)
        {
            Speed = speed;
            Size = size;
        }

        public Projectail TryGenerateProjectile(Direction controlX, Direction controlY)
        {
            if (isAbleToShoot == false)
                return null;

            var projectile = new Projectail(Speed, Size);
            var angle = CalculateDirectionAngle(controlX, controlY);

            if (angle == null)
                return null;

            isAbleToShoot = false;

            projectile.DirectionAngle = angle.Value;
            isAbleToShoot = false;

            var timer = new System.Timers.Timer(cooldown);
            timer.Elapsed += (sender, args) => { Cooldown(); };
            timer.AutoReset = false;
            timer.Start();

            return projectile;
        }

        private void Cooldown()
        {
            isAbleToShoot = true;
        }

        private double? CalculateDirectionAngle(Direction controlX, Direction controlY)
        {
            var directionVector = new Vector(
                controlX > 0 ? 1 : controlX < 0 ? -1 : 0,
                controlY > 0 ? 1 : controlY < 0 ? -1 : 0);

            if (directionVector.X == 0 && directionVector.Y == 0)
                return null;

            return Math.Atan2(directionVector.Y, directionVector.X);
        }
    }
}
