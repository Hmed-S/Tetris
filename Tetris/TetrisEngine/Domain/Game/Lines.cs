using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisEngine.Domain.Game
{
    public readonly struct Lines
    {
        public readonly int Value { get; } = 0;

        public Lines()
        {

        }

        private Lines(int value)
        {
            Value = value;
        }

        public static Lines operator +(Lines line, int lines) => new (line.Value + lines);
    }
}
