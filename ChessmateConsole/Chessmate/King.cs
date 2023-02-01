using GameBoard;

namespace Chessmate
{
    public class King : Piece
    {
        public King(Color color, Board board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "R";
        }
    }
}