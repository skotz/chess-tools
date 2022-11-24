using Skotz.Chess.Tools.Game;

namespace Skotz.Chess.Tools.Extension
{
    public static class SquareExtensions
    {
        public static Square Transpose(this Square square, int fileDelta, int rankDelta)
        {
            if (square != Square.None)
            {
                var text = square.ToString();
                var file = text[0] - 'A' + fileDelta;
                var rank = text[1] - '1' + rankDelta;

                text = ((char)(file + 'A')).ToString() + ((char)(rank + '1')).ToString();

                if (Enum.TryParse(text, out Square destination))
                {
                    return destination;
                }
            }

            return Square.None;
        }

        public static int Transpose(this int square, int fileDelta, int rankDelta)
        {
            return (int)((Square)square).Transpose(fileDelta, rankDelta);
        }

        public static bool IsRank(this int square, int rank)
        {
            // TODO: lol, too slow
            return ((Square)square).ToString()[1].ToString() == rank.ToString();
        }
    }
}