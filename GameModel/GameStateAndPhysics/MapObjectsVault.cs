using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AnotherRound
{
    public class MapObjectsVault
    {
        public Player Player { get; set; } = new Player(new Vector(100, 100), new Size(50, 50));
        private List<Obstacle> Obstacles = new List<Obstacle>();
        
        public IEnumerable<Obstacle> GetAllObstacles()
        {
            foreach(var obstacle in Obstacles)
            {
                if (obstacle is ICanBeDamaged obstacleDamaged && obstacleDamaged.IsDead)
                    continue;

                yield return obstacle;
            }
        }

        public static MapObjectsVault GenerateTestLevel()
        {
            var vault = new MapObjectsVault();
            vault.Obstacles = new List<Obstacle>()
            { 
                new CircleObstacle(new Vector(500, 540), new Size(50, 50)),
                new RemovableDamaging(new Vector(200,200), new Size(40, 40), 3),
                new SquareRemovable(new Vector(200, 500), new Size(100, 50), 10),
                new MovingDamaging(new Vector(600, 300), new Size(25, 25)),
                new Enemy(new Vector(700, 600), new Size(10, 10))
            };

            return vault;
        }

        public static MapObjectsVault GenerateProrotypeLevel()
        {
            var vault = new MapObjectsVault();
            foreach (var obstacle in FirstThird())
            {
                vault.Obstacles.Add(obstacle);
            }

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

        public void ExecuteMoving(Size fieldSize)
        {
            foreach(var obstacle in GetAllObstacles())
            {
                if (!(obstacle is ICanMove movable))
                    continue;

                var moveVector = movable.GetMoveVector(this);
                if (movable.IsCanCollision)
                    moveVector = Movement.CalculateMoveWithCollision(movable, this, fieldSize, moveVector);

                movable.Location += moveVector;
            }
        }

        public void CleanDead()
        {
            for (var i = 0; i < Obstacles.Count; i++)
            {
                if (Obstacles[i] is ICanBeDamaged damaged && damaged.IsDead)
                    Obstacles.RemoveAt(i);
            }
        }
    }
}
