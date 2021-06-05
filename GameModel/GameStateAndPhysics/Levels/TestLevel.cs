using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    class TestLevel : ObstaclesVault
    {
        public static ObstaclesVault GenerateTestLevel()
        {
            var vault = new ObstaclesVault();
            vault.Obstacles = new HashSet<Obstacle>()
            {
                new CircleObstacle(new Vector(500, 540), new Size(50, 50)),
                new RemovableDamaging(new Vector(200,200), new Size(40, 40), 3),
                new SquareRemovable(new Vector(200, 500), new Size(100, 50), 10),
                new MovingDamaging(new Vector(600, 300), new Size(25, 25)),
                new Enemy(new Vector(700, 600), new Size(10, 10))
            };

            return vault;
        }
    }
}
