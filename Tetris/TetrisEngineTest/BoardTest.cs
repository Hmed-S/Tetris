using NUnit.Framework;
using System;
using Engine;

namespace TetrisEngineTest
{
    public class BoardTest
    {


        [Test]
        public void BoardHasRigthValues()
        {
            Board board = new (15, 10);

            int[,] expectedBoard = new int[,] 
            { 
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            };
            Assert.AreEqual(expectedBoard, board.Values);
        }

        [Test]
        public void BoardReplacesValuesCorrect()
        {
            Board board = new(15, 10);

            board.Replace(0, 4, 0,5, (i,j) => i + j);

            int[,] expectedBoard = new int[,]
            {
            { 0,1,2,3,4,0,0,0,0,0 },
            { 1,2,3,4,5,0,0,0,0,0 },
            { 2,3,4,5,6,0,0,0,0,0 },
            { 3,4,5,6,7,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            };
            Assert.AreEqual(expectedBoard, board.Values);
        }


        [Test]
        [TestCase(9, 9)]
        [TestCase(7, 8)]
        [TestCase(5, -8)]
        [TestCase(3, -6)]
        [TestCase(5, 4)]
        [TestCase(0, 0)]
        [TestCase(-4, -9)]
        public void ThrowsExeptionWithValuesBelowTen(int rowCount, int columnCount)
        {
            Assert.Throws<ArgumentException>(()=> new Board(rowCount, columnCount));
        }
    }
}