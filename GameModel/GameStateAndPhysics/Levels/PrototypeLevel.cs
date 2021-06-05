using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    class PrototypeLevel : ObstaclesVault
    {
        public static ObstaclesVault GenerateProrotypeLevel()
        {
            var vault = new ObstaclesVault();
            foreach (var obstacle in FirstThird())
            {
                vault.Obstacles.Add(obstacle);
            }

            vault.Player = new Player(new Vector(30, 30), new Size(50, 50));

            return vault;
        }

        private static List<Obstacle> FirstThird()
        {
            var list = new List<Obstacle>()
            {
                new SquareObstacle(new Vector(500, 100), new Size(900, 20)),
                new SquareRemovable(new Vector(25, 100), new Size(50, 20), 25)
            };

            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 2; j++)
                    list.Add(new CircleDamage(
                        new Vector(100 + 80 * i + 60 * (j % 2), 20 + 70 * j),
                        new Size(20, 20)));
            }

            return list;
        }
    }
}
