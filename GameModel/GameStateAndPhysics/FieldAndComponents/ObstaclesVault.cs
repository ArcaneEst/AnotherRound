using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AnotherRound
{
    public class ObstaclesVault
    {
        public Player Player { get; set; } = new Player(new Vector(100, 100), new Size(50, 50));
        public HashSet<Obstacle> Obstacles { get; set; } = new HashSet<Obstacle>();
        
        public virtual void ExecuteVault(Size fieldSize)
        {
            ClearFromDead();
            ExecuteMoving(fieldSize);
        }

        public virtual string[] GenerateInfoTable()
        {
            return new string[0];
        }

        public IEnumerable<Obstacle> GetAllObstacles()
        {
            foreach(var obstacle in Obstacles)
            { 
                if (obstacle is ICanBeDamaged obstacleDamaged && obstacleDamaged.IsDead)
                    continue;

                yield return obstacle;
            }
        }

        public virtual void ClearFromDead()
        {
            var list = new List<Obstacle>();
            foreach (var obstacle in Obstacles)
            {
                if (obstacle is ICanBeDamaged damaged && damaged.IsDead)
                    list.Add(obstacle);
            }

            foreach (var toDelete in list)
            {
                Obstacles.Remove(toDelete);
            }
        }

        public void AddNewObtacle(Obstacle obstacle)
        {
            lock (Obstacles)
            {
                Obstacles.Add(obstacle);
            }
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
    }
}
