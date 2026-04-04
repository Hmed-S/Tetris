namespace TetrisEngine.Domain.Game
{
    public readonly struct Score
    {
        private readonly int _linesGained = 0;
        public readonly int Value { get; }

        public Score(int linesGained)
        {
            _linesGained = linesGained;
            Value = linesGained switch
            {
                1 => 40,
                2 => 100,
                3 => 300,
                4 => 1200,
                _ => 0
            };
        }
    }
}
