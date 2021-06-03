using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public static class PlayerMovement
    {
        /// <summary>
        /// Пробует передвинуть объект игрока по вводу игрока.
        /// </summary>
        /// <param name="controlX">Нажатая кнопка по x</param>
        /// <param name="controlY">Нажатая кнопка по y</param>
        /// <param name="obstacleVault">Список препятствий</param>
        /// <param name="player">Объект игрока</param>
        /// <param name="fieldSize">Размер поля</param>
        public static Vector CalculateMoveVector(
            Direction controlX, 
            Direction controlY, 
            MapObjectsVault obstacleVault, 
            Player player, 
            Size fieldSize)
        {
            var move = MoveInDirectionUsingVector(controlX, controlY, player.Speed);
            var newPlayer = new CircleObstacle(player.Location.Copy() + move, player.Size);

            foreach (var obstacle in obstacleVault.GetAllObstacles())
            {
                if (Physics.CollisionTwoAbstract(newPlayer, obstacle))
                    move = player.ReactOnCollishion(obstacle, move);
            }

            return CheckMoveWithFieldBounds(move, fieldSize, player);
        }

        /// <summary>
        /// Возвращает вектор движения игрока по обработанному вводу.
        /// </summary>
        /// <param name="controlX">Обработанный ввод по оси x</param>
        /// <param name="controlY">Обработанный ввод по оси y</param>
        /// <returns></returns>
        public static Vector MoveInDirectionUsingVector(Direction controlX, Direction controlY, int speed)
        {
            var xMove = controlX < 0 ? -1 : controlX > 0 ? 1 : 0;
            var yMove = controlY < 0 ? -1 : controlY > 0 ? 1 : 0;

            return new Vector(speed * xMove, speed * yMove);
        }

        /// <summary>
        /// Проверяет, находится ли объект игрока за полем, после чего применяет движение, если возможно.
        /// </summary>
        /// <param name="move">Вектор движения объекта игрока.</param>
        public static Vector CheckMoveWithFieldBounds(Vector move, Size fieldSize, Player player)
        {
            var newX = player.Location.X + move.X;
            var newY = player.Location.Y + move.Y;
            var moveX = move.X;
            var moveY = move.Y;

            if (newX > fieldSize.Width - player.Size.Width / 2)
                moveX = (fieldSize.Width - player.Size.Width / 2) - player.Location.X;

            if (newX < player.Size.Width / 2)
                moveX = player.Location.X - player.Size.Width / 2;

            if (newY > fieldSize.Height - player.Size.Height / 2)
                moveY = (fieldSize.Height - player.Size.Height / 2) - player.Location.Y;

            if (newY < player.Size.Height / 2)
                moveY = player.Location.Y - player.Size.Height / 2;

            return new Vector(moveX, moveY);
        }
    }
}
