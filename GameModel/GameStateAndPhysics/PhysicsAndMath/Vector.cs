using System;

namespace AnotherRound
{
    /// <summary>
    /// Класс векторов, описывающих положения и траектории снарядов.
    /// </summary>
    public class Vector
    {
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static Vector Zero { get => new Vector(0, 0); }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator *(Vector a, int e)
        {
            return new Vector(a.X * e, a.Y * e);
        }

        public static Vector operator /(Vector a, int d)
        {
            return new Vector(a.X / d, a.Y / d);
        }

        public double ModOfDistanceTo(Vector anotherVector)
        {
            return Math.Sqrt(
                (X - anotherVector.X) * (X - anotherVector.X) + (Y - anotherVector.Y) * (Y - anotherVector.Y));
        }

        public Vector VectorTo(Vector anotherVector)
        {
            return new Vector(Math.Abs(anotherVector.X - X), Math.Abs(anotherVector.Y - Y));
        }

        public Vector Copy()
        {
            return new Vector(X, Y);
        }

        public double Length { get => Math.Sqrt(X * X + Y * Y); }
    }
}
