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


        public void IncreaseNumMoves()
        {
            NmbrOfMoves++;
        }

        public bool ExistPossibleMoves()
        {
            bool[,] mat = PossibleMoves();
            for (int i = 0; i < Board.Lines; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    if (mat[i, j] == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanMoveFor(Position pos)
        {
            return PossibleMoves()[pos.Line, pos.Column];
        }

        public abstract bool[,] PossibleMoves();
    }
}