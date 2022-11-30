using FluentAssertions;
using Skotz.Chess.Tools.Game;
using Skotz.Chess.Tools.Notation;
using Xunit;

namespace Skotz.Chess.Tools.Tests.Notation
{
    public class SanNotationTests
    {
        [Theory]
        [InlineData("5k2/8/8/8/8/8/8/4K2R w K - 0 1", Square.E1, Square.G1, PieceType.None, "O-O+")]
        [InlineData("4rkr1/4r1r1/3B3B/8/8/8/4P1PP/4K2R w K - 0 1", Square.E1, Square.G1, PieceType.None, "O-O#")]
        [InlineData("r1bqkbnr/ppp1pppp/2n5/3pP3/8/8/PPPP1PPP/RNBQKBNR w KQkq d6 0 3", Square.E5, Square.D6, PieceType.None, "exd6")]
        [InlineData("1rbqkb1r/ppP1pppp/2n2n2/8/8/8/PPPP1PPP/RNBQKBNR w KQk - 1 5", Square.C7, Square.B8, PieceType.Queen, "cxb8=Q")]
        [InlineData("3bkb2/3p1p2/8/8/4N3/8/8/3KR3 w - - 0 1", Square.E4, Square.D6, PieceType.None, "Nd6#")]
        [InlineData("kb6/pp6/3q2Q1/8/8/3Q2Q1/PP6/KB6 w - - 0 1", Square.G3, Square.D6, PieceType.None, "Qg3xd6")]
        [InlineData("kb6/pp6/3q2Q1/8/8/3Q4/PP6/KB6 w - - 0 1", Square.D3, Square.D6, PieceType.None, "Qdxd6")]
        [InlineData("kb6/pp6/3q2Q1/8/8/6Q1/PP6/KB6 w - - 0 1", Square.G6, Square.D6, PieceType.None, "Q6xd6")]
        public void ParseSanMove(string fen, Square source, Square destination, PieceType promotion, string expected)
        {
            var move = new Move(source, destination, promotion);
            var state = FenNotation.Parse(fen);
            var san = SanNotation.ToSan(state, move);

            san.Should().Be(expected);
        }
    }
}