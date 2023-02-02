using System;
using GameBoard;
namespace Chessmate
{
    public class ChessGame
    {
        public Board Board { get; private set; }
        private int Turn;
        private Color CurrentPlayer;
        public bool Ending { get; private set; }

        public ChessGame()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Ending = false;
            AddPiece();
        }

        public void ExeMove(Position origin, Position destiny) //Executar movimento
        {
            Piece p = Board.RemovePiece(origin);
            p.IncreaseNumMoves();
            Piece catchPiece = Board.RemovePiece(destiny);
            Board.AddPiece(p, destiny);
        }

        private void AddPiece()
        {
            Board.AddPiece(new Tower(Color.White, Board), new ChessPosition('c', 1).ToPosition());
            Board.AddPiece(new Tower(Color.White, Board), new ChessPosition('c', 2).ToPosition());
            Board.AddPiece(new Tower(Color.White, Board), new ChessPosition('d', 2).ToPosition());
            Board.AddPiece(new Tower(Color.White, Board), new ChessPosition('e', 2).ToPosition());
            Board.AddPiece(new Tower(Color.White, Board), new ChessPosition('e', 1).ToPosition());
            Board.AddPiece(new King(Color.White, Board), new ChessPosition('d', 1).ToPosition());

            Board.AddPiece(new Tower(Color.Black, Board), new ChessPosition('c', 7).ToPosition());
            Board.AddPiece(new Tower(Color.Black, Board), new ChessPosition('c', 8).ToPosition());
            Board.AddPiece(new Tower(Color.Black, Board), new ChessPosition('d', 7).ToPosition());
            Board.AddPiece(new Tower(Color.Black, Board), new ChessPosition('e', 7).ToPosition());
            Board.AddPiece(new Tower(Color.Black, Board), new ChessPosition('e', 8).ToPosition());
            Board.AddPiece(new King(Color.Black, Board), new ChessPosition('d', 8).ToPosition());
        }
    }
}