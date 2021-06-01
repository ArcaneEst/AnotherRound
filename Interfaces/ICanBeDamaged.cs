using System;
using System.Collections.Generic;
using System.Text;

namespace AnotherRound
{
    public interface ICanBeDamaged
    {
        int HealthPoints { get; set; }
        bool IsDead { get; }
        void GetHit();
    }
}
