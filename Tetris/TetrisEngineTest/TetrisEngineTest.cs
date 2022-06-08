using NUnit.Framework;
using Engine;

namespace TetrisEngineTest
{
    public class TetrisEngineTest
    {
        public static object[] DropCases()
        {
            return new object[]
            {
                new object[]{
                    1, new [,]{
                        {0, 0, 1},
                        {1, 1, 1},
                        {0, 0, 0},
                    }},
                
                new object[]{
                    2, new [,]{
                    {0, 0, 0},
                    {0, 0, 1},
                    {1, 1, 1},
                    {0, 0, 0}
                    
                }},
                new object[]{
                    3, new [,]{
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 1},
                    {1, 1, 1},
                    {0, 0, 0}
                    
                    }},
                new object[]{
                    4, new[,]{
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 1},
                    {1, 1, 1},
                    {0, 0, 0}
                    
                    }},
                new object[]{
                    5, new [,]{
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 1},
                        {1, 1, 1},
                        {0, 0, 0}
                    }},
                new object[]{
                    6, new [,]{
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 1},
                    {1, 1, 1},
                    {0, 0, 0}
                    }},
                new object[]{
                    7, new [,]{
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 1},
                        {1, 1, 1},
                        {0, 0, 0}
                    }},
                new object[]{
                    8, new[,]{
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 1},
                        {1, 1, 1},
                        {0, 0, 0}
                    }},
                new object[]{
                9, new[,]{
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 1},
                    {1, 1, 1}
                }},
                new object[]{
                    10, new[,]{
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 1},
                        {1, 1, 1}
                    }},
            };
        }
        
        
         public static object[] ShiftRightCases()
        {
            return new object[]
            {
                new object[]{
                    1, new [,]{
                        {0, 0, 0, 1},
                        {0, 1, 1, 1},
                        {0, 0, 0, 0},
                    }},
                
                new object[]{
                    2, new [,]{
                        {0, 0, 0, 0, 1},
                        {0, 0, 1, 1, 1},
                        {0, 0, 0, 0, 0},
                    }},
                new object[]{
                    3, new [,]{
                        {0, 0, 0, 0, 0, 1},
                        {0, 0, 0, 1, 1, 1},
                        {0, 0, 0, 0, 0, 0},
                    }},
                new object[]{
                    4, new[,]{
                        {0, 0, 0, 0, 0, 0, 1},
                        {0, 0, 0, 0, 1, 1, 1},
                        {0, 0, 0, 0, 0, 0, 0},
                    }},
                new object[]{
                    5, new [,]{
                        {0, 0, 0, 0, 0, 0, 0, 1},
                        {0, 0, 0, 0, 0, 1, 1, 1},
                        {0, 0, 0, 0, 0, 0, 0, 0},
                    }},
                new object[]{
                    6, new [,]{
                        {0, 0, 0, 0, 0, 0, 0, 0, 1},
                        {0, 0, 0, 0, 0, 0, 1, 1, 1},
                        {0, 0, 0, 0, 0, 0, 0, 0, 0},
                    }},
                new object[]{
                    7, new [,]{
                        {0, 0, 0, 0, 0, 0, 0, 0, 1},
                        {0, 0, 0, 0, 0, 0, 1, 1, 1},
                        {0, 0, 0, 0, 0, 0, 0, 0, 0},
                    }},
                new object[]{
                    8, new[,]{
                        {0 ,0 ,0 ,0 ,0 ,0 ,0, 0, 1},
                        {0, 0, 0, 0, 0, 0, 1, 1, 1},
                        {0, 0, 0, 0, 0, 0, 0, 0, 0},
                    }},
                new object[]{
                9, new[,]{
                    {0 ,0 ,0 ,0 ,0 ,0 ,0, 0, 1},
                    {0, 0, 0, 0, 0, 0, 1, 1, 1},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0},
                }},
                new object[]{
                    10, new[,]{
                        {0 ,0 ,0 ,0 ,0 ,0 ,0, 0, 1},
                        {0, 0, 0, 0, 0, 0, 1, 1, 1},
                        {0, 0, 0, 0, 0, 0, 0, 0, 0},
                    }},
            };
        }
        
        [TestCaseSource(nameof(DropCases))]
        public void DropTetrominoReturnsRightRows(int numberOfDrops, int[,] rows)
        {
            TetrisEngine engine = new(){ Board = new Board(10, 10) };

            for (int i = 0; i < numberOfDrops-1; i++) 
                engine.DropTetromino();
            engine.DropTetromino();
            Result result = engine.Status();
            
            Assert.AreEqual(rows, result.Rows);
        }

        [TestCaseSource(nameof(ShiftRightCases))]
        public void CanShiftToRight(int numberOfShifts, int[,] rows)
        {
            TetrisEngine engine = new(){ Board = new Board(10, 10) };
            engine.DropTetromino();

            for (int i = 0; i < numberOfShifts - 1; i++)
                engine.ShiftToRight();
            engine.ShiftToRight();
            Result result = engine.Status();
            
            Assert.AreEqual(rows, result.Rows);
        }
        
    }
}
