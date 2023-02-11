namespace GameBoard
{
    public abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; set; }
        public Board Board { get; set; }
        public int NmbrOfMoves { get; set; }

        public Piece(Color color, Board board)
        {
            this.Position = null; //Posição inicial a ser definida pela classe 'Board'
            this.Color = color;
            this.Board = board;
            this.NmbrOfMoves = 0; //Contador de movimentos de cada peça instânciada
        }

        public abstract bool[,] PossibleMoves();

        public void IncreaseNumMoves()
        {
            NmbrOfMoves++;
        }
    }
}