using Skotz.Chess.Tools.Extension;

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

        public State Clone()
        {
            var clone = (State)MemberwiseClone();
            clone.Pieces = (Piece[])Pieces.Clone();
            return clone;
        }

        public List<Move> GetAllMoves()
        {
            var moves = new List<Move>();

            for (var square = 0; square < 64; square++)
            {
                if (PlayerToMove == GetPlayer(square))
                {
                    moves.AddRange(GetMovesForPiece(square));
                }
            }

            // Remove moves that lead to check
            for (int i = moves.Count - 1; i >= 0; i--)
            {
                var temp = Clone();
                temp.MakeMove(moves[i]);
                if (temp.IsCheck(PlayerToMove))
                {
                    moves.RemoveAt(i);
                }
            }

            return moves;
        }

        private List<Move> GetMovesForPiece(int square)
        {
            var moves = new List<Move>();

            // Castling
            if (Pieces[square] == Piece.WhiteKing)
            {
                // Short
                if (CastleKingsideWhite)
                {
                    if (Pieces[(int)Square.E1] == Piece.WhiteKing &&
                        Pieces[(int)Square.H1] == Piece.WhiteRook &&
                        Pieces[(int)Square.F1] == Piece.None &&
                        Pieces[(int)Square.G1] == Piece.None &&
                        !IsSquareAttacked(Square.E1, Player.White) &&
                        !IsSquareAttacked(Square.F1, Player.White) &&
                        !IsSquareAttacked(Square.G1, Player.White))
                    {
                        moves.Add(new Move(Square.E1, Square.G1));
                    }
                }

                // Long
                if (CastleQueensideWhite)
                {
                    if (Pieces[(int)Square.E1] == Piece.WhiteKing &&
                        Pieces[(int)Square.A1] == Piece.WhiteRook &&
                        Pieces[(int)Square.D1] == Piece.None &&
                        Pieces[(int)Square.C1] == Piece.None &&
                        Pieces[(int)Square.B1] == Piece.None &&
                        !IsSquareAttacked(Square.E1, Player.White) &&
                        !IsSquareAttacked(Square.D1, Player.White) &&
                        !IsSquareAttacked(Square.C1, Player.White))
                    {
                        moves.Add(new Move(Square.E1, Square.C1));
                    }
                }
            }
            else if (Pieces[square] == Piece.BlackKing)
            {
                // Short
                if (CastleKingsideBlack)
                {
                    if (Pieces[(int)Square.E8] == Piece.BlackKing &&
                        Pieces[(int)Square.A8] == Piece.BlackRook &&
                        Pieces[(int)Square.F8] == Piece.None &&
                        Pieces[(int)Square.G8] == Piece.None &&
                        !IsSquareAttacked(Square.E8, Player.Black) &&
                        !IsSquareAttacked(Square.F8, Player.Black) &&
                        !IsSquareAttacked(Square.G8, Player.Black))
                    {
                        moves.Add(new Move(Square.E8, Square.C8));
                    }
                }

                // Long
                if (CastleQueensideBlack)
                {
                    if (Pieces[(int)Square.E8] == Piece.BlackKing &&
                        Pieces[(int)Square.A8] == Piece.BlackRook &&
                        Pieces[(int)Square.D8] == Piece.None &&
                        Pieces[(int)Square.C8] == Piece.None &&
                        Pieces[(int)Square.B8] == Piece.None &&
                        !IsSquareAttacked(Square.E8, Player.Black) &&
                        !IsSquareAttacked(Square.D8, Player.Black) &&
                        !IsSquareAttacked(Square.C8, Player.Black))
                    {
                        moves.Add(new Move(Square.E8, Square.C8));
                    }
                }
            }

            var deltas = Constants.EmptyMoveDeltas;
            var depth = 1;
            var breakOnFirst = true;
            var pawnOnStartingSquare = false;
            if (Pieces[square] == Piece.WhiteKing || Pieces[square] == Piece.BlackKing)
            {
                deltas = Constants.QueenMoveDeltas;
                depth = 1;
                breakOnFirst = false;
            }
            if (Pieces[square] == Piece.WhiteQueen || Pieces[square] == Piece.BlackQueen)
            {
                deltas = Constants.QueenMoveDeltas;
                depth = 8;
            }
            if (Pieces[square] == Piece.WhiteRook || Pieces[square] == Piece.BlackRook)
            {
                deltas = Constants.HorizontalVerticalMoveDeltas;
                depth = 8;
            }
            if (Pieces[square] == Piece.WhiteBishop || Pieces[square] == Piece.BlackBishop)
            {
                deltas = Constants.DiagonalMoveDeltas;
                depth = 8;
            }
            if (Pieces[square] == Piece.WhiteKnight || Pieces[square] == Piece.BlackKnight)
            {
                deltas = Constants.KnightMoveDeltas;
                depth = 1;
                breakOnFirst = false;
            }
            if (Pieces[square] == Piece.WhitePawn)
            {
                pawnOnStartingSquare = square.IsRank(2);
                deltas = Constants.WhitePawnMoveDeltas;
                depth = 1;
                breakOnFirst = false;
            }
            if (Pieces[square] == Piece.BlackPawn)
            {
                pawnOnStartingSquare = square.IsRank(7);
                deltas = Constants.BlackPawnMoveDeltas;
                depth = 1;
                breakOnFirst = false;
            }

            var self = GetPlayer(square);

            // Loop through directions
            for (var d = 0; d < deltas.Length; d++)
            {
                // Loop through movements within this direction in order
                for (var m = 1; m <= depth; m++)
                {
                    var capture = false;

                    if (Pieces[square] == Piece.WhitePawn || Pieces[square] == Piece.BlackPawn)
                    {
                        if (deltas[d].PawnOriginOnly)
                        {
                            if (!pawnOnStartingSquare)
                            {
                                // Can only jump two squares on the first move
                                continue;
                            }
                            else
                            {
                                // You can't jump over another piece
                                var skipSquare = square.Transpose(deltas[d - 1].DeltaFile, deltas[d - 1].DeltaRank);
                                if (GetPlayer(skipSquare) != Player.None)
                                {
                                    continue;
                                }
                            }
                        }

                        var nextSquare = square.Transpose(deltas[d].DeltaFile, deltas[d].DeltaRank);
                        if (nextSquare >= 0)
                        {
                            if (deltas[d].PawnCaptureOnly)
                            {
                                // Destination must either be an opponent piece or an en passant square
                                var targetPlayer = GetPlayer(nextSquare);
                                if (targetPlayer == Player.None && nextSquare != (int)EnPassant)
                                {
                                    // Can only capture diagonally onto a blank square for en passant
                                    continue;
                                }
                                else if (targetPlayer == self)
                                {
                                    // Can't en passant your own pieces
                                    continue;
                                }

                                capture = true;
                            }
                            else
                            {
                                // You can't capture a piece by moving forward
                                if (GetPlayer(nextSquare) != Player.None)
                                {
                                    continue;
                                }
                            }
                        }
                    }

                    var destination = square.Transpose(deltas[d].DeltaFile * m, deltas[d].DeltaRank * m);

                    // Off the board?
                    if (destination == (int)Square.None)
                    {
                        if (breakOnFirst)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    var destinationPlayer = GetPlayer(destination);

                    // Is the piece on this square friendly?
                    if (destinationPlayer == self)
                    {
                        if (breakOnFirst)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    // Is this move a capture?
                    if (destinationPlayer != Player.None)
                    {
                        capture = true;
                    }

                    // Promotions
                    if ((Pieces[square] == Piece.WhitePawn && destination.IsRank(8)) || (Pieces[square] == Piece.BlackPawn && destination.IsRank(1)))
                    {
                        // Enter all 4 possible promotion types
                        moves.Add(new Move(square, destination, PieceType.Queen));
                        moves.Add(new Move(square, destination, PieceType.Rook));
                        moves.Add(new Move(square, destination, PieceType.Bishop));
                        moves.Add(new Move(square, destination, PieceType.Knight));
                    }
                    else
                    {
                        // Enter a regular move
                        moves.Add(new Move(square, destination));
                    }

                    // We found a capture searching down this direction, so stop looking further
                    if (capture)
                    {
                        if (breakOnFirst)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            return moves;
        }

        private Square FindPiece(Piece piece)
        {
            for (int i = 0; i < 63; i++)
            {
                if (Pieces[i] == piece)
                {
                    return (Square)i;
                }
            }

            return Square.None;
        }

        public bool IsCheck()
        {
            return IsCheck(PlayerToMove);
        }

        private bool IsCheck(Player player)
        {
            if (player == Player.White)
            {
                return IsSquareAttacked(FindPiece(Piece.WhiteKing), Player.White);
            }
            else
            {
                return IsSquareAttacked(FindPiece(Piece.BlackKing), Player.Black);
            }
        }

        public bool IsCheckmate()
        {
            return IsCheck() && GetAllMoves().Count == 0;
        }

        private void MovePiece(Square source, Square destination)
        {
            Pieces[(int)destination] = Pieces[(int)source];
            Pieces[(int)source] = Piece.None;
        }

        public void MakeMove(Move move)
        {
            var piece = Pieces[(int)move.Source];
            var enpassant = (piece == Piece.WhitePawn || piece == Piece.BlackPawn) && Pieces[(int)move.Destination] == Piece.None && move.Source.GetFile() != move.Destination.GetFile();
            var capture = Pieces[(int)move.Destination] != Piece.None || enpassant;

            // Reset flags
            EnPassant = Square.None;

            // Move the piece
            MovePiece(move.Source, move.Destination);

            // Is it white to move?
            if (PlayerToMove == Player.White)
            {
                // Increment the half move count
                HalfMovesSinceCaptureOrPawnMove++;

                if (piece == Piece.WhiteKing)
                {
                    // Castling
                    if (move.Source == Square.E1)
                    {
                        if (move.Destination == Square.G1)
                        {
                            // Kingside
                            MovePiece(Square.H1, Square.F1);
                        }
                        else if (move.Destination == Square.C1)
                        {
                            // Queenside
                            MovePiece(Square.A1, Square.D1);
                        }
                    }

                    // The king moved, so you can't castle any more
                    CastleKingsideWhite = false;
                    CastleQueensideWhite = false;
                }
                else if (piece == Piece.WhiteRook)
                {
                    // Clear the corresponding castling flag for the side that this rook came from
                    if (move.Source == Square.A1)
                    {
                        CastleQueensideWhite = false;
                    }
                    if (move.Source == Square.H1)
                    {
                        CastleKingsideWhite = false;
                    }
                }
                else if (piece == Piece.WhitePawn)
                {
                    // Is this a 2 square jump? Set the en passant square
                    if (move.Source.IsRank(2) && move.Destination.IsRank(4))
                    {
                        EnPassant = move.Destination.Transpose(0, -1);
                    }

                    // Is this an en-passant capture?
                    if (enpassant)
                    {
                        // Remove the pawn that jumped past your destination square
                        Pieces[(int)move.Destination.Transpose(0, -1)] = Piece.None;
                    }

                    // Deal with promotions
                    if (move.Promotion == PieceType.Queen)
                    {
                        Pieces[(int)move.Destination] = Piece.WhiteQueen;
                    }
                    else if (move.Promotion == PieceType.Rook)
                    {
                        Pieces[(int)move.Destination] = Piece.WhiteRook;
                    }
                    else if (move.Promotion == PieceType.Bishop)
                    {
                        Pieces[(int)move.Destination] = Piece.WhiteBishop;
                    }
                    else if (move.Promotion == PieceType.Knight)
                    {
                        Pieces[(int)move.Destination] = Piece.WhiteKnight;
                    }
                }

                // It's now black's turn
                PlayerToMove = Player.Black;
            }
            else
            {
                // After each move by black we increment the full move counter
                FullMoves++;
                HalfMovesSinceCaptureOrPawnMove++;

                if (piece == Piece.BlackKing)
                {
                    // Castling
                    if (move.Source == Square.E8)
                    {
                        if (move.Destination == Square.G8)
                        {
                            // Kingside
                            MovePiece(Square.H8, Square.F8);
                        }
                        else if (move.Destination == Square.C8)
                        {
                            // Queenside
                            MovePiece(Square.A8, Square.D8);
                        }
                    }

                    // The king moved, so you can't castle any more
                    CastleKingsideBlack = false;
                    CastleQueensideBlack = false;
                }
                else if (piece == Piece.BlackRook)
                {
                    // Clear the corresponding castling flag for the side that this rook came from
                    if (move.Source == Square.A8)
                    {
                        CastleQueensideBlack = false;
                    }
                    if (move.Source == Square.H8)
                    {
                        CastleKingsideBlack = false;
                    }
                }
                else if (piece == Piece.BlackPawn)
                {
                    // Is this a 2 square jump? Set the en passant square
                    if (move.Source.IsRank(7) && move.Destination.IsRank(5))
                    {
                        EnPassant = move.Destination.Transpose(0, 1);
                    }

                    // Is this an en-passant capture?
                    if (enpassant)
                    {
                        // Remove the pawn that jumped past your destination square
                        Pieces[(int)move.Destination.Transpose(0, 1)] = Piece.None;
                    }

                    // Deal with promotions
                    if (move.Promotion == PieceType.Queen)
                    {
                        Pieces[(int)move.Destination] = Piece.BlackQueen;
                    }
                    else if (move.Promotion == PieceType.Rook)
                    {
                        Pieces[(int)move.Destination] = Piece.BlackRook;
                    }
                    else if (move.Promotion == PieceType.Bishop)
                    {
                        Pieces[(int)move.Destination] = Piece.BlackBishop;
                    }
                    else if (move.Promotion == PieceType.Knight)
                    {
                        Pieces[(int)move.Destination] = Piece.BlackKnight;
                    }
                }

                // It's now white's turn
                PlayerToMove = Player.White;
            }

            // Reset the half move counter on any capture or pawn move
            if (Pieces[(int)move.Source] == Piece.WhitePawn || Pieces[(int)move.Source] == Piece.BlackPawn || capture)
            {
                HalfMovesSinceCaptureOrPawnMove = 0;
            }
        }

        private Player GetPlayer(int square)
        {
            if (Pieces[square] == Piece.WhiteKing || Pieces[square] == Piece.WhiteQueen || Pieces[square] == Piece.WhiteRook || Pieces[square] == Piece.WhiteBishop || Pieces[square] == Piece.WhiteKnight || Pieces[square] == Piece.WhitePawn)
            {
                return Player.White;
            }
            else if (Pieces[square] == Piece.BlackKing || Pieces[square] == Piece.BlackQueen || Pieces[square] == Piece.BlackRook || Pieces[square] == Piece.BlackBishop || Pieces[square] == Piece.BlackKnight || Pieces[square] == Piece.BlackPawn)
            {
                return Player.Black;
            }
            else
            {
                return Player.None;
            }
        }

        private bool IsSquareAttacked(Square square, Player self)
        {
            return IsSquareAttacked((int)square, self);
        }

        private bool IsSquareAttacked(int square, Player self)
        {
            // Check for king captures
            for (var file = -1; file <= 1; file++)
            {
                for (var rank = -1; rank <= 1; rank++)
                {
                    if (file != 0 || rank != 0)
                    {
                        var destination = square.Transpose(file, rank);

                        // Off the board?
                        if (destination == (int)Square.None)
                        {
                            continue;
                        }

                        // Is the piece on this square friendly?
                        if (GetPlayer(destination) == self)
                        {
                            continue;
                        }

                        // Is there a king on this square that could attack us?
                        if (Pieces[destination] == Piece.WhiteKing || Pieces[destination] == Piece.BlackKing)
                        {
                            return true;
                        }
                    }
                }
            }

            // Check for diagonal captures
            for (var d = 0; d < 4; d++)
            {
                // Loop through movements within this direction in order
                for (var m = 1; m < 8; m++)
                {
                    var destination = square.Transpose(Constants.DiagonalMoveDeltas[d].DeltaFile * m, Constants.DiagonalMoveDeltas[d].DeltaRank * m);

                    // Off the board?
                    if (destination == (int)Square.None)
                    {
                        break;
                    }

                    // Is the piece on this square friendly?
                    if (GetPlayer(destination) == self)
                    {
                        break;
                    }

                    // Is there a bishop or queen on this square that could attack us?
                    if (Pieces[destination] == Piece.WhiteQueen || Pieces[destination] == Piece.BlackQueen || Pieces[destination] == Piece.WhiteBishop || Pieces[destination] == Piece.BlackBishop)
                    {
                        return true;
                    }

                    // There's a piece on this square that isn't ours, but it also can't attack us
                    if (GetPlayer(destination) != Player.None)
                    {
                        break;
                    }
                }
            }

            // Check for horizontal and vertical captures
            for (var d = 0; d < 4; d++)
            {
                // Loop through movements within this direction in order
                for (var m = 1; m < 8; m++)
                {
                    var destination = square.Transpose(Constants.HorizontalVerticalMoveDeltas[d].DeltaFile * m, Constants.HorizontalVerticalMoveDeltas[d].DeltaRank * m);

                    // Off the board?
                    if (destination == (int)Square.None)
                    {
                        break;
                    }

                    // Is the piece on this square friendly?
                    if (GetPlayer(destination) == self)
                    {
                        break;
                    }

                    // Is there a rook or queen on this square that could attack us?
                    if (Pieces[destination] == Piece.WhiteQueen || Pieces[destination] == Piece.BlackQueen || Pieces[destination] == Piece.WhiteRook || Pieces[destination] == Piece.BlackRook)
                    {
                        return true;
                    }

                    // There's a piece on this square that isn't ours, but it also can't attack us
                    if (GetPlayer(destination) != Player.None)
                    {
                        break;
                    }
                }
            }

            // Check for knight captures
            for (var d = 0; d < 8; d++)
            {
                var destination = square.Transpose(Constants.KnightMoveDeltas[d].DeltaFile, Constants.KnightMoveDeltas[d].DeltaRank);

                // Off the board?
                if (destination == (int)Square.None)
                {
                    continue;
                }

                // Is the piece on this square friendly?
                if (GetPlayer(destination) == self)
                {
                    continue;
                }

                // Is there a knight on this square that could attack us?
                if (Pieces[destination] == Piece.WhiteKnight || Pieces[destination] == Piece.BlackKnight)
                {
                    return true;
                }
            }

            // Check for pawn captures
            for (var d = 0; d < 2; d++)
            {
                var deltas = self == Player.White ? Constants.WhitePawnAttackDeltas : Constants.BlackPawnAttackDeltas;
                var destination = square.Transpose(deltas[d].DeltaFile, deltas[d].DeltaRank);

                // Off the board?
                if (destination == (int)Square.None)
                {
                    continue;
                }

                // Is the piece on this square friendly?
                if (GetPlayer(destination) == self)
                {
                    continue;
                }

                // Is there a pawn on this square that could attack us?
                if (Pieces[destination] == Piece.WhitePawn || Pieces[destination] == Piece.BlackPawn)
                {
                    return true;
                }
            }

            return false;
        }
    }
}