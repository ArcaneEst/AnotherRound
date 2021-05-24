﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AnotherRound
{
    class Controller
    {
        public ControlFlags ShootFlags { get; set; } = new ControlFlags();
        public ControlFlags MoveFlags { get; set; } = new ControlFlags();

        public void HandleKey(Keys e, bool down)
        {
            if (e == Keys.W) MoveFlags.Up = down;
            if (e == Keys.S) MoveFlags.Down = down;
            if (e == Keys.A) MoveFlags.Left = down;
            if (e == Keys.D) MoveFlags.Right = down;

            if (e == Keys.Up) ShootFlags.Up = down;
            if (e == Keys.Down) ShootFlags.Down = down;
            if (e == Keys.Left) ShootFlags.Left = down;
            if (e == Keys.Right) ShootFlags.Right = down;
        }

        public void ExecuteMove(Field field)
        {
            var controlX = CalculateControlX(MoveFlags);
            var controlY = CalculateControlY(MoveFlags);
            field.MovePlayer(controlX, controlY);
        }

        public void ExecuteShoot(Field field)
        {
            var controlX = CalculateControlX(ShootFlags);
            var controlY = CalculateControlY(ShootFlags);
            field.TryShoot(controlX, controlY);
        }

        public Direction CalculateControlY(ControlFlags controlFlags)
        {
            if (controlFlags.Up == controlFlags.Down)
                return Direction.None;
            else
                return controlFlags.Up ? Direction.Up : Direction.Down;
        }

        public Direction CalculateControlX(ControlFlags controlFlags)
        {
            if (controlFlags.Right == controlFlags.Left)
                return Direction.None;
            else
                return controlFlags.Right ? Direction.Right : Direction.Left;
        }
    }
}
