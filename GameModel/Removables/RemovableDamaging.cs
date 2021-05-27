using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    class RemovableDamaging : CircleRemovable, ICanDamage
    {
        public RemovableDamaging(Vector location, Size size, int healthPoints) 
            : base(location, size, healthPoints)
        {
            Location = location;
            Size = size;
            HealthPoints = healthPoints;
        }

        public void DealPlayerDamage(Player player)
        {
            player.DealDamage(1);
        }
    }
}
