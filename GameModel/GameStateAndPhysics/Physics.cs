using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FluentAssertions;

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
        public static bool CollisionTwoForms(ICircle initiator, ICircle obstacle, Vector move)
        {
            var newPosition = initiator.Location + move;
            var space = initiator.Size.Width / 2 + obstacle.Size.Width / 2;

            return newPosition.ModOfDistanceTo(obstacle.Location) <= space; // -5, чтобы не цепляться за углы
        }

        /// <summary>
        /// Проверка столкновения двух прямоугольников.
        /// </summary>
        /// <param name="initiator">Объект, который двигается.</param>
        /// <param name="obstacle">Объект, с которым сталкиваются.</param>
        /// <param name="move">Вектор движения инициатора.</param>
        /// <returns></returns>
        public static bool CollisionTwoForms(ISquare initiator, ISquare obstacle, Vector move)
        {
            var newPositionMin = initiator.MinPoints + move;
            var newPositionMax = initiator.MaxPoints + move;

            if (newPositionMax.X < obstacle.MinPoints.X || 
                newPositionMin.X > obstacle.MaxPoints.X) return false;
            if (newPositionMax.Y < obstacle.MinPoints.Y || 
                newPositionMin.Y > obstacle.MaxPoints.Y) return false;

            return true;
        }

        /// <summary>
        /// Проверка столкновения квадрата и круга
        /// </summary>
        /// <param name="initiator">Инициатор-квадрат</param>
        /// <param name="obstacle">Препятствие-круг</param>
        /// <param name="move">Вектор движения инициатора</param>
        /// <returns></returns>
        public static bool CollisionTwoForms(ISquare initiator, ICircle obstacle, Vector move)
        {
            var newLocation = initiator.Location + move;
            var newSquare = new SquareObstacle(newLocation, initiator.Size);
            if (IsLocationBetweenBounds(newLocation, newSquare.MinPoints, newSquare.MaxPoints))
                return CollisionTwoForms(newSquare, new SquareObstacle(obstacle.Location, obstacle.Size), Vector.Zero);
            else
                return CollisionTwoForms(GenerateCircleFromSquare(initiator), obstacle, move);
        }

        /// <summary>
        /// Проверка столкновения круга и квадрата
        /// </summary>
        /// <param name="initiator">Инициатор-круг</param>
        /// <param name="obstacle">Препятствие-квадрат</param>
        /// <param name="move">Вектор движения инициатора</param>
        /// <returns></returns>
        public static bool CollisionTwoForms(ICircle initiator, ISquare obstacle, Vector move)
        {
            var newLocation = initiator.Location + move;

            if (IsLocationBetweenBounds(newLocation, obstacle.MinPoints, obstacle.MaxPoints))
                return CollisionTwoForms(new SquareObstacle(newLocation, initiator.Size), obstacle, Vector.Zero);
            else
                return CollisionTwoForms(initiator, GenerateCircleFromSquare(obstacle), move);
        }

        /// <summary>
        /// Проверка - находится ли локация между прямыми-ребрами квадрата по горизонтали или вертикали.
        /// </summary>
        /// <param name="location">Локация для проверки</param>
        /// <param name="minBounds">Свойство квадрата, координаты сторон-минимумов</param>
        /// <param name="maxBounds">Свойство квадрата, координаты сторон-максимумов</param>
        /// <returns></returns>
        public static bool IsLocationBetweenBounds(Vector location, Vector minBounds, Vector maxBounds)
        {
            return (location.X >= minBounds.X && location.X <= maxBounds.X) ||
                (location.Y >= minBounds.Y && location.Y <= maxBounds.Y);
        }

        /// <summary>
        /// Генерирует круг из квадрата, с радиусом, равным половине диагонали.
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        private static ICircle GenerateCircleFromSquare(ISquare square)
        {
            var radious = (square.MaxPoints - square.Location).Length * 2;
            var size = new Size((int)Math.Ceiling(radious), (int)Math.Ceiling(radious));

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

            var isCollision = Physics.CollisionTwoForms(first, second, move);

            isCollision.Should().Be(predicate);
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

            var isCollision = Physics.CollisionTwoForms(first, second, move);

            isCollision.Should().Be(predicate);
        }

        [TestCase(100, 100, 100, 100, true)] // локация с центром в квадрате
        [TestCase(100, 100, 80, 100, true)] // локация между y
        [TestCase(100, 100, 100, 80, true)] // локация между x
        [TestCase(100, 100, 85, 85, true)] // локация находится в углу квадрата
        [TestCase(100, 100, 84, 85, true)] // локация находится на один пиксель правее угла квадрата
        [TestCase(100, 100, 80, 85, true)] // локация находится на пять пикселей правее угла квадрата
        public void BetweenBordersTest
            (int firstX, int firstY, int secondX, int secondY, bool predicate)
        {
            var location = new Vector(firstX, firstY);
            var square = new SquareObstacle(new Vector(secondX, secondY), new Size(30, 30));
            var isBetween = Physics.IsLocationBetweenBounds(location, square.MinPoints, square.MaxPoints);

            isBetween.Should().Be(predicate);
        }

        [TestCase(100, 100, 100, 140, 0, 10, true)] // круг вертикально над квадратом, движение вниз.
        [TestCase(100, 100, 140, 100, 10, 0, true)] // круг слева от квадрата, движение вправо
        [TestCase(100, 100, 100, 60, 0, -10, true)] // круг снизу от квадрата, движение вверх.
        [TestCase(100, 100, 60, 100, -10, 0, true)] // круг справа от квадрата, движение влево.
        [TestCase(100, 100, 130, 130, 5, 5, true)] // круг слева сверху от квадрата, пересечение с углом
        [TestCase(100, 100, 70, 130, -5, 5, true)] // круг справа сверху от квадртата, пересечение с углом
        [TestCase(100, 100, 70, 70, -5, -5, true)] // круг справа снизу от квадрата, пересечение с углом
        [TestCase(100, 100, 70, 130, -5, 5, true)] // круг слева снизу от квадрата, пересечение с углом.
        [TestCase(100, 100, 120, 130, 5, 0, true)] // круг слева от угла квадрата, центр напротив ребра квадрата
        [TestCase(100, 100, 130, 120, 0, 5, true)] // круг сверху от угла квадрата, центр напротив ребра
        public void CollisionCircleSquareTest(
            int firstX, int firstY, int secondX, int secondY, int moveX, int moveY, 
            bool collisionPredicate)
        {
            var circle = new CircleObstacle(new Vector(firstX, firstY), new Size(30, 30));
            var square = new SquareObstacle(new Vector(secondX, secondY), new Size(30, 30));
            var move = new Vector(moveX, moveY);

            var isCollision = Physics.CollisionTwoForms(circle, square, move);

            isCollision.Should().Be(collisionPredicate);
        }

        [TestCase(100, 100, 135, 100, 5, 0, true)] // горизонтальное движение на одной высоте
        [TestCase(100, 100, 130, 130, 5, 5, true)] // диагональное движение, коснется углом
        [TestCase(100, 100, 135, 115, 5, 0, true)] // диагональное движение, коснется углом
        public void CollisionSquareCircle(int firstX, int firstY, int secondX, int secondY, int moveX, int moveY,
            bool collisionPredicate)
        {
            var square = new SquareObstacle(new Vector(firstX, firstY), new Size(30, 30));
            var circle = new CircleObstacle(new Vector(secondX, secondY), new Size(30, 30));
            var move = new Vector(moveX, moveY);

            var isCollishion = Physics.CollisionTwoForms(square, circle, move);

            isCollishion.Should().Be(collisionPredicate);
        }
    }
}
