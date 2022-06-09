using NUnit.Framework;
using System;
using System.Collections.Generic;
using Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

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
    }
}