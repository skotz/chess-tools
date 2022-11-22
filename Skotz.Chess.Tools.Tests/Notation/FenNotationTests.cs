using FluentAssertions;
using Skotz.Chess.Tools.Game;
using Skotz.Chess.Tools.Notation;
using Xunit;

namespace Skotz.Chess.Tools.Tests.Notation
{
    public class FenNotationTests
    {
        [Theory]
        [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        public void ParseFenToStateAndBack(string fen)
        {
            var state = FenNotation.Parse(fen);
            var reverse = FenNotation.ToFen(state);

            reverse.Should().Be(fen);
        }

        [Fact]
        public void ParseStateWithEnPassant()
        {
            var fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";

            var state = FenNotation.Parse(fen);

            FenNotation.ToFen(state).Should().Be(fen);

            state.PlayerToMove.Should().Be(Player.Black);
            state.EnPassant.Should().Be(Square.E3);
            state[Square.E4].Should().Be(Piece.WhitePawn);
            state[Square.E2].Should().Be(Piece.None);
        }
    }
}