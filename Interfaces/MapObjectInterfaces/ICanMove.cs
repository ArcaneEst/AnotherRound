using System;
using System.Collections.Generic;
using System.Text;

namespace AnotherRound
{
    public interface ICanMove : IMapObject, IForm
    {
        int Speed { get; set; }
        public Vector GetMoveVector(ObstaclesVault MapObjects);
        public ICanMove GetCopyWithNewLocation(Vector location);
        public Vector ReactOnCollision(Obstacle obstacle, Vector move);
    }
}
