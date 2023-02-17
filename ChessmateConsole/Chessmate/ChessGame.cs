using System;
using System.Collections.Generic;
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
        private HashSet<Piece> Pieces;
        private HashSet<Piece> Catches;
        public bool Check { get; private set; }

        public ChessGame()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Ending = false;
            Check = false;
            Pieces = new HashSet<Piece>();
            Catches = new HashSet<Piece>();
            AddPiece();
        }

        public Piece ExeMove(Position origin, Position destiny) //Executar movimento
        {
            Piece p = Board.RemovePiece(origin);
            p.IncrementNumMoves();
            Piece catchPiece = Board.RemovePiece(destiny);
            Board.AddPiece(p, destiny);
            if (catchPiece != null)
            {
                Catches.Add(catchPiece);
            }
            return catchPiece;
        }

        public void UndoMove(Position origin, Position destiny, Piece catchPiece)
        {
            Piece p = Board.RemovePiece(destiny);
            p.DecrementNumMoves();
            if (catchPiece != null)
            {
                Board.AddPiece(catchPiece, destiny);
                Catches.Remove(catchPiece);
            }
            Board.AddPiece(p, origin);
        }

        public void RealizeMove(Position origin, Position destiny)
        {
            Piece catchPiece = ExeMove(origin, destiny);

            if (InCheck(CurrentPlayer))
            {
                UndoMove(origin, destiny, catchPiece);
                throw new BoardException("Você não pode se colocar em xeque!");
            }

            if (InCheck(Opponent(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }

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

        public HashSet<Piece> CatchesPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece i in Catches)
            {
                if (i.Color == color)
                {
                    aux.Add(i);
                }
            }
            return aux;
        }
        public HashSet<Piece> PiecesInGame(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece i in Pieces)
            {
                if (i.Color == color)
                {
                    aux.Add(i);
                }
            }
            aux.ExceptWith(CatchesPieces(color));
            return aux;
        }

        private Color Opponent(Color color)
        {
            if (color == Color.White)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        private Piece King(Color color)
        {
            foreach (Piece x in PiecesInGame(color))
            {
                if (x is King)
                {
                    return x;
                }
            }
            return null;
        }

        public bool InCheck(Color color)
        {
            Piece R = King(color);
            if (R == null)
            {
                throw new BoardException("Não há rei da cor " + color + " no tabuleiro!");
            }
            foreach (Piece x in PiecesInGame(Opponent(color)))
            {
                bool[,] mat = x.PossibleMoves();
                if (mat[R.Position.Line, R.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }

        public void AddNewPiece(char column, int line, Piece piece)
        {
            Board.AddPiece(piece, new ChessPosition(column, line).ToPosition());
            Pieces.Add(piece);
        }

        private void AddPiece()
        {
            AddNewPiece('c', 1, new Tower(Color.White, Board));
            AddNewPiece('c', 2, new Tower(Color.White, Board));
            AddNewPiece('d', 2, new Tower(Color.White, Board));
            AddNewPiece('e', 2, new Tower(Color.White, Board));
            AddNewPiece('e', 1, new Tower(Color.White, Board));
            AddNewPiece('d', 1, new King(Color.White, Board));

            AddNewPiece('c', 7, new Tower(Color.Black, Board));
            AddNewPiece('c', 8, new Tower(Color.Black, Board));
            AddNewPiece('d', 7, new Tower(Color.Black, Board));
            AddNewPiece('e', 7, new Tower(Color.Black, Board));
            AddNewPiece('e', 8, new Tower(Color.Black, Board));
            AddNewPiece('d', 8, new King(Color.Black, Board));
        }
    }
}