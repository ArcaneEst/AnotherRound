using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class SquareRemovable : SquareObstacle, ICanBeDamaged
    {
        public int HealthPoints { get; set; }
        public bool IsDead => HealthPoints <= 0;
        public SquareRemovable(Vector location, Size size, int healthPoints) : base(location, size)
        {
            Location = location;
            Size = size;
            HealthPoints = healthPoints;
        }

        public void GetHit()
        {
            HealthPoints--;
        }
    }
}