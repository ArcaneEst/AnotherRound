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
        private List<ICanBeDamaged> Removables = new List<ICanBeDamaged>();
        private List<ICanMove> MovingObstacles { get; set; } = new List<ICanMove>() {};

        public IEnumerable<Obstacle> GetAllObstacles()
        {
            foreach (var obstacle in Obstacles)
                yield return obstacle;

            foreach (var moving in MovingObstacles)
                if (moving is Obstacle movingObstacle)
                    yield return movingObstacle;

            foreach (var removable in Removables)
                if (!removable.IsDead && removable is Obstacle removableObstacle)
                    yield return removableObstacle;
        }

        public static MapObjectsVault GenerateTestLevel()
        {
            var vault = new MapObjectsVault();
            vault.Obstacles = new List<Obstacle>()
                { new CircleObstacle(new Vector(500, 540), new Size(50, 50))
                };

            vault.Removables = new List<ICanBeDamaged>()
            {
                new RemovableDamaging(new Vector(200,200), new Size(40, 40), 3),
                new SquareRemovable(new Vector(200, 500), new Size(100, 50), 10),
            };

            vault.MovingObstacles = new List<ICanMove>() { 
                new MovingDamaging(new Vector(600, 300), new Size(25, 25)), 
                new Enemy(new Vector(700, 600), new Size(10, 10))
            };

            return vault;
        }

        public void ExecuteMoving(Size fieldSize)
        {
            foreach(var moving in MovingObstacles)
            {
                if (moving is ICanBeDamaged movingDamaged && movingDamaged.IsDead)
                    continue;

                var moveVector = moving.GetMoveVector(this);
                if (moving.IsCanCollision)
                    moveVector = Movement.CalculateMoveWithCollision(moving, this, fieldSize, moveVector);

                moving.Location = moving.Location + moveVector;
            }
        }
    }
}
