using System.Diagnostics;
using Engine.Extensions;

namespace Engine
{
    public class Board
    {
        public int Height { get;}
        public int Width { get; }
        public List<Tetronmino> Tetronminos { get; } = new();

        public Board(int rowCount, int columnCount)
        {
            if (rowCount < 10 || columnCount < 10)
                throw new ArgumentException("row and column count must at least be ten");
            Height = rowCount;
            Width = columnCount;
        }

        public void AddTetromino(Tetronmino tetronmino) => Tetronminos.Add(tetronmino);

        /// <summary>
        ///  tetromino1:
        /// [
        ///  [ 1, 1, 1 ]
        ///  [ 0, 1, 1 ]
        ///  [ 0, 1, 0 ] // returns 0 if last is [1,0,0] continue
        /// ]
        ///
        ///  tetromino2
        /// [
        ///  [ 1, 0, 0 ]
        ///  [ 1, 0, 0 ]  [1,0,0] 
        ///  [ 1, 0, 0 ]  [1,1,1]
        /// ] returns 1 with above output and collumn = 1
        /// </summary>
        /// <param name="Tetromino1"></param>
        /// <returns></returns>
        public int FitTogether(Tetronmino tetronmino1, Tetronmino tetronmino2, int collumn)
        {
            int total = 0;
            Func<Tetronmino,int, int, bool> trueForRow = (m, i, match) =>  Array.TrueForAll(m.Shape.Value.GetRow(i), i => i==match);
            if (trueForRow(tetronmino1, 2, 2) || trueForRow(tetronmino2, 0, 1)) return total;
            int[] tetromino1Column = tetronmino1.Shape.Value.GetColumn(collumn);
            int[] tetromnino2Column = tetronmino2.Shape.Value.GetColumn(collumn);
            foreach (int i in Enumerable.Range(0,2))
            {
                if (tetromino1Column[i] == 1 && tetromnino2Column[i] == 0 ||
                    tetromino1Column[i] == 0 && tetromnino2Column[i] == 1  ) total += 1;
                else if(tetromnino2Column[i] == 1)  break;
            }
            return total;
        }
        
        public int ShiftCoordinates(int x, int y, Tetronmino tetromino)
        {
            int maxYValue = Height - 2 + tetromino.NumberOfEmptyRows();
            int maxXValue = Width - 2 + tetromino.numberOfEmptyColumns();

            var other = Tetronminos.FindLast(tetro =>  tetro.YPosition == tetromino.YPosition+3);
            
            
            if (other != null && Tetronminos.Count >1)
            {
                if (other.XPosition == tetromino.XPosition)
                {
                    maxYValue = other.YPosition-3;

                    if(Array.TrueForAll(other.Shape.Value.GetRow(0), i=> i==0)) maxYValue += 1;
                    if(Array.TrueForAll(tetromino.Shape.Value.GetRow(2), i=> i==0)) maxYValue += 1;
                    maxYValue +=FitTogether(tetromino, other, 0);
                    
                    tetromino.YPosition = maxYValue;
                }

                if (tetromino.XPosition == other.XPosition + 1)
                {
                    Trace.WriteLine("+1");
                }
                if(tetromino.XPosition == other.XPosition+2) Trace.WriteLine("+2");
                
            }

            if (y >=maxYValue) return -1;
            if (x >= maxXValue || x<0) return -2;

            tetromino.YPosition = y;
            tetromino.XPosition = x;
            return 0;
        }
    }
}
