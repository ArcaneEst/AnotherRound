using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AnotherRound
{
    public class Physics
    {
        /// <summary>
        /// Проверка столкновения двух кругов.
        /// </summary>
        /// <param name="initiator">Объект, который двигается</param>
        /// <param name="obstacle">Объект, с которым сталкиваются.</param>
        /// <param name="move">Вектор движения инициатора.</param>
        /// <returns></returns>
        public static bool CollisionTwoСircles(ICircle initiator, ICircle obstacle, Vector move)
        {
            var newPosition = initiator.Location + move;
            var space = initiator.Size.Width / 2 + obstacle.Size.Width / 2;

            return newPosition.ModOfDistanceTo(obstacle.Location) <= space;
        }

        /// <summary>
        /// Проверка столкновения двух прямоугольников.
        /// </summary>
        /// <param name="initiator">Объект, который двигается.</param>
        /// <param name="obstacle">Объект, с которым сталкиваются.</param>
        /// <param name="move">Вектор движения инициатора.</param>
        /// <returns></returns>
        public static bool CollisionTwoSquares(ISquare initiator, ISquare obstacle, Vector move)
        {
            var newPositionMin = initiator.MinPoints + move;
            var newPositionMax = initiator.MaxPoints + move;

            if (newPositionMax.X < obstacle.MinPoints.X || newPositionMin.X > obstacle.MaxPoints.X) return false;
            if (newPositionMax.Y < obstacle.MinPoints.Y || newPositionMin.Y > obstacle.MaxPoints.Y) return false;

            return true;
        }

        public static bool CollisionDifferentForms(ISquare initiator, ICircle obstacle, Vector move)
        {
            var newLocation = initiator.Location + move;
            var newSquare = new SquareObstacle(newLocation, initiator.Size);
            if (IsLocationBetweenBounds(newLocation, newSquare.MinPoints, newSquare.MaxPoints))
                return CollisionTwoSquares(newSquare, new SquareObstacle(obstacle.Location, obstacle.Size), Vector.Zero);
            else
                return CollisionTwoСircles(GenerateCircleFromSquare(initiator), obstacle, move);
        }

        public static bool CollisionDifferentForms(ICircle initiator, ISquare obstacle, Vector move)
        {
            var newLocation = initiator.Location + move;

            if (IsLocationBetweenBounds(newLocation, obstacle.MinPoints, obstacle.MaxPoints))
                return CollisionTwoSquares(new SquareObstacle(newLocation, initiator.Size), obstacle, Vector.Zero);
            else
                return CollisionTwoСircles(initiator, GenerateCircleFromSquare(obstacle), move);
        }

        public static bool IsLocationBetweenBounds(Vector location, Vector minBounds, Vector maxBounds)
        {
            return (location.X >= minBounds.X && location.X <= maxBounds.X) ||
                (location.Y >= minBounds.Y && location.Y <= maxBounds.Y);
        }

        private static ICircle GenerateCircleFromSquare(ISquare square)
        {
            var radious = square.MinPoints.Length;
            var size = new Size((int)Math.Round(radious), (int)Math.Round(radious));

            return new CircleObstacle(square.Location, size);
        }
    }

    [TestFixture]
    class TestPhysics
    {
        [TestCase(100, 100, 150, 100, 20, 0, true)] // горизонтальное движение вправо, встанут вплонтую
        [TestCase(200, 100, 150, 100, -20, 0, true)] // горизонтальное движение влево, встанут вплотную
        [TestCase(100, 170, 100, 120, 0, -20, true)] // вертикально движение вверх, встанут вплотную
        [TestCase(100, 70, 100, 120, 0, 20, true)] // вертикальное движение вниз, встанут вплотную
        [TestCase(115, 40, 80, 70, -10, 10, false)] // влево вниз, встанут недалеко друг от друга по диагонали
        [TestCase(115, 40, 80, 70, -10, 15, true)] // влево вниз, коллизия
        [TestCase(115, 40, 80, 70, -15, 10, true)] // влево вниз, коллизия
        public void CollitionTwoSyrclesTest(
            int firstX, int firstY, int secondX, int secondY, int moveX, int moveY, bool predicate)
        {
            var first = new CircleObstacle(new Vector(firstX, firstY), new Size(30, 30));
            var second = new CircleObstacle(new Vector(secondX, secondY), new Size(30, 30));
            var move = new Vector(moveX, moveY);

            var isCollision = Physics.CollisionTwoСircles(first, second, move);

            Assert.AreEqual(predicate, isCollision);
        }

        [TestCase(100, 100, 140, 100, 9, 0, false)] // разделяются 1 пикселем
        [TestCase(100, 100, 100, 140, 0, 9, false)] // разделяются 1 пикселем
        [TestCase(100, 100, 60, 100, -9, 0, false)] // разделяются 1 пикселем
        [TestCase(100, 100, 100, 60, 0, -9, false)] // разделяются 1 пикселем
        [TestCase(100, 100, 140, 100, 10, 0, true)] // пересечение - отрезок в 1 пиксель
        [TestCase(100, 100, 100, 140, 0, 10, true)] // пересечение - отрезок в 1 пиксель
        [TestCase(100, 100, 60, 100, -10, 0, true)] // пересечение - отрезок в 1 пиксель
        [TestCase(100, 100, 100, 60, 0, -10, true)] // пересечение - отрезок в 1 пиксель
        [TestCase(100, 100, 130, 130, 0, 0, true)] //соприкосновение уголками
        [TestCase(100, 100, 70, 70, 0, 0, true)] //соприкосновение уголками
        [TestCase(100, 100, 130, 70, 0, 0, true)] //соприкосновение уголками
        [TestCase(100, 100, 70, 130, 0, 0, true)] //соприкосновение уголками
        public void CollitionTwoSquaresTest(
            int firstX, int firstY, int secondX, int secondY, int moveX, int moveY, bool predicate)
        {
            var first = new SquareObstacle(new Vector(firstX, firstY), new Size(30, 30));
            var second = new SquareObstacle(new Vector(secondX, secondY), new Size(30, 30));
            var move = new Vector(moveX, moveY);

            var isCollision = Physics.CollisionTwoSquares(first, second, move);

            Assert.AreEqual(predicate, isCollision);
        }

        [TestCase(100, 100, 100, 140, 0, 10, true, true)] // круг вертикально над квадратом, движение вниз.
        [TestCase(100, 100, 140, 100, 10, 0, true, true)] // круг слева от квадрата, движение вправо
        [TestCase(100, 100, 100, 60, 0, -10, true, true)] // круг снизу от квадрата, движение вверх.
        [TestCase(100, 100, 60, 100, -10, 0, true, true)] // круг справа от квадрата, движение влево.
        [TestCase(100, 100, 130, 130, 5, 5, true, false)] // круг слева сверху от квадрата, пересечение с углом
        [TestCase(100, 100, 70, 130, -5, 5, true, false)] // круг справа сверху от квадртата, пересечение с углом
        [TestCase(100, 100, 70, 70, -5, -5, true, false)] // круг справа снизу от квадрата, пересечение с углом
        [TestCase(100, 100, 70, 130, -5, 5, true, false)] // круг слева снизу от квадрата, пересечение с углом.
        [TestCase(100, 100, 135, 130, 5, 0, true, true)] // круг слева от угла квадрата, центр напротив ребра квадрата
        [TestCase(100, 100, 130, 135, 0, 5, true, true)] // круг сверху от угла квадрата, центр напротив ребра
        public void CollisionCyrcleSquareTest(
            int firstX, int firstY, int secondX, int secondY, int moveX, int moveY, 
            bool predicate, bool BetweenBordersResult)
        {
            var circle = new CircleObstacle(new Vector(firstX, firstY), new Size(30, 30));
            var square = new SquareObstacle(new Vector(secondX, secondY), new Size(30, 30));
            var move = new Vector(moveX, moveY);
            var newLocation = circle.Location + move;

            var isCollision = Physics.CollisionDifferentForms(circle, square, move);
            var betweenBordsResult = Physics.IsLocationBetweenBounds(newLocation, square.MinPoints, square.MaxPoints);

            Assert.AreEqual(BetweenBordersResult, betweenBordsResult);
            Assert.AreEqual(predicate, isCollision);
        }
    }
}
