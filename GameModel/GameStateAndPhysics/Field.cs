using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AnotherRound
{
    public class Field
    {
        public delegate void GameEnded();
        public event GameEnded EndGameEvent;
        public Field(Vector playerStartPosition)
        {
            Player = new Player(playerStartPosition);
        }

        public Field()
        { }
        public Size FieldSize { get; set; } = new Size(1200, 700);
        public Player Player { get; set; } = new Player(new Vector(100, 100));
        public ProjectileList Projectails { get; set; } = new ProjectileList();
        public List<Obstacle> Obstacles { get; set; } = new List<Obstacle>() 
        { new CircleObstacle(new Vector(500, 540), new Size(50, 50)),
        new RemovableDamaging(new Vector(200,200), new Size(40, 40), 3),
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
            Projectails.ExecuteAllProjectiles(FieldSize, Obstacles);
        }

        public void TryShoot(Direction controlX, Direction controlY)
        {
            Projectails.TryShoot(controlX, controlY, Player);
        }
        #endregion

        //Движение персонажа игрока, проверка на смерть.
        #region
        /// <summary>
        /// Пробует передвинуть объект игрока по вводу игрока. Проверяет игрока на окончание игры.
        /// </summary>
        /// <param name="controlX">Обработанный ввод по оси х</param>
        /// <param name="controlY">Обработанный ввод по оси y</param>
        internal void MovePlayer(Direction controlX, Direction controlY)
        {
            Player.DoMoveWithActivatingCollisions(controlX, controlY, Obstacles, FieldSize);

            if (Player.IsDead)
                EndGameEvent.Invoke();
        }
        #endregion
    }
}