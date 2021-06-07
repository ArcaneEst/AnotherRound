using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AnotherRound
{
    public class Field
    {
        private Dictionary<int, Func<ObstaclesVault>> LevelGenerator = 
            new Dictionary<int, Func<ObstaclesVault>>() 
            {
                {0, PrototypeLevel.GenerateProrotypeLevel },
                {1, SurvivalLevel.GenerateSurvival },
                {2, TestLevel.GenerateTestLevel }
            };

        public delegate void GameEnded(string reason);
        public event GameEnded EndGameEvent;
        public Field(int level)
        {
            ObjectsVault.Player = new Player(new Vector(50, 50), new Size(50, 50));
            ObjectsVault = LevelGenerator[level]();
        }

        public Size FieldSize { get; private set; } = new Size(1200, 700);
        public Player Player { get => ObjectsVault.Player; } 
        public ProjectileList Projectails { get; private set; } = new ProjectileList();
        public ObstaclesVault ObjectsVault { get; private set; } = SurvivalLevel.GenerateSurvival();

        public void GameTick(ControlCommand moveCommand, ControlCommand shootCommand)
        {
            ExecuteAllProjectiles();
            ObjectsVault.ExecuteVault(FieldSize);
            TryShoot(shootCommand);
            MovePlayer(moveCommand);

            if (Player.IsDead)
                EndGameEvent.Invoke("Dead");

            if (Player.IsWinGame)
                EndGameEvent.Invoke("Win");
        }

        //Методы вызова компонентов игры.
        #region
        //Стрельба
        #region
        /// <summary>
        /// Выполняет действие всех объектов-пуль на поле.
        /// </summary>
        private void ExecuteAllProjectiles()
        {
            Projectails.ExecuteAllProjectiles(FieldSize, ObjectsVault);
        }

        private void TryShoot(ControlCommand shootCommand)
        {
            Projectails.TryShoot(shootCommand.X, shootCommand.Y, Player);
        }
        #endregion

        //Движение персонажа игрока, проверка на смерть.
        #region
        /// <summary>
        /// Пробует передвинуть объект игрока по вводу игрока. Проверяет игрока на окончание игры.
        /// </summary>
        /// <param name="controlX">Обработанный ввод по оси х</param>
        /// <param name="controlY">Обработанный ввод по оси y</param>
        private void MovePlayer(ControlCommand moveCommand)
        {
            Player.DoPlayerMove(moveCommand.X, moveCommand.Y, ObjectsVault, FieldSize);
        }
        #endregion
        #endregion
    }
}