using FluentAssertions;
using Skotz.Chess.Tools.Game;
using Skotz.Chess.Tools.Notation;
using Xunit;

namespace Skotz.Chess.Tools.Tests.Game
{
    public class StateTests
    {
        [Fact]
        public void CheckForValidPawnMoves()
        {
            var state = FenNotation.Parse("7k/1P6/8/3pP3/6p1/1p2p3/2P1P1P1/7K w - d6 0 2");
            var moves = state.GetAllMoves();

            moves.Count.Should().Be(12);

            moves.Should().Contain(x => x.Source == Square.G2 && x.Destination == Square.G3);
            moves.Should().NotContain(x => x.Source == Square.G2 && x.Destination == Square.G4);
            moves.Should().NotContain(x => x.Source == Square.G2 && x.Destination == Square.F3);
            moves.Should().NotContain(x => x.Source == Square.G2 && x.Destination == Square.H3);

            moves.Should().NotContain(x => x.Source == Square.E2);

            moves.Should().Contain(x => x.Source == Square.C2 && x.Destination == Square.C3);
            moves.Should().Contain(x => x.Source == Square.C2 && x.Destination == Square.C4);
            moves.Should().Contain(x => x.Source == Square.C2 && x.Destination == Square.B3);
            moves.Should().NotContain(x => x.Source == Square.C2 && x.Destination == Square.D3);

            moves.Should().Contain(x => x.Source == Square.E5 && x.Destination == Square.E6);
            moves.Should().Contain(x => x.Source == Square.E5 && x.Destination == Square.D6); // En passant

            moves.Should().Contain(x => x.Source == Square.B7 && x.Destination == Square.B8 && x.Promotion == PieceType.Queen);
            moves.Should().Contain(x => x.Source == Square.B7 && x.Destination == Square.B8 && x.Promotion == PieceType.Rook);
            moves.Should().Contain(x => x.Source == Square.B7 && x.Destination == Square.B8 && x.Promotion == PieceType.Bishop);
            moves.Should().Contain(x => x.Source == Square.B7 && x.Destination == Square.B8 && x.Promotion == PieceType.Knight);
        }

        [Fact]
        public void CaptureDoesNotLeadToCheck()
        {
            var state = FenNotation.Parse("8/8/k7/r2pP2K/8/8/8/8 w - d6 0 2");
            var moves = state.GetAllMoves();

            moves.Should().NotContain(x => x.Source == Square.E5 && x.Destination == Square.D6);
        }
    }
}