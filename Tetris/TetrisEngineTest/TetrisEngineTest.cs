using NUnit.Framework;
using Engine;
using Moq;

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
               new object[]{9,8},
               new object[]{10,8},
            };
        }

        public static object[] ShiftRightCases()
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
                new object[]{8,7},
                new object[]{9,7},
                new object[]{10,7}
            };
        }
        
        [TestCaseSource(nameof(DropCases))]
        public void DropTetrominoReturnsRightRows(int numberOfDrops, int yPosition)
        {
            var engineMock = new Mock<TetrisEngine>(10,10);
            engineMock.Setup(engine => engine.CurrentTetromino()).Returns(Tetronmino.FromShape(Shapes.LShape));
            TetrisEngine engine = engineMock.Object;

            for (int i = 0; i < numberOfDrops-1; i++) 
                engine.DropTetromino();
            engine.DropTetromino();
            Tetronmino tetronmino = engine.CurrentTetromino();
            
            Assert.AreEqual(yPosition, tetronmino.YPosition);
        }

        [TestCaseSource(nameof(ShiftRightCases))]
        public void CanShiftToRight(int numberOfShifts, int xPosition)
        {
            var engineMock = new Mock<TetrisEngine>(10,10);
            engineMock.Setup(engine => engine.CurrentTetromino()).Returns(Tetronmino.FromShape(Shapes.LShape));
            TetrisEngine engine = engineMock.Object;

            for (int i = 0; i < numberOfShifts - 1; i++)
                engine.ShiftToRight();
            engine.ShiftToRight();
            Tetronmino tetronmino = engine.CurrentTetromino();
            
            Assert.AreEqual(xPosition, tetronmino.XPosition);
        }
    }
}
