using System;
using System.Collections.Generic;
using System.Drawing;
namespace AnotherRound
{
    public interface IForm
    {
        Vector Location { get; set; }
        Size Size { get; set; }
    }
}
