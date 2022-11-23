namespace Skotz.Chess.Tools.Game
{
    internal class Constants
    {
        internal static MoveDelta[] EmptyMoveDeltas = new MoveDelta[0];

        internal static MoveDelta[] DiagonalMoveDeltas = new MoveDelta[]
        {
            new MoveDelta(-1, -1),
            new MoveDelta(-1, 1),
            new MoveDelta(1, -1),
            new MoveDelta(1, 1),
        };

        internal static MoveDelta[] HorizontalVerticalMoveDeltas = new MoveDelta[]
        {
            new MoveDelta(0, 1),
            new MoveDelta(0, -1),
            new MoveDelta(1, 0),
            new MoveDelta(-1, 0),
        };

        internal static MoveDelta[] QueenMoveDeltas = new MoveDelta[]
        {
            new MoveDelta(-1, -1),
            new MoveDelta(-1, 1),
            new MoveDelta(1, -1),
            new MoveDelta(1, 1),
            new MoveDelta(0, 1),
            new MoveDelta(0, -1),
            new MoveDelta(1, 0),
            new MoveDelta(-1, 0),
        };

        internal static MoveDelta[] KnightMoveDeltas = new MoveDelta[]
        {
            new MoveDelta(-1, -2),
            new MoveDelta(1, -2),
            new MoveDelta(-2, -1),
            new MoveDelta(2, -1),
            new MoveDelta(-2, 1),
            new MoveDelta(2, 1),
            new MoveDelta(-1, 2),
            new MoveDelta(1, 2),
        };

        internal static MoveDelta[] WhitePawnMoveDeltas = new MoveDelta[]
        {
            new MoveDelta(1, -1),
            new MoveDelta(1, 1),
        };

        internal static MoveDelta[] BlackPawnMoveDeltas = new MoveDelta[]
        {
            new MoveDelta(-1, -1),
            new MoveDelta(-1, 1),
        };

        internal class MoveDelta
        {
            public int DeltaFile { get; set; }

            public int DeltaRank { get; set; }

            public MoveDelta(int deltaFile, int deltaRank)
            {
                DeltaFile = deltaFile;
                DeltaRank = deltaRank;
            }
        }
    }
}