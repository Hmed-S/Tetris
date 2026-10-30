using TetrisEngine.Domain.Board;

namespace UnitTest.Domain;

public class TetrisBoardTest
{
    private static TetrisBoard CreateBoard(int height = 10, int width = 10)
        => new(height, width);

    private static Tetromino CreateOPiece(int x = 0, int y = 0)
        => Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(x, y);

    private static void FillRow(TetrisBoard board, int row)
    {
        for (int col = 0; col < board.Width; col++)
            board.Values[row, col] = 1;
    }

    private static bool RowIsEmpty(TetrisBoard board, int row)
    {
        for (int col = 0; col < board.Width; col++)
        {
            if (board.Values[row, col] != 0)
                return false;
        }

        return true;
    }

    // Constructor

    [Theory]
    [InlineData(9, 10)]
    [InlineData(10, 9)]
    [InlineData(0, 10)]
    [InlineData(10, 0)]
    public void Constructor_InvalidDimensions_ThrowsArgumentException(
        int height, int width)
    {
        Assert.Throws<ArgumentException>(
            () => new TetrisBoard(height, width));
    }

    [Theory]
    [InlineData(10, 10)]
    [InlineData(20, 10)]
    [InlineData(10, 15)]
    public void Constructor_ValidDimensions_CreatesEmptyBoard(
        int height, int width)
    {
        var board = CreateBoard(height, width);

        Assert.Equal(height, board.Height);
        Assert.Equal(width, board.Width);
        Assert.Equal(0, board.Lines);
        Assert.Equal(height, board.Values.GetLength(0));
        Assert.Equal(width, board.Values.GetLength(1));

        Assert.All(board.Values.Cast<int>(), value =>
            Assert.Equal(0, value));
    }

    // ClearRow

    [Fact]
    public void ClearRow_PopulatedRow_ClearsOnlySpecifiedRow()
    {
        var board = CreateBoard();

        for (int col = 0; col < board.Width; col++)
        {
            board.Values[4, col] = 1;
            board.Values[5, col] = 1;
        }

        board.ClearRow(4);

        Assert.True(RowIsEmpty(board, 4));

        for (int col = 0; col < board.Width; col++)
            Assert.Equal(1, board.Values[5, col]);
    }

    [Fact]
    public void ClearRow_EmptyRow_RemainsEmpty()
    {
        var board = CreateBoard();

        board.ClearRow(4);

        Assert.True(RowIsEmpty(board, 4));
    }

    // CanFit

    [Fact]
    public void CanFit_EmptyBoard_ReturnsTrue()
    {
        var board = CreateBoard();

        Assert.True(board.CanFit(CreateOPiece()));
    }

    [Fact]
    public void CanFit_OverlappingOccupiedCell_ReturnsFalse()
    {
        var board = CreateBoard();
        board.Values[0, 0] = 1;

        Assert.False(board.CanFit(CreateOPiece()));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(9, 0)]
    [InlineData(0, 9)]
    public void CanFit_PieceOutsideBoard_ReturnsFalse(int x, int y)
    {
        var board = CreateBoard();

        Assert.False(board.CanFit(CreateOPiece(x, y)));
    }

    [Fact]
    public void CanFit_PieceAtRightAndBottomEdges_ReturnsTrue()
    {
        var board = CreateBoard();

        // An O piece occupies a 2x2 area.
        Assert.True(board.CanFit(CreateOPiece(8, 8)));
    }

    [Fact]
    public void CanFit_NonzeroCellIsOccupied_ReturnsFalse()
    {
        var board = CreateBoard();
        board.Values[1, 1] = 2;

        Assert.False(board.CanFit(CreateOPiece()));
    }

    // IsToppedOut

    [Fact]
    public void IsToppedOut_OverlapsOccupiedCell_ReturnsTrue()
    {
        var board = CreateBoard();
        board.Values[0, 0] = 1;

        Assert.True(board.IsToppedOut(CreateOPiece()));
    }

    [Fact]
    public void IsToppedOut_NoOverlap_ReturnsFalse()
    {
        var board = CreateBoard();

        Assert.False(board.IsToppedOut(CreateOPiece()));
    }

    [Fact]
    public void IsToppedOut_PieceAboveBoard_IgnoresNegativeRows()
    {
        var board = CreateBoard();

        // The O piece occupies rows -1 and 0.
        var piece = CreateOPiece(0, -1);

        Assert.False(board.IsToppedOut(piece));
    }

    [Fact]
    public void IsToppedOut_NegativeRowsAndOccupiedVisibleCell_ReturnsTrue()
    {
        var board = CreateBoard();
        board.Values[0, 0] = 1;

        var piece = CreateOPiece(0, -1);

        Assert.True(board.IsToppedOut(piece));
    }

    // WriteTetromino

    [Fact]
    public void WriteTetromino_WritesAllOccupiedPieceCells()
    {
        var board = CreateBoard();
        var piece = CreateOPiece(3, 4);

        board.WriteTetromino(piece);

        Assert.Equal(1, board.Values[4, 3]);
        Assert.Equal(1, board.Values[4, 4]);
        Assert.Equal(1, board.Values[5, 3]);
        Assert.Equal(1, board.Values[5, 4]);
    }

    // Clearlines

    [Fact]
    public void Clearlines_NoFullRows_ReturnsZeroAndDoesNotInvokeCallbacks()
    {
        var board = CreateBoard();
        board.Values[4, 0] = 1;

        var erasedRows = new List<int>();
        var shiftedRows = new List<(int NewRow, int OldRow)>();

        int cleared = board.Clearlines(
            new List<Action<int>> { row => erasedRows.Add(row) },
            new List<Action<int, int>>
            {
                (newRow, oldRow) => shiftedRows.Add((newRow, oldRow))
            });

        Assert.Equal(0, cleared);
        Assert.Empty(erasedRows);
        Assert.Empty(shiftedRows);
    }

    [Fact]
    public void Clearlines_OneFullRow_ClearsRowAndInvokesEraseCallback()
    {
        var board = CreateBoard();
        FillRow(board, 5);

        var erasedRows = new List<int>();

        int cleared = board.Clearlines(
            new List<Action<int>> { row => erasedRows.Add(row) },
            new List<Action<int, int>>());

        Assert.Equal(1, cleared);
        Assert.True(RowIsEmpty(board, 5));
        Assert.Equal(new[] { 5 }, erasedRows);
    }

    [Fact]
    public void Clearlines_OneFullRow_ShiftsRowsAboveDown()
    {
        var board = CreateBoard();

        // Give each row above the cleared row a recognizable value.
        for (int row = 0; row < 5; row++)
            board.Values[row, 0] = row + 2;

        FillRow(board, 5);

        int cleared = board.Clearlines(
            new List<Action<int>>(),
            new List<Action<int, int>>());

        Assert.Equal(1, cleared);

        // The previous row 4 moves to row 5.
        Assert.Equal(6, board.Values[5, 0]);

        // The previous row 0 moves to row 1.
        Assert.Equal(2, board.Values[1, 0]);

        // The top row becomes empty.
        Assert.True(RowIsEmpty(board, 0));
    }

    [Fact]
    public void Clearlines_ConsecutiveFullRows_ClearsBothRows()
    {
        var board = CreateBoard();

        FillRow(board, 5);
        FillRow(board, 6);

        var erasedRows = new List<int>();

        int cleared = board.Clearlines(
            new List<Action<int>> { row => erasedRows.Add(row) },
            new List<Action<int, int>>());

        Assert.Equal(2, cleared);
        Assert.True(RowIsEmpty(board, 5));
        Assert.True(RowIsEmpty(board, 6));
        Assert.Equal(new[] { 5, 6 }, erasedRows);
    }

    [Fact]
    public void Clearlines_TwoSeparatedFullRows_ClearsBothRows()
    {
        var board = CreateBoard();

        FillRow(board, 2);
        FillRow(board, 7);

        int cleared = board.Clearlines(
            new List<Action<int>>(),
            new List<Action<int, int>>());

        Assert.Equal(2, cleared);
        Assert.True(RowIsEmpty(board, 2));
        Assert.True(RowIsEmpty(board, 7));
    }

    [Fact]
    public void Clearlines_FullTopRow_ClearsTopRow()
    {
        var board = CreateBoard();
        FillRow(board, 0);

        int cleared = board.Clearlines(
            new List<Action<int>>(),
            new List<Action<int, int>>());

        Assert.Equal(1, cleared);
        Assert.True(RowIsEmpty(board, 0));
    }

    [Fact]
    public void Clearlines_FullBottomRow_ClearsBottomRow()
    {
        var board = CreateBoard();
        FillRow(board, board.Height - 1);

        int cleared = board.Clearlines(
            new List<Action<int>>(),
            new List<Action<int, int>>());

        Assert.Equal(1, cleared);
        Assert.True(RowIsEmpty(board, board.Height - 1));
    }

    [Fact]
    public void Clearlines_ShiftingRows_InvokesRowSwabCallbacks()
    {
        var board = CreateBoard();
        FillRow(board, 5);

        var shiftedRows = new List<(int NewRow, int OldRow)>();

        board.Clearlines(
            new List<Action<int>>(),
            new List<Action<int, int>>
            {
                (newRow, oldRow) => shiftedRows.Add((newRow, oldRow))
            });

        // Clearing row 5 shifts each row above it down one position.
        Assert.Equal(5, shiftedRows.Count);

        Assert.Equal(
            new[] { (5, 4), (4, 3), (3, 2), (2, 1), (1, 0) },
            shiftedRows);
    }

    [Fact]
    public void Clearlines_MultipleCallbacks_InvokesEveryCallback()
    {
        var board = CreateBoard();
        FillRow(board, 5);

        int firstCount = 0;
        int secondCount = 0;

        board.Clearlines(
            new List<Action<int>>
            {
                _ => firstCount++,
                _ => secondCount++
            },
            new List<Action<int, int>>());

        Assert.Equal(1, firstCount);
        Assert.Equal(1, secondCount);
    }

}
