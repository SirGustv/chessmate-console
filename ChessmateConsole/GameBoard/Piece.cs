namespace GameBoard
{
    public class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; set; }
        public Board Board { get; set; }
        public int NmbrOfMoves { get; set; }

        public Piece(Position position, Color color, Board board)
        {
            this.Position = position;
            this.Color = color;
            this.Board = board;
            this.NmbrOfMoves = 0;
        }
    }
}