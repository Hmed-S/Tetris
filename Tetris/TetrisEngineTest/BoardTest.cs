using NUnit.Framework;
using System;
using System.Collections.Generic;
using Engine;

namespace TetrisEngineTest
{
    public class BoardTest
    {
        
        public static IEnumerable<TestCaseData> DrawCases()
        {
            return new[]
            {   
                new TestCaseData(1,2, 0, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(1,5, 0, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(1,7, 0, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(1,8, -1, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(1,9, -1, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(1,10,-1, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(1,11,-1, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(1,1, 0, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(5,1, 0, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(7,1, 0, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(8,1, 0, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(9,1, -2, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(10,1,-2, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(11,1,-2, Tetronmino.FromShape(Shapes.JShape)),
                new TestCaseData(1,2, 0, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(1,5, 0, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(1,7, 0, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(1,8, 0, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(1,9, -1, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(1,10,-1, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(1,11,-1, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(4,1, 0, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(5,1, 0, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(6,1, 0, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(7,1, 0, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(8,1, -2, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(9,1, -2, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(10,1, -2, Tetronmino.FromShape(Shapes.LShape)),
                new TestCaseData(11,1, -2, Tetronmino.FromShape(Shapes.LShape))
            };
        }

        public static IEnumerable<TestCaseData> CollisionCases()
        {
            return new[]
            {
                new TestCaseData(1,1,Tetronmino.FromShape(Shapes.JShape),Tetronmino.FromShape(Shapes.OShape),1,1, 0,1),
                new TestCaseData(1,2,Tetronmino.FromShape(Shapes.JShape),Tetronmino.FromShape(Shapes.OShape),1,2, 0,2),
                new TestCaseData(1,3,Tetronmino.FromShape(Shapes.JShape),Tetronmino.FromShape(Shapes.OShape),1,2,-0,2),
                new TestCaseData(1,4,Tetronmino.FromShape(Shapes.JShape),Tetronmino.FromShape(Shapes.OShape),1,3,0,3),
                new TestCaseData(1,5,Tetronmino.FromShape(Shapes.JShape),Tetronmino.FromShape(Shapes.OShape),1,4,0,4),
                new TestCaseData(1,6,Tetronmino.FromShape(Shapes.JShape),Tetronmino.FromShape(Shapes.OShape),1,5,0,5),
                new TestCaseData(1,7,Tetronmino.FromShape(Shapes.JShape),Tetronmino.FromShape(Shapes.OShape),1,6,0,6),
                
                new TestCaseData(1,1,Tetronmino.FromShape(Shapes.SShape),Tetronmino.FromShape(Shapes.IShape),1,1,0,1),
                new TestCaseData(1,2,Tetronmino.FromShape(Shapes.SShape),Tetronmino.FromShape(Shapes.IShape),1,2,0,2),
                new TestCaseData(1,3,Tetronmino.FromShape(Shapes.SShape),Tetronmino.FromShape(Shapes.IShape),1,2,0,2),
                new TestCaseData(1,4,Tetronmino.FromShape(Shapes.SShape),Tetronmino.FromShape(Shapes.IShape),1,3,0,3),
                new TestCaseData(1,5,Tetronmino.FromShape(Shapes.SShape),Tetronmino.FromShape(Shapes.IShape),1,4,0,4),
                new TestCaseData(1,6,Tetronmino.FromShape(Shapes.SShape),Tetronmino.FromShape(Shapes.IShape),1,5,0,5),
                new TestCaseData(1,7,Tetronmino.FromShape(Shapes.SShape),Tetronmino.FromShape(Shapes.IShape),1,6,0,6),
                new TestCaseData(1,8,Tetronmino.FromShape(Shapes.SShape),Tetronmino.FromShape(Shapes.IShape),1,6,0,6),
                new TestCaseData(1,9,Tetronmino.FromShape(Shapes.SShape),Tetronmino.FromShape(Shapes.IShape),1,6,0,6)
                
                
            };
        }
        

        [Test]
        [TestCase(9, 9)]
        [TestCase(7, 8)]
        [TestCase(5, -8)]
        [TestCase(3, -6)]
        [TestCase(5, 4)]
        [TestCase(0, 0)]
        [TestCase(-4, -9)]
        public void ThrowsExeptionWithValuesBelowTen(int rowCount, int columnCount) =>
            Assert.Throws<ArgumentException>(()=> new Board(rowCount, columnCount));
        

        [TestCaseSource(nameof(DrawCases))]
        public void BoardAppliesRightCoordinates(int x, int y, int dropstatus, Tetronmino tetronmino)
        {
            Board board = new Board(10, 10);

            int result = board.ShiftCoordinates(x, y, tetronmino);
            Assert.AreEqual(dropstatus, result);
        }

        [TestCaseSource(nameof(CollisionCases))]
        public void CollisionDetection(int x, int y, Tetronmino tetronmino, Tetronmino tetronmino2, int x2, int y2, int expected, int ExpectedYPosition)
        {
            Board board = new Board(10, 10);
            board.AddTetromino(tetronmino);
            board.AddTetromino(tetronmino2);
            
            board.ShiftCoordinates(x, y, tetronmino);
            int result = board.ShiftCoordinates(x2, y2, tetronmino2);
            
            Assert.AreEqual(expected, result);
            Assert.AreEqual(ExpectedYPosition, tetronmino2.YPosition);
        }

    }
}