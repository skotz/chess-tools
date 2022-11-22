namespace Skotz.Chess.Tools.Game
{
    public class State
    {
        public Piece[] Pieces;

        public Piece this[Square square]
        {
            get
            {
                return Pieces[(int)square];
            }
            set
            {
                Pieces[(int)square] = value;
            }
        }

        public Player PlayerToMove { get; set; }

        public bool CastleKingsideWhite { get; set; }

        public bool CastleQueensideWhite { get; set; }

        public bool CastleKingsideBlack { get; set; }

        public bool CastleQueensideBlack { get; set; }

        public Square EnPassant { get; set; }

        public int FullMoves { get; set; }

        public int HalfMovesSinceCaptureOrPawnMove { get; set; }

        public State()
        {
            Pieces = new Piece[8 * 8];
        }
    }
}