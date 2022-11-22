namespace Skotz.Chess.Tools.Game
{
    public class Move
    {
        public Square Source { get; set; }

        public Square Destination { get; set; }

        public Move()
        {
            Source = Square.None;
            Destination = Square.None;
        }

        public Move(Square source, Square destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}