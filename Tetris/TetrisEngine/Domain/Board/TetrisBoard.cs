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

        public void Clearlines(List<Action<int>> onEraseLineActions)
        {
          
            for (int i =0; i < Height; i++)
            {
                int[] row = Values.GetRow(i);

                if (Array.TrueForAll(row, x => x > 1))
                {
                    Array.ForEach(row, (col) => col = 0);
                    onEraseLineActions.ForEach(onEraseLineAction => onEraseLineAction(i));
                }
            }
            
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
