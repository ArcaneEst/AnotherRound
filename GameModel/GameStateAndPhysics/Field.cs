using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AnotherRound
{
    /// <summary>
    /// Поле боя.
    /// </summary>
    public class Field
    {
        public Field(Vector playerStartPosition)
        {
            Player = new Player(playerStartPosition);
        }

        public Field()
        { }

        public int Speed { get; set; } = 5;
        public Size FieldSize { get; set; } = new Size(1200, 700);
        public Player Player { get; set; } = new Player(new Vector(100, 100));
        public List<Projectail> Projectails { get; set; } = new List<Projectail>();
        public List<Obstacle> Obstacles { get; set; } = new List<Obstacle>() 
        { new CircleObstacle(new Vector(300, 300), new Size(50, 50)),
        new CircleRemovable(new Vector(200,200), new Size(40, 40), 3),
        new SquareRemovable(new Vector(500, 500), new Size(100, 50), 10)};

        /// <summary>
        /// Проверка - находится ли объект за полем хотя бы частично.
        /// </summary>
        /// <param name="newLocation"></param>
        /// <param name="objectSize"></param>
        /// <returns></returns>
        private bool IsOutOfField(Vector newLocation, Size objectSize)
        {
            return newLocation.X > FieldSize.Width - objectSize.Width / 2 ||
                newLocation.X < objectSize.Width / 2 ||
                newLocation.Y > FieldSize.Height - objectSize.Height / 2 ||
                newLocation.Y < objectSize.Height / 2;
        }

        //Стрельба
        #region
        /// <summary>
        /// Выполняет действие всех объектов-пуль на поле.
        /// </summary>
        public void ExecuteAllProjectiles()
        {
            foreach (var proj in Projectails)
                ExecuteProjectile(proj);

            RemoveHitedProjectiles();
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
        private void RemoveHitedProjectiles()
        {
            Projectails = Projectails
                .Where(proj => !IsOutOfField(proj.Location, proj.Size))
                .Where(proj => !IsCollisionWithObstacle(proj))
                .ToList();
        }

        /// <summary>
        /// Проверяет, столкнулась ли пуля хотя бы с одним препятствием.
        /// </summary>
        /// <param name="proj">Проверяемая пуля</param>
        /// <returns></returns>
        private bool IsCollisionWithObstacle(Projectail proj)
        {
            foreach (var obstacle in Obstacles)
                if (Physics.CollisionTwoAbstract(proj, obstacle))
                {
                    DoDamageIfEnemy(obstacle);
                    return true;
                }

            return false;
        }

        private void DoDamageIfEnemy(Obstacle obstacle)
        {
            if (obstacle is IEnemy removable)
            {
                removable.GetHit();
                if (removable.IsDead)
                    Obstacles.Remove(obstacle);
            }
        }

        /// <summary>
        /// Просит объект игрока попробовать сгенерировать новую пулю. Если объект игрока не может выстрелить,
        /// null, пуля не появляется.
        /// </summary>
        /// <param name="controlX">Обработанный ввод от игрока по оси x</param>
        /// <param name="controlY">Обработанный ввод от игрока по оси y</param>
        internal void TryShoot(Direction controlX, Direction controlY)
        {
            var newProjectile = Player.TryGenerateProjectile(controlX, controlY);

            if (newProjectile is null)
                return;

            Projectails.Add(newProjectile);
        }
        #endregion

        //Движение персонажа игрока.
        #region
        /// <summary>
        /// Пробует передвинуть объект игрока по вводу игрока.
        /// </summary>
        /// <param name="controlX">Обработанный ввод по оси х</param>
        /// <param name="controlY">Обработанный ввод по оси y</param>
        internal void MovePlayer(Direction controlX, Direction controlY)
        {
            var move = MoveInDirectionUsingVector(controlX, controlY);
            var newPlayer = new CircleObstacle(Player.Location.Copy() + move, Player.Size);

            foreach (var obstacle in Obstacles)
            {
                if (Physics.CollisionTwoAbstract(newPlayer, obstacle))
                    move = Player.ReactOnCollishion(obstacle, move);
            }

            ApplyMoveWithCheckFieldSize(move);
        }

        /// <summary>
        /// Возвращает вектор движения игрока по обработанному вводу.
        /// </summary>
        /// <param name="controlX">Обработанный ввод по оси x</param>
        /// <param name="controlY">Обработанный ввод по оси y</param>
        /// <returns></returns>
        private Vector MoveInDirectionUsingVector(Direction controlX, Direction controlY)
        {
            var xMove = controlX < 0 ? -1 : controlX > 0 ? 1 : 0;
            var yMove = controlY < 0 ? -1 : controlY > 0 ? 1 : 0;

            return new Vector(Speed * xMove, Speed * yMove);
        }

        /// <summary>
        /// Проверяет, находится ли объект игрока за полем, после чего применяет движение, если возможно.
        /// </summary>
        /// <param name="move">Вектор движения объекта игрока.</param>
        private void ApplyMoveWithCheckFieldSize(Vector move)
        {
            var newX = Player.Location.X + move.X;
            var newY = Player.Location.Y + move.Y;
            var moveX = move.X;
            var moveY = move.Y;

            if (newX > FieldSize.Width - Player.Size.Width / 2)
                moveX = (FieldSize.Width - Player.Size.Width / 2) - Player.Location.X;

            if (newX < Player.Size.Width / 2)
                moveX = Player.Location.X - Player.Size.Width / 2;

            if (newY > FieldSize.Height - Player.Size.Height / 2)
                moveY = (FieldSize.Height - Player.Size.Height / 2) - Player.Location.Y;

            if (newY < Player.Size.Height / 2)
                moveY = Player.Location.Y - Player.Size.Height / 2;

            Player.ApplyMove(new Vector(moveX, moveY));
        }
        #endregion
    }
}