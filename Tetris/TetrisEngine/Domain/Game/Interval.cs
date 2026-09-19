using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TetrisEngine.Domain.Game
{
    public readonly struct Interval
    {
        public readonly int Seconds { get; } = 1;
        public readonly int MiliSeconds { get => Seconds * 1000; }

        public Interval() { }

        private Interval(Level level)
        {
            Math.Pow(0.8 - ((level.Value - 1) * 0.007), level.Value - 1);
        }

        public static Interval operator +(Interval interval, Level level) => new Interval(level);

    }
}
