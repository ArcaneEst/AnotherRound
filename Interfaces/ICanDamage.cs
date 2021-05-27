using System;
using System.Collections.Generic;
using System.Text;

namespace AnotherRound
{
    public interface ICanDamage
    {
        public int CalculateDamage(Player player);
    }
}
