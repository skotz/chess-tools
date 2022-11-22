using Skotz.Chess.Tools.Game;

namespace Skotz.Chess.Tools.Notation
{
    public class FenNotation
    {
        public static State Parse(string fen)
        {
            var state = new State();

            var parts = fen.Split(' ').ToList();

            // Piece Placement
            var square = Square.A8;
            foreach (var row in parts[0].Split('/'))
            {
                foreach (var piece in row)
                {
                    switch (piece)
                    {
                        case 'p':
                            state[square] = Piece.BlackPawn;
                            break;

                        case 'P':
                            state[square] = Piece.WhitePawn;
                            break;

                        case 'r':
                            state[square] = Piece.BlackRook;
                            break;

                        case 'R':
                            state[square] = Piece.WhiteRook;
                            break;

                        case 'b':
                            state[square] = Piece.BlackBishop;
                            break;

                        case 'B':
                            state[square] = Piece.WhiteBishop;
                            break;

                        case 'n':
                            state[square] = Piece.BlackKnight;
                            break;

                        case 'N':
                            state[square] = Piece.WhiteKnight;
                            break;

                        case 'q':
                            state[square] = Piece.BlackQueen;
                            break;

                        case 'Q':
                            state[square] = Piece.WhiteQueen;
                            break;

                        case 'k':
                            state[square] = Piece.BlackKing;
                            break;

                        case 'K':
                            state[square] = Piece.WhiteKing;
                            break;

                        default:
                            if (int.TryParse(piece.ToString(), out int skip))
                            {
                                square += skip - 1;
                            }
                            break;
                    }
                    square++;
                }
            }

            // Side to Move
            state.PlayerToMove = parts[1].ToLower() == "w" ? Player.White : Player.Black;

            // Castling Rights
            state.CastleKingsideWhite = parts[2].Contains("K");
            state.CastleKingsideBlack = parts[2].Contains("k");
            state.CastleQueensideWhite = parts[2].Contains("Q");
            state.CastleQueensideBlack = parts[2].Contains("q");

            // En-Passant Square
            if (!string.IsNullOrEmpty(parts[3]))
            {
                if (Enum.TryParse(parts[3]?.ToUpper(), out Square enPassantSquare))
                {
                    state.EnPassant = enPassantSquare;
                }
                else
                {
                    state.EnPassant = Square.None;
                }
            }

            // Move Numbers
            if (int.TryParse(parts[4], out int halfMoves))
            {
                state.HalfMovesSinceCaptureOrPawnMove = halfMoves;
            }
            if (int.TryParse(parts[5], out int fullMoves))
            {
                state.FullMoves = fullMoves;
            }

            return state;
        }

        public static string ToFen(State state)
        {
            var fen = "";

            var blanks = 0;
            var addBlanks = () =>
            {
                var count = blanks > 0 ? blanks.ToString() : "";
                blanks = 0;
                return count;
            };

            // Piece Placement
            for (var square = Square.A8; square <= Square.H1; square++)
            {
                if (state[square] == Piece.WhiteKing)
                {
                    fen += addBlanks() + "K";
                }
                else if (state[square] == Piece.WhiteQueen)
                {
                    fen += addBlanks() + "Q";
                }
                else if (state[square] == Piece.WhiteRook)
                {
                    fen += addBlanks() + "R";
                }
                else if (state[square] == Piece.WhiteBishop)
                {
                    fen += addBlanks() + "B";
                }
                else if (state[square] == Piece.WhiteKnight)
                {
                    fen += addBlanks() + "N";
                }
                else if (state[square] == Piece.WhitePawn)
                {
                    fen += addBlanks() + "P";
                }
                else if (state[square] == Piece.BlackKing)
                {
                    fen += addBlanks() + "k";
                }
                else if (state[square] == Piece.BlackQueen)
                {
                    fen += addBlanks() + "q";
                }
                else if (state[square] == Piece.BlackRook)
                {
                    fen += addBlanks() + "r";
                }
                else if (state[square] == Piece.BlackBishop)
                {
                    fen += addBlanks() + "b";
                }
                else if (state[square] == Piece.BlackKnight)
                {
                    fen += addBlanks() + "n";
                }
                else if (state[square] == Piece.BlackPawn)
                {
                    fen += addBlanks() + "p";
                }
                else
                {
                    blanks++;
                }

                if (square == Square.H8 || square == Square.H7 || square == Square.H6 || square == Square.H5 || square == Square.H4 || square == Square.H3 || square == Square.H2)
                {
                    fen += addBlanks() + "/";
                }
            }
            fen += addBlanks();

            // Side to Move
            fen += " " + (state.PlayerToMove == Player.White ? "w" : "b");

            // Castling Rights
            fen += " ";
            var castle = 0;
            if (state.CastleKingsideWhite)
            {
                fen += "K";
                castle++;
            }
            if (state.CastleQueensideWhite)
            {
                fen += "Q";
                castle++;
            }
            if (state.CastleKingsideBlack)
            {
                fen += "k";
                castle++;
            }
            if (state.CastleQueensideBlack)
            {
                fen += "q";
                castle++;
            }
            if (castle == 0)
            {
                fen += "-";
            }

            // En-Passant Square
            fen += " " + (state.EnPassant != Square.None ? state.EnPassant.ToString().ToLower() : "-");

            // Move Numbers
            fen += " " + state.HalfMovesSinceCaptureOrPawnMove + " " + state.FullMoves;

            return fen;
        }
    }
}