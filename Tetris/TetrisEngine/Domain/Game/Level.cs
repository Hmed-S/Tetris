using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisEngine.Domain.Game
{
    public readonly struct Level
    {
        public int Value { get; } = 1;
        
        public Level() { }

        private Level(int level)
        {
            if(level > 4) Value = level;
            Value = level;
        }

        public static Level operator +(Level level, Lines lines) => new(lines.Value/10+1);
        
    }
}
