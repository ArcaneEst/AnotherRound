using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnotherRound
{
    public class Enemy : CircleObstacle, IMapObject, ICanMove
    {
        public int Speed { get; set; } = 3;

        public Enemy(Vector location, Size size) : base(location, size)
        {
            Location = location;
            Size = size;
        }

        public Vector GetMoveVector(MapObjectsVault mapObjects)
        {
            var angleToPlayer = Location.AngleTo(mapObjects.Player.Location);

            return new Vector((int)Math.Ceiling(Math.Cos(angleToPlayer) * Speed), 
                (int)Math.Ceiling(Math.Sin(angleToPlayer) * Speed));
        }

        public ICanMove GetCopyWithNewLocation(Vector location)
        {
            throw new NotImplementedException();
        }

        public Vector ReactOnCollision(Obstacle obstacle, Vector move)
        {
            throw new NotImplementedException();
        }
    }
}
