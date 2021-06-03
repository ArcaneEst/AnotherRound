using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AnotherRound
{
    public class ProjectileList
    {
        public List<Projectail> Projectails { get; set; } = new List<Projectail>();
        
        public ProjectileList()
        {

        }

        /// <summary>
        /// Выполняет действие всех объектов-пуль на поле.
        /// </summary>
        public void ExecuteAllProjectiles(Size FieldSize, MapObjectsVault obstacleVault)
        {
            foreach (var proj in Projectails)
                ExecuteProjectile(proj);

            RemoveHitedProjectiles(FieldSize, obstacleVault);
        }

        /// <summary>
        /// Расчитывает новое положение пули и выполняет ее действие.
        /// </summary>
        /// <param name="projectile">Выполняемая пуля</param>
        private void ExecuteProjectile(Projectail projectile)
        {
            var newProjectileLocation = projectile.CalculateNewLocation();

            projectile.Location = newProjectileLocation;
        }

        /// <summary>
        /// Убирает из списка пуль все те, что находятся за полем или куда-то попали.
        /// </summary>
        private void RemoveHitedProjectiles(Size FieldSize, MapObjectsVault obstacleVault)
        {
            Projectails = Projectails
                .Where(proj => !IsOutOfField(proj.Location, proj.Size, FieldSize))
                .Where(proj => !IsCollisionWithObstacle(proj, obstacleVault.GetAllObstacles()))
                .ToList();
        }

        /// <summary>
        /// Проверяет, столкнулась ли пуля хотя бы с одним препятствием.
        /// </summary>
        /// <param name="proj">Проверяемая пуля</param>
        /// <returns></returns>
        private bool IsCollisionWithObstacle(Projectail proj, IEnumerable<Obstacle> Obstacles)
        {
            foreach (var obstacle in Obstacles)
                if (Physics.CollisionTwoAbstract(proj, obstacle))
                {
                    DoDamageIfCanBeDamaged(obstacle, Obstacles);
                    return true;
                }

            return false;
        }

        private void DoDamageIfCanBeDamaged(Obstacle obstacle, IEnumerable<Obstacle> Obstacles)
        {
            if (obstacle is ICanBeDamaged removable)
            {
                removable.GetHit();
            }
        }

        /// <summary>
        /// Просит объект игрока попробовать сгенерировать новую пулю. Если объект игрока не может выстрелить,
        /// null, пуля не появляется.
        /// </summary>
        /// <param name="controlX">Обработанный ввод от игрока по оси x</param>
        /// <param name="controlY">Обработанный ввод от игрока по оси y</param>
        internal void TryShoot(Direction controlX, Direction controlY, Player player)
        {
            var newProjectile = player.TryGenerateProjectile(controlX, controlY);

            if (newProjectile is null)
                return;

            Projectails.Add(newProjectile);
        }

        private bool IsOutOfField(Vector newLocation, Size objectSize, Size FieldSize)
        {
            return newLocation.X > FieldSize.Width - objectSize.Width / 2 ||
                newLocation.X < objectSize.Width / 2 ||
                newLocation.Y > FieldSize.Height - objectSize.Height / 2 ||
                newLocation.Y < objectSize.Height / 2;
        }
    }
}
