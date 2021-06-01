using System;
using System.Collections.Generic;
using System.Drawing;


namespace AnotherRound
{
    class RunningEnemy : CircleRemovable, ICanBeDamaged, ICanDamage
    {
        public new Vector Location { get; set; }
        public new Size Size {get;set;}
        public int HealthPoint { get; set; }

        public RunningEnemy(Vector location, Size size, int healthPoints) : base(location, size, healthPoints)
        {
            Location = location;
            Size = size;
            HealthPoint = healthPoints;
        }

        public int CalculateDamage(Player player)
        {
            return 1;
        }
    }
}
