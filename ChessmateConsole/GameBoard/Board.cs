using GameBoard.Excepitions;

namespace GameBoard
{
    public class Board
    {
        //Propriedades
        public int Lines { get; set; }
        public int Columns { get; set; }
        private Piece[,] Pieces;

        //Construtor
        public Board(int lines, int columns)
        {
            this.Lines = lines;
            this.Columns = columns;
            Pieces = new Piece[Lines, Columns];
        }

        //Métodos
        public Piece Piece(int line, int column)
        {
            return Pieces[line, column];
        }

        public Piece Piece(Position pos) //Sobrecarga que recebe peça pela posição
        {
            return Pieces[pos.Line, pos.Column];
        }

        public bool ExistsPiece(Position pos)
        {
            ValidatePosition(pos);
            return Piece(pos) != null;
        }

        public void AddPiece(Piece p, Position pos) //Adiciona peça no tabuleiro
        {
            if (ExistsPiece(pos))
            {
                throw new BoardException("Já existe uma peça nesta posição!");
            }
            Pieces[pos.Line, pos.Column] = p;
            p.Position = pos;
        }

        public Piece RemovePiece(Position pos) //Remove peça do tabuleiro
        {
            if (Piece(pos) == null)
            {
                return null;
            }
            Piece aux = Piece(pos);
            aux.Position = null;
            Pieces[pos.Line, pos.Column] = null;
            return aux;
        }

        public bool ValidPosition(Position pos) //Posição válida
        {
            if (pos.Line < 0 || pos.Line >= Lines || pos.Column < 0 || pos.Column >= Columns)
            {
                return false;
            }
            return true;
        }

        public void ValidatePosition(Position pos) //Validar posição
        {
            if (!ValidPosition(pos))
            {
                throw new BoardException("Posição Inválida!");
            }
        }
    }
}