namespace Skotz.Chess.Tools.Game
{
    public class Move
    {
        public Square Source { get; set; }

        public Square Destination { get; set; }

        public PieceType Promotion { get; set; }

        public Move()
        {
            Source = Square.None;
            Destination = Square.None;
            Promotion = PieceType.None;
        }

        public Move(Square source, Square destination)
        {
            Source = source;
            Destination = destination;
            Promotion = PieceType.None;
        }

        public Move(int source, int destination)
        {
            Source = (Square)source;
            Destination = (Square)destination;
            Promotion = PieceType.None;
        }

        public Move(Square source, Square destination, PieceType promotion)
        {
            Source = source;
            Destination = destination;
            Promotion = promotion;
        }

        public Move(int source, int destination, PieceType promotion)
        {
            Source = (Square)source;
            Destination = (Square)destination;
            Promotion = promotion;
        }

        public override string ToString()
        {
            return $"{Source}-{Destination}{(Promotion != PieceType.None ? "-" + Promotion : "")}";
        }
    }
}