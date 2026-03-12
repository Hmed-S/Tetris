using NUnit.Framework;
using System;
using System.Collections.Generic;
using TetrisEngine.Board;

namespace TetrisEngineTest
{
    public class BoardTest
    {
        
        public static IEnumerable<TestCaseData> DrawCases()
        {
            return new[]
            {   
                new TestCaseData(1,2, 0, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(1,5, 0, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(1,7, -1, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(1,8, -1, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(1,9, -1, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(1,10,-1, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(1,11,-1, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(1,1, 0, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(5,1, 0, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(7,1, 0, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(8,1, 0, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(9,1, -1, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(10,1,-1, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(11,1,-1, Tetromino.FromShape(Shapes.JShape)),
                new TestCaseData(1,2, 0, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(1,5, 0, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(1,7, 0, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(1,8, -1, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(1,9, -1, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(1,10,-1, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(1,11,-1, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(4,1, 0, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(5,1, 0, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(6,1, 0, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(7,1, 0, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(8,1, -1, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(9,1, -1, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(10,1, -1, Tetromino.FromShape(Shapes.LShape)),
                new TestCaseData(11,1, -1, Tetromino.FromShape(Shapes.LShape))
            };
        }

        public static IEnumerable<TestCaseData> Linecases()
        {
            return new[]
            {
                new TestCaseData(
                    new int[16,10]
                    {
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {1,1,0,0,0,0,0,0,0,0},
                        {1,0,0,1,1,0,1,1,0,0},
                        {1,0,0,1,1,0,0,0,1,1},
                        {1,1,1,1,1,1,1,1,1,1},
                        {1,1,0,0,0,0,0,0,1,1},
                        {1,1,1,0,1,0,0,1,1,1},
                        {1,1,1,0,1,1,1,1,1,1},
                        {1,1,0,0,1,1,1,1,1,1}
                    },
                    new int[16,10]
                    {
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {1,1,0,0,0,0,0,0,0,0},
                        {1,0,0,1,1,0,1,1,0,0},
                        {1,0,0,1,1,0,0,0,1,1},
                        {1,1,0,0,0,0,0,0,1,1},
                        {1,1,1,0,1,0,0,1,1,1},
                        {1,1,1,0,1,1,1,1,1,1},
                        {1,1,0,0,1,1,1,1,1,1}
                    }), 
                new TestCaseData(
                    new int[16,10]
                    {
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {1,1,0,0,0,0,0,0,0,0},
                        {1,0,0,1,1,0,1,1,0,0},
                        {1,0,0,1,1,0,0,0,1,1},
                        {1,0,0,1,1,1,1,1,1,1}, 
                        {1,1,0,1,0,0,1,0,1,1},
                        {1,1,1,1,1,0,1,1,1,1},
                        {1,1,1,0,1,1,1,1,1,1},
                        {1,1,1,1,1,1,1,1,1,1}
                    },
                    new int[16,10]
                    {
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {1,1,0,0,0,0,0,0,0,0},
                        {1,0,0,1,1,0,1,1,0,0},
                        {1,0,0,1,1,0,0,0,1,1},
                        {1,0,0,1,1,1,1,1,1,1}, 
                        {1,1,0,1,0,0,1,0,1,1},
                        {1,1,1,1,1,0,1,1,1,1},
                        {1,1,1,0,1,1,1,1,1,1},
                    }), 
                new TestCaseData( 
                    new int[16,10]
                    {
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {1,1,1,0,0,0,0,1,1,1}, 
                        {1,0,0,0,0,0,0,1,0,0},
                        {1,0,0,0,0,0,0,1,0,0},
                        {1,1,1,1,1,1,1,1,1,1},
                        {1,1,1,1,1,1,1,1,1,1}
                    },
                    new int[16,10]
                    {
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0}, 
                        {0,0,0,0,0,0,0,0,0,0},
                        {1,1,1,0,0,0,0,1,1,1},
                        {1,0,0,0,0,0,0,1,0,0},
                        {1,0,0,0,0,0,0,1,0,0}
                    }), 
                   new TestCaseData(
                       new int[16,10]
                       {
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,1,1,0,0,0,0,0,0,0},
                           {0,1,1,0,0,0,0,0,0,0},
                           {1,1,1,0,0,0,0,0,0,0},
                           {1,0,0,0,0,0,0,1,1,0}, 
                           {1,0,0,0,0,0,0,1,1,0},
                           {1,1,1,1,1,1,1,1,1,1},
                           {1,1,1,1,1,1,1,1,1,1},
                           {1,1,1,1,1,1,1,1,1,1}
                       },
                       new int[16,10]
                       {
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,1,1,0,0,0,0,0,0,0}, 
                           {0,1,1,0,0,0,0,0,0,0},
                           {1,1,1,0,0,0,0,0,0,0},
                           {1,0,0,0,0,0,0,1,1,0},
                           {1,0,0,0,0,0,0,1,1,0}
                       }),
                   
                   new TestCaseData(
                       new int[16,10]
                       {
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,1,0,0,0,1,0,0,0},
                           {1,1,1,0,0,0,1,0,0,0},
                           {1,1,1,1,1,0,1,0,0,0},
                           {1,0,0,0,0,0,0,1,1,0}, 
                           {1,1,1,1,1,1,1,1,1,1},
                           {1,1,1,1,1,1,1,1,1,1},
                           {1,1,1,1,1,1,1,1,1,1},
                           {1,1,1,1,1,1,1,1,1,1}
                       },
                       new int[16,10]
                       {
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0}, 
                           {0,0,1,0,0,0,1,0,0,0},
                           {1,1,1,0,0,0,1,0,0,0},
                           {1,1,1,1,1,0,1,0,0,0},
                           {1,0,0,0,0,0,0,1,1,0}, 
                       }),
                   
                   new TestCaseData(
                       new int[16,10]
                       {
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {1,1,1,0,1,1,1,1,1,1},
                           {0,1,0,0,1,1,1,0,1,1},
                           {0,1,0,0,1,1,1,0,1,1},
                           {1,1,1,1,1,1,1,1,1,1},
                           {1,1,1,1,1,1,1,1,1,1},
                           {1,1,1,0,1,1,1,0,1,1},
                           {1,0,0,0,0,0,0,1,1,0}, 
                           {1,1,1,1,1,0,1,1,0,0},
                           {0,0,0,1,1,1,1,1,0,0},
                           {1,1,0,0,1,0,1,1,0,0},
                           {1,1,1,0,1,1,0,0,0,0}
                       },
                       new int[16,10]
                       {
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0,0},
                           {1,1,1,0,1,1,1,1,1,1},
                           {0,1,0,0,1,1,1,0,1,1},
                           {0,1,0,0,1,1,1,0,1,1}, 
                           {1,1,1,0,1,1,1,0,1,1},
                           {1,0,0,0,0,0,0,1,1,0}, 
                           {1,1,1,1,1,0,1,1,0,0},
                           {0,0,0,1,1,1,1,1,0,0},
                           {1,1,0,0,1,0,1,1,0,0},
                           {1,1,1,0,1,1,0,0,0,0}
                       })
            };
        }

        [TestCase(9, 9)]
        [TestCase(7, 8)]
        [TestCase(5, -8)]
        [TestCase(3, -6)]
        [TestCase(5, 4)]
        [TestCase(0, 0)]
        [TestCase(-4, -9)]
        public void ThrowsExeptionWithValuesBelowTen(int rowCount, int columnCount) =>
            Assert.Throws<ArgumentException>(()=> new TetrisBoard(rowCount, columnCount));
        

        [TestCaseSource(nameof(DrawCases))]
        public void BoardAppliesRightCoordinates(int x, int y, int dropstatus, Tetromino tetromino)
        {
            TetrisBoard board = new (10, 10);

            int result = board.ShiftCoordinates(x, y, tetromino);
            Assert.AreEqual(dropstatus, result);
        }

        [TestCaseSource(nameof(Linecases))]
        public void BoardRemovesLinesCorrectly(int[,] givenBoard, int[,] expectedBoard)
        {
            // doesn't work
            // ToDo: make mock work
            // var boardMock = new Mock<Board>(16,10);
            // boardMock.SetupGet(board => board.Values).Returns(givenBoard);
            TetrisBoard board = new(16,10);
            board.Values = givenBoard;

            board.CountLines();
            Assert.AreEqual(expectedBoard, board.Values);

        }
    }
}