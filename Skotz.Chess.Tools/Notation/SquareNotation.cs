using Skotz.Chess.Tools.Game;

namespace Skotz.Chess.Tools.Notation
{
    public class SquareNotation
    {
        public static Move Parse(string move)
        {
            var parsed = new Move();

            if (!string.IsNullOrEmpty(move) && move.Length < 4)
            {
                return parsed;
            }

            var from = move.Substring(0, 2);
            var to = move.Substring(2, 2);
            var extra = move.Length > 4 ? move.Substring(4) : "";

            parsed.Source = (Square)Enum.Parse(typeof(Square), from.ToUpper());
            parsed.Destination = (Square)Enum.Parse(typeof(Square), to.ToUpper());
            parsed.Promotion = PieceType.None;

            switch (extra.ToLower())
            {
                case "=q":
                    parsed.Promotion = PieceType.Queen;
                    break;

                case "=r":
                    parsed.Promotion = PieceType.Rook;
                    break;

                case "=b":
                    parsed.Promotion = PieceType.Bishop;
                    break;

                case "=n":
                    parsed.Promotion = PieceType.Knight;
                    break;
            }

            return parsed;
        }
    }
}