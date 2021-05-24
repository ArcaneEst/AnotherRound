using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AnotherRound
{
    /// <summary>
    /// Поле боя.
    /// </summary>
    public class Field
    {
        public Field(Vector playerStartPosition)
        {
            Player = new Player(playerStartPosition);
        }

        public Field()
        { }

        public int Speed { get; set; } = 5;
        public Size FieldSize { get; set; } = new Size(1200, 700);
        public Player Player { get; set; } = new Player(new Vector(100, 100));
        public List<Projectail> Projectails { get; set; } = new List<Projectail>();
        public List<SquareObstacle> Obstacles { get; set; } = new List<SquareObstacle>() 
        { new SquareObstacle(new Vector(101, 50), new Size(50, 50)) };

        private bool IsOutOfField(Vector location, Size objectSize, Vector move)
        {
            var newX = location.X + move.X;
            var newY = location.Y + move.Y;

            return newX > FieldSize.Width - objectSize.Width / 2 ||
                newX < objectSize.Width / 2 ||
                newY > FieldSize.Height - objectSize.Height / 2 ||
                newY < objectSize.Height / 2;
        }

        private bool IsOutOfField(Vector newLocation, Size objectSize)
        {
            return newLocation.X > FieldSize.Width - objectSize.Width / 2 ||
                newLocation.X < objectSize.Width / 2 ||
                newLocation.Y > FieldSize.Height - objectSize.Height / 2 ||
                newLocation.Y < objectSize.Height / 2;
        }

        //Стрельба
        #region
        public void ExecuteAllProjectiles()
        {
            foreach (var proj in Projectails)
                ExecuteProjectile(proj);

            RemoveHitedProjectiles();
        }

        private void RemoveHitedProjectiles()
        {
            Projectails = Projectails
                .Where(proj => !IsOutOfField(proj.Location, proj.Size))
                .ToList();
        }

        private void ExecuteProjectile(Projectail projectile)
        {
            var newProjectileLocation = projectile.CalculateNewLocation();

            projectile.Location = newProjectileLocation;
        }



        internal void TryShoot(Direction controlX, Direction controlY)
        {
            var newProjectile = Player.TryGenerateProjectile(controlX, controlY);

            if (newProjectile is null)
                return;

            Projectails.Add(newProjectile);
        }
        #endregion

        //Движение персонажа игрока.
        #region
        private Vector MoveInDirectionUsingVector(Direction controlX, Direction controlY)
        {
            var xMove = controlX < 0 ? -1 : controlX > 0 ? 1 : 0;
            var yMove = controlY < 0 ? -1 : controlY > 0 ? 1 : 0;

            return new Vector(Speed * xMove, Speed * yMove);
        }

        internal void MovePlayer(Direction controlX, Direction controlY)
        {
            var move = MoveInDirectionUsingVector(controlX, controlY);
            
            ApplyMoveWithCheckFieldSize(move);
        }

        private void ApplyMoveWithCheckFieldSize(Vector move)
        {
            var newX = Player.Location.X + move.X;
            var newY = Player.Location.Y + move.Y;
            var moveX = move.X;
            var moveY = move.Y;

            if (newX > FieldSize.Width - Player.Size.Width / 2)
                moveX = (FieldSize.Width - Player.Size.Width / 2) - Player.Location.X;

            if (newX < Player.Size.Width / 2)
                moveX = Player.Location.X - Player.Size.Width / 2;

            if (newY > FieldSize.Height - Player.Size.Height / 2)
                moveY = (FieldSize.Height - Player.Size.Height / 2) - Player.Location.Y;

            if (newY < Player.Size.Height / 2)
                moveY = Player.Location.Y - Player.Size.Height / 2;

            Player.ApplyMove(new Vector(moveX, moveY));
        }
        #endregion
    }
}
