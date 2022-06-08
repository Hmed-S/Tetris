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
               new object[]{1,1},
               new object[]{2,2},
               new object[]{3,3},
               new object[]{4,4},
               new object[]{5,5},
               new object[]{6,6},
               new object[]{7,7},
               new object[]{8,8},
               new object[]{9,10},
               new object[]{10,10},
            };
        }
        
        
         public static object[] ShiftRightCases()
        {
            return new object[]
            {
                new object[]{1,2},
                new object[]{2,3},
                new object[]{3,4},
                new object[]{4,5},
                new object[]{5,6},
                new object[]{6,7},
                new object[]{7,7},
                new object[]{8,7},
                new object[]{9,7},
                new object[]{10,7},
            };
        }
        
        [TestCaseSource(nameof(DropCases))]
        public void DropTetrominoReturnsRightRows(int numberOfDrops, int yPosition)
        {
            TetrisEngine engine = new(){ Board = new Board(10, 10) };

            for (int i = 0; i < numberOfDrops-1; i++) 
                engine.DropTetromino();
            engine.DropTetromino();
            Result result = engine.Status();
            
            Assert.AreEqual(yPosition, result.LastYPosition);
        }

        [TestCaseSource(nameof(ShiftRightCases))]
        public void CanShiftToRight(int numberOfShifts, int xPosition)
        {
            TetrisEngine engine = new(){ Board = new Board(10, 10) };
            engine.DropTetromino();

            for (int i = 0; i < numberOfShifts - 1; i++)
                engine.ShiftToRight();
            engine.ShiftToRight();
            Result result = engine.Status();
            
            Assert.AreEqual(xPosition, result.LastXPosition);
        }
        
    }
}
