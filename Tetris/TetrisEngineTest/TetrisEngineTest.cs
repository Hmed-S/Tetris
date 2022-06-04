using NUnit.Framework;
using Engine;

namespace TetrisEngineTest
{
    public class TetrisEngineTest
    {
        [Test]
        public void TetrominoIsOnTopOnStart()
        {
            TetrisEngine engine = new()
            {
                Board = new Board(10, 10)
            };

            int[,] expectedBoard = new int[,]
            {
            { 0,0,1,0,0,0,0,0,0,0 },
            { 1,1,1,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0 },
            };
            Assert.AreEqual(expectedBoard, engine.Board.Values);
        }
    }
}
