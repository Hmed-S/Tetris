using NUnit.Framework;
using Moq;
using TetrisEngine.Board;

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
               new object[]{8,7},
               new object[]{9,7},
               new object[]{10,7},
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
            var engineMock = new Mock<TetrisEngine>(new TetrisBoard(10,10));
            engineMock.Setup(engine => engine.CurrentTetromino).Returns(Tetromino.FromShape(Shapes.LShape));
            TetrisEngine engine = engineMock.Object;

            for (int i = 0; i < numberOfDrops-1; i++) 
                engine.DropTetromino();
            engine.DropTetromino();
            Tetromino tetromino = engine.CurrentTetromino;
            
            Assert.AreEqual(yPosition, tetromino.YPosition);
        }

        [TestCaseSource(nameof(ShiftRightCases))]
        public void CanShiftToRight(int numberOfShifts, int xPosition)
        {
            var engineMock = new Mock<TetrisEngine>(new TetrisBoard(10,10));
            engineMock.Setup(engine => engine.CurrentTetromino).Returns(Tetromino.FromShape(Shapes.LShape));
            TetrisEngine engine = engineMock.Object;

            for (int i = 0; i < numberOfShifts - 1; i++)
                engine.ShiftToRight();
            engine.ShiftToRight();
            Tetromino tetromino = engine.CurrentTetromino;
            
            Assert.AreEqual(xPosition, tetromino.XPosition);
        }
        
        
        [TestCase(0, 0)]
        [TestCase(1, 40)]
        [TestCase(2, 100)]
        [TestCase(3, 300)]
        [TestCase(4, 1200)]
        [TestCase(5, 0)]
        public void EngineCalculatesScoreCorrect(int numberOfLines, int score)
        {
            var boardMock = new Mock<TetrisBoard>(16,10);
            boardMock.Setup(b => b.Values).Returns(new int[16, 10]);
            boardMock.Setup(b => b.CountLines()).Returns(numberOfLines);
            TetrisEngine engine = new(boardMock.Object);
            
            engine.Next();
            
            Assert.AreEqual(score, engine.Score);
        }
    }
}
