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

        public List<Move> GetAllMoves()
        {
            var moves = new List<Move>();

            for (var square = 0; square < 64; square++)
            {
                if (PlayerToMove == Player.White)
                {
                    if (Pieces[square] == Piece.WhiteKing)
                    {
                        moves.AddRange(GetMovesForPiece(square));
                    }
                    else if (Pieces[square] == Piece.WhiteQueen)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_Q, my_pieces, enemy_pieces, true, capturesOnly);
                    }
                    else if (Pieces[square] == Piece.WhiteRook)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_R, my_pieces, enemy_pieces, true, capturesOnly);
                    }
                    else if (Pieces[square] == Piece.WhiteBishop)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_B, my_pieces, enemy_pieces, true, capturesOnly);
                    }
                    else if (Pieces[square] == Piece.WhiteKnight)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_N, my_pieces, enemy_pieces, true, capturesOnly);
                    }
                    else if (Pieces[square] == Piece.WhitePawn)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_P, my_pieces, enemy_pieces, true, capturesOnly);
                    }
                }
                else if (PlayerToMove == Player.Black)
                {
                    if ((position.b_king & square_mask) != 0UL)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_K, my_pieces, enemy_pieces, false, capturesOnly);
                    }
                    else if ((position.b_queen & square_mask) != 0UL)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_Q, my_pieces, enemy_pieces, false, capturesOnly);
                    }
                    else if ((position.b_rook & square_mask) != 0UL)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_R, my_pieces, enemy_pieces, false, capturesOnly);
                    }
                    else if ((position.b_bishop & square_mask) != 0UL)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_B, my_pieces, enemy_pieces, false, capturesOnly);
                    }
                    else if ((position.b_knight & square_mask) != 0UL)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_N, my_pieces, enemy_pieces, false, capturesOnly);
                    }
                    else if ((position.b_pawn & square_mask) != 0UL)
                    {
                        GetMovesForPiece(ref moves, ref position, square, square_mask, Constants.piece_P, my_pieces, enemy_pieces, false, capturesOnly);
                    }
                }
            }

            return moves;
        }

        private List<Move> GetMovesForPiece(int square)
        {
            var moves = new List<Move>();

            var capture = false;
            var promotion = false;

            // Castling
            if (Pieces[square] == Piece.WhiteKing || Pieces[square] == Piece.BlackKing)
            {
                if (PlayerToMove == Player.White)
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
                else
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
            }

            var deltas = Constants.EmptyMoveDeltas;
            var depth = 1;
            if (Pieces[square] == Piece.WhiteKing || Pieces[square] == Piece.BlackKing)
            {
                deltas = Constants.QueenMoveDeltas;
                depth = 1;
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
            }
            if (Pieces[square] == Piece.WhitePawn)
            {
                deltas = Constants.WhitePawnMoveDeltas;
                depth = 1;
            }
            if (Pieces[square] == Piece.BlackPawn)
            {
                deltas = Constants.BlackPawnMoveDeltas;
                depth = 1;
            }

            var self = GetPlayer(square);

            // Loop through directions
            for (var d = 0; d < deltas.Length; d++)
            {
                // Loop through movements within this direction in order
                for (var m = 1; m <= depth; m++)
                {
                    var destination = square.Transpose(deltas[d].DeltaFile * m, deltas[d].DeltaRank * m);

                    // Off the board?
                    if (destination == (int)Square.None)
                    {
                        break;
                    }

                    var destinationPlayer = GetPlayer(destination);

                    // Is the piece on this square friendly?
                    if (destinationPlayer == self)
                    {
                        break;
                    }

                    capture = false;

                    // Is this move a capture?
                    if (destinationPlayer != Player.None)
                    {
                        capture = true;
                    }

                    // Take care of all the joys of pawn calculation...
                    if (Pieces[square] == Piece.WhitePawn || Pieces[square] == Piece.BlackPawn)
                    {
                        if (PlayerToMove == Player.White)
                        {
                            // En-passant - run BEFORE general capture checking since this won't normally be considered a capture (no piece on target square)
                            if (EnPassant != Square.None && destination == (int)EnPassant)
                            {
                                moveflags |= Constants.move_flag_is_en_passent;
                                moveflags |= Constants.move_flag_is_capture;
                            }
                            else
                            {
                                // Make sure the pawns only move sideways if they are capturing
                                if (destination == (square_mask << 9) && !capture)
                                {
                                    break;
                                }
                                if (destination == (square_mask << 7) && !capture)
                                {
                                    break;
                                }
                            }

                            // Make sure pawns don't try to capture on an initial 2 square jump or general forward move
                            if (destination == (square_mask << 16) && capture)
                            {
                                break;
                            }
                            if (destination == (square_mask << 8) && capture)
                            {
                                break;
                            }

                            // Deal with promotions
                            if ((destination & 0xFF00000000000000UL) != 0UL)
                            {
                                promotion = true;
                            }
                        }
                        else
                        {
                            // Make sure the pawns don't try to move backwards
                            if (destination > square_mask)
                            {
                                break;
                            }

                            // En-passant - run BEFORE general capture checking since this won't normally be considered a capture (no piece on target square)
                            if (destination == position.en_passent_square && position.en_passent_square != 0UL)
                            {
                                moveflags |= Constants.move_flag_is_en_passent;
                                moveflags |= Constants.move_flag_is_capture;
                            }
                            else
                            {
                                // Make sure the pawns only move sideways if they are capturing
                                if (destination == (square_mask >> 9) && !capture)
                                {
                                    break;
                                }
                                if (destination == (square_mask >> 7) && !capture)
                                {
                                    break;
                                }
                            }

                            // Make sure pawns don't try to capture on an initial 2 square jump or general forward move
                            if (destination == (square_mask >> 16) && capture)
                            {
                                break;
                            }
                            if (destination == (square_mask >> 8) && capture)
                            {
                                break;
                            }

                            // Deal with promotions
                            if ((destination & 0x00000000000000FFUL) != 0UL)
                            {
                                promotion = true;
                            }
                        }
                    }

                    if (promotion)
                    {
                        // Enter all 4 possible promotion types
                        moves.Add(new Move()
                        {
                            mask_from = square_mask,
                            mask_to = destination,
                            flags = moveflags | Constants.move_flag_is_promote_bishop,
                            from_piece_type = pieceType
                        });
                        moves.Add(new Move()
                        {
                            mask_from = square_mask,
                            mask_to = destination,
                            flags = moveflags | Constants.move_flag_is_promote_knight,
                            from_piece_type = pieceType
                        });
                        moves.Add(new Move()
                        {
                            mask_from = square_mask,
                            mask_to = destination,
                            flags = moveflags | Constants.move_flag_is_promote_rook,
                            from_piece_type = pieceType
                        });
                        moves.Add(new Move()
                        {
                            mask_from = square_mask,
                            mask_to = destination,
                            flags = moveflags | Constants.move_flag_is_promote_queen,
                            from_piece_type = pieceType
                        });
                    }
                    else
                    {
                        // Enter a regular move
                        moves.Add(new Move()
                        {
                            mask_from = square_mask,
                            mask_to = destination,
                            flags = moveflags,
                            from_piece_type = pieceType
                        });
                    }

                    // We found a capture searching down this direction, so stop looking further
                    if (capture)
                    {
                        break;
                    }
                }
            }

            return moves;
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
            for (var d = 0; d < 8; d++)
            {
                var deltas = self == Player.White ? Constants.WhitePawnMoveDeltas : Constants.BlackPawnMoveDeltas;
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