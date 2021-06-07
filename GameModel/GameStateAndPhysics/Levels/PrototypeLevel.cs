using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    class PrototypeLevel : ObstaclesVault
    {
        private Objective Objective;
        private bool ToSpawn = true;
        private List<Vector> SpawnPoints = new List<Vector>()
        {
            new Vector(1100, 50),
            new Vector(600, 350)
        };
        private Random rnd = new Random();
        private System.Timers.Timer SpawnTimer = new System.Timers.Timer();

        public override void ExecuteVault(Size fieldSize)
        {
            base.ExecuteVault(fieldSize);
            IsPlayerWin();

            if (ToSpawn)
            {
                ToSpawn = false;
                AddNewObtacle(new Enemy(SpawnPoints[rnd.Next(0, SpawnPoints.Count)], new Size(15, 15)));
            }
        }

        public void IsPlayerWin()
        {
            if (Physics.IsTwoAbstracktsCollision(Player, Objective))
                Player.IsWinGame = true;
        }

        public static ObstaclesVault GenerateProrotypeLevel()
        {
            var cooldown = 2000;
            var vault = new PrototypeLevel();
            var objective = new Objective(new Vector(1175, 675), new Size(50, 50));
            vault.Objective = objective;
            vault.Obstacles.Add(objective);
            GenerateVaultObjectList(vault);

            vault.SpawnTimer = new System.Timers.Timer(cooldown);

            vault.SpawnTimer.Elapsed += (e, args) =>
            {
                vault.SpawnTimer.Interval = vault.SpawnTimer.Interval - 10 > 100 ?
                    vault.SpawnTimer.Interval - 10 : 100;
                vault.ToSpawn = true;
            };

            vault.Player = new Player(new Vector(600, 400), new Size(50, 50));
            vault.SpawnTimer.Start();

            vault.Player = new Player(new Vector(30, 30), new Size(50, 50));

            return vault;
        }

        private static void GenerateVaultObjectList(PrototypeLevel vault)
        {
            foreach (var obstacle in FirstThird())
            {
                vault.Obstacles.Add(obstacle);
            }

            foreach (var obstacle in MidOfMap())
            {
                vault.Obstacles.Add(obstacle);
            }

            foreach (var obstacle in ObjectsAroundObjective())
            {
                vault.Obstacles.Add(obstacle);
            }
        }

        public override string[] GenerateInfoTable()
        {
            return new string[] {
                $"Health: {Player.HealthPoints}" };
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

        private static List<Obstacle> MidOfMap()
        {
            var movingSize = new Size(25, 25);

            var list = new List<Obstacle>();

            for (var i = 0; i < 5; i++)
            {
                list.Add(new MovingDamaging(new Vector(60 + 200 * i, 120), 
                    new Vector(300 + 200 * i, 675), movingSize));

                list.Add(new MovingDamaging(new Vector(1100 - 200 * i, 120),
                    new Vector(860 - 200 * i, 675), movingSize));

                if (i % 2 == 0)
                    list.Add(new MovingDamaging(new Vector(25, 200 + 75 * i), 
                        new Vector(1175, 200 + 75 * i), movingSize));
            }

            return list;
        }

        private static List<Obstacle> ObjectsAroundObjective()
        {
            var list = new List<Obstacle>()
            {
                new SquareObstacle(new Vector(900, 640), new Size(600, 20)),
                new SquareObstacle(new Vector(240, 640), new Size(500, 20)),
                new SquareRemovable(new Vector(25, 675), new Size(50, 50), 1),
                new MovingDamaging(new Vector(75, 675), new Vector(1125, 675), new Size(25, 25), 5)
            };

            return list;
        }
    }
}
