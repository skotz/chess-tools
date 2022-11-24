using FluentAssertions;
using Skotz.Chess.Tools.Extension;
using Skotz.Chess.Tools.Game;
using Xunit;

namespace Skotz.Chess.Tools.Tests.Extension
{
    public class SquareExtensionsTests
    {
        [Theory]
        [InlineData(Square.None, 2, 0, Square.None)]
        [InlineData(Square.A1, -1, 0, Square.None)]
        [InlineData(Square.E2, 0, 2, Square.E4)]
        [InlineData(Square.E2, 2, 2, Square.G4)]
        [InlineData(Square.H8, -2, -1, Square.F7)]
        [InlineData(Square.A1, 7, 7, Square.H8)]
        [InlineData(Square.B7, -1, 1, Square.A8)]
        public void GetRelativeSquares(Square square, int file, int rank, Square expected)
        {
            square.Transpose(file, rank).Should().Be(expected);
        }
    }
}