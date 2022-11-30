using Skotz.Chess.Tools.Game;

namespace Skotz.Chess.Tools.Notation
{
    public class SanNotation
    {
        private const string castleCharacter = "O";

        public static Move Parse(State state, string san)
        {
            return new Move();
        }

        public static string ToSan(State state, Move move)
        {
            var san = "";

            var source = move.Source.ToString().ToLower();
            var sourceFile = source[0].ToString();
            var sourceRank = source[1].ToString();
            var destination = move.Destination.ToString().ToLower();
            var destinationFile = destination[0].ToString();
            var destinationRank = destination[1].ToString();

            // Castling
            if (state[move.Source] == Piece.WhiteKing && move.Source == Square.E1 && move.Destination == Square.G1)
            {
                san = $"{castleCharacter}-{castleCharacter}";
            }
            else if (state[move.Source] == Piece.BlackKing && move.Source == Square.E8 && move.Destination == Square.G8)
            {
                san = $"{castleCharacter}-{castleCharacter}";
            }
            else if (state[move.Source] == Piece.WhiteKing && move.Source == Square.E1 && move.Destination == Square.C1)
            {
                san = $"{castleCharacter}-{castleCharacter}-{castleCharacter}";
            }
            else if (state[move.Source] == Piece.BlackKing && move.Source == Square.E8 && move.Destination == Square.C8)
            {
                san = $"{castleCharacter}-{castleCharacter}-{castleCharacter}";
            }
            else
            {
                // Piece
                if (state[move.Source] == Piece.WhiteKing || state[move.Source] == Piece.BlackKing)
                {
                    san += "K";
                }
                else if (state[move.Source] == Piece.WhiteQueen || state[move.Source] == Piece.BlackQueen)
                {
                    san += "Q";
                }
                else if (state[move.Source] == Piece.WhiteRook || state[move.Source] == Piece.BlackRook)
                {
                    san += "R";
                }
                else if (state[move.Source] == Piece.WhiteBishop || state[move.Source] == Piece.BlackBishop)
                {
                    san += "B";
                }
                else if (state[move.Source] == Piece.WhiteKnight || state[move.Source] == Piece.BlackKnight)
                {
                    san += "N";
                }
                else if (state[move.Source] == Piece.WhitePawn || state[move.Source] == Piece.BlackPawn)
                {
                    san += sourceFile;
                }

                // Captures
                if (state[move.Destination] != Piece.None || ((state[move.Source] == Piece.WhitePawn || state[move.Source] == Piece.BlackPawn) && move.Destination == state.EnPassant))
                {
                    san += "x";
                }

                // Destination
                san += destination;

                // Promotion
                if (move.Promotion == PieceType.Queen)
                {
                    san += "=Q";
                }
                else if (move.Promotion == PieceType.Rook)
                {
                    san += "=R";
                }
                else if (move.Promotion == PieceType.Bishop)
                {
                    san += "=B";
                }
                else if (move.Promotion == PieceType.Knight)
                {
                    san += "=N";
                }
            }

            // Check and checkmate
            var temp = state.Clone();
            temp.MakeMove(move);
            if (temp.IsCheckmate())
            {
                san += "#";
            }
            else if (temp.IsCheck())
            {
                san += "+";
            }

            return san;
        }
    }
}