using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    class CircleRemovable : CircleObstacle, IEnemy
    {
        public new Vector Location {get;set;}
        public new Size Size { get; set; }
        public int HealthPoints { get; set; }
        public bool IsDead => HealthPoints <= 0;
        public CircleRemovable(Vector location, Size size, int healthPoints) : base(location, size)
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
