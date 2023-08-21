using FluentAssertions;
using Skotz.Chess.Tools.Game;
using Skotz.Chess.Tools.Notation;
using Xunit;

namespace Skotz.Chess.Tools.Tests.Notation
{
    public class SquareNotationTests
    {
        [Theory]
        [InlineData("e1g1", Square.E1, Square.G1, PieceType.None)]
        [InlineData("e5d6ep", Square.E5, Square.D6, PieceType.None)]
        [InlineData("c7b8=q", Square.C7, Square.B8, PieceType.Queen)]
        public void ParseSanStringMove(string move, Square expectedSource, Square expectedDestination, PieceType expectedPromotion)
        {
            var parsed = SquareNotation.Parse(move);

            parsed.Source.Should().Be(expectedSource);
            parsed.Destination.Should().Be(expectedDestination);
            parsed.Promotion.Should().Be(expectedPromotion);
        }
    }
}