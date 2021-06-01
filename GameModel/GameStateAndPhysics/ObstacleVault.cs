using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class ObstacleVault
    {
        public List<Obstacle> Obstacles = new List<Obstacle>()
        { new CircleObstacle(new Vector(500, 540), new Size(50, 50)),
        new RemovableDamaging(new Vector(200,200), new Size(40, 40), 3),
        new SquareRemovable(new Vector(500, 500), new Size(100, 50), 10)};

        public List<ICanMove> MovingObstacles { get; set; }
    }
}
