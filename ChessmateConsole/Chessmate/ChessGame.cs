using System;
using GameBoard;
using GameBoard.Excepitions;

namespace Chessmate
{
    public class ChessGame
    {
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public Color CurrentPlayer { get; private set; }
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

        public void RealizeMove(Position origin, Position destiny)
        {
            ExeMove(origin, destiny);
            Turn++;
            ChangePlayer();
        }

        public void ValidOriginPosition(Position pos)
        {
            if (Board.Piece(pos) == null)
            {
                throw new BoardException("Não existe peça na posição de origem escolhida!");
            }
            if (CurrentPlayer != Board.Piece(pos).Color)
            {
                throw new BoardException("A peça de origem escolhida não é sua!");
            }
            if (!Board.Piece(pos).ExistPossibleMoves())
            {
                throw new BoardException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidDestinyPosition(Position origin, Position destiny)
        {
            if (!Board.Piece(origin).CanMoveFor(destiny))
            {
                throw new BoardException("Posição de destino inválida!");
            }
        }

        private void ChangePlayer()
        {
            if (CurrentPlayer == Color.White)
            {
                CurrentPlayer = Color.Black;
            }
            else
            {
                CurrentPlayer = Color.White;
            }
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