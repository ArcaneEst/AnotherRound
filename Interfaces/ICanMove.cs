using System;
using System.Collections.Generic;
using System.Text;

namespace AnotherRound
{
    public interface ICanMove
    {
        int Speed { get; set; }

        public Vector GetMoveVector(); 
    }
}
