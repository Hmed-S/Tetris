using TetrisEngine.Domain.Extensions;

namespace TetrisEngine.Domain.Board
{
    public class TetrisBoard
    {
        public int Height { get; }
        public int Width { get; }
        public int Lines { get; private set; }
        public virtual int[,] Values { get; init; }


        public TetrisBoard(int rowCount, int columnCount)
        {
            if (rowCount < 10 || columnCount < 10)
                throw new ArgumentException("row and column count must at least be ten");
            Height = rowCount;
            Width = columnCount;
            Values = new int[rowCount, columnCount];

        }

        public void ClearRow(int rowNumber)
        {
            for (int col = 0; col < Values.GetLength(1); col++)
            {
                Values[rowNumber, col] = 0;
            }
        }

        private void ShiftDown(int rowNumber, List<Action<int, int>> onRowSwab)
        {
            for (int i = rowNumber-1; i >= 0; i--)
            {
                for (int j = 0; j < Width; j++)
                {
                    int emptyCel = Values[i+1, j];
                    Values[i+1, j] = Values[i, j];
                    Values[i, j] = emptyCel;
                }

                onRowSwab.ForEach(onrowSwab => onrowSwab(i+1, i));
            }
        }

        public int Clearlines(List<Action<int>> onEraseLineActions, List<Action<int, int>> onRowSwab)
        {
            int linesCleared = 0;
          
            for (int i = 0; i < Height; i++)
            {
                int[] row = Values.GetRow(i);

                if (Array.TrueForAll(row, x => x == 1))
                {
                    for (int j = 0; j < row.Length; j++) Values[i, j] = 0;

                    onEraseLineActions.ForEach(onEraseLineAction => onEraseLineAction(i));
                    linesCleared += 1;

                    ShiftDown(i, onRowSwab);
                }
            }

            return linesCleared;
        }

        public bool IsToppedOut(Tetromino tetromino)
        {
            foreach (var point in tetromino.Points)
            {
                if (point.Row < 0)
                    continue;

                if (Values[point.Row, point.Column] != 0)
                    return true;
            }

            return false;
        }

        public bool CanFit(Tetromino tetromino)
        {
            bool canFit = true;

            foreach (var point in tetromino.Points)
            {
                if(point.Column < 0 || point.Column > Width - 1 || point.Row < 0 || point.Row > Height - 1)
                {
                    canFit = false;
                    break;
                }

                if (Values[point.Row, point.Column] != 0)
                {
                    canFit = false;
                    break;
                }
            }

            return canFit;
        }

        public void WriteTetromino(Tetromino tetromino) => tetromino
            .Points
            .ForEach(point => Values[point.Row, point.Column] = 1);


       
    }
}
