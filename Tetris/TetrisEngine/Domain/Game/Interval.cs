using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TetrisEngine.Domain.Game
{
    public readonly struct Interval
    {
        public readonly double Seconds { get; } = 1;
        public readonly double MiliSeconds { get => Seconds * 1000; }
        public readonly Interval HardDrop { get => new Interval(0.1); }

        public Interval() { }

        private Interval(Level level)
        {
            Seconds = Math.Pow(0.8 - ((level.Value - 1) * 0.007), level.Value - 1);
        }

        private Interval(double seconds)
        {
            Seconds = 0.1;
        }

        public static Interval operator +(Interval interval, Level level) => new Interval(level);

    }
}
