using System;
using System.Collections.Generic;
using System.Drawing;
namespace AnotherRound
{
    public interface IForm : IMapObject
    {
        Size Size { get; set; }
        public bool IsCanCollision { get; set; }
    }
}
