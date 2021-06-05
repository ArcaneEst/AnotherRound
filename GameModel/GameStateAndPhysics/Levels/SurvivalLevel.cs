using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class SurvivalLevel : ObstaclesVault
    {
        public List<Vector> SpawnPoints = new List<Vector>()
        {
            new Vector(50, 50),
            new Vector(50, 650),
            new Vector(1150, 50),
            new Vector(1150, 650)
        };

        public bool ToSpawn { get; set; } = true;
        private Random rnd { get; set; }
        private System.Timers.Timer SpawnTimer = new System.Timers.Timer();
        public SurvivalLevel()
        {
            rnd = new Random();
        }

        public override void ExecuteVault(Size fieldSize)
        {
            ClearFromDead();
            ExecuteMoving(fieldSize);

            if (ToSpawn)
            {
                ToSpawn = false;
                AddNewObtacle(new Enemy(SpawnPoints[rnd.Next(0, SpawnPoints.Count)], new Size(15, 15)));
            }
        }

        public static ObstaclesVault GenerateSurvival()
        {
            var cooldown = 1000;
            
            var rnd = new Random();
            var vault = new SurvivalLevel();

            vault.SpawnTimer = new System.Timers.Timer(cooldown);

            vault.SpawnTimer.Elapsed += (e, args) =>
            {
                vault.SpawnTimer.Interval = vault.SpawnTimer.Interval - 10 > 100 ? 
                    vault.SpawnTimer.Interval - 10 : 100;
                vault.ToSpawn = true;
            };

            vault.Player = new Player(new Vector(600, 400), new Size(50, 50));
            vault.SpawnTimer.Start();

            return vault;
        }
    }
}
