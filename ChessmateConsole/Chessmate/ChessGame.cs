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
        public Piece VunerableEnPassant { get; private set; }

        public ChessGame()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Ending = false;
            Check = false;
            VunerableEnPassant = null;
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

            //#JogadaEspecial Roque Pequeno
            if (p is King && destiny.Column == origin.Column + 2)
            {
                Position originTwr = new Position(origin.Line, origin.Column + 3);
                Position destinyTwr = new Position(origin.Line, origin.Column + 1);
                Piece Twr = Board.RemovePiece(originTwr);
                Twr.IncrementNumMoves();
                Board.AddPiece(Twr, destinyTwr);
            }

            //#JogadaEspecial Roque Grande
            if (p is King && destiny.Column == origin.Column - 2)
            {
                Position originTwr = new Position(origin.Line, origin.Column - 4);
                Position destinyTwr = new Position(origin.Line, origin.Column - 1);
                Piece Twr = Board.RemovePiece(originTwr);
                Twr.IncrementNumMoves();
                Board.AddPiece(Twr, destinyTwr);
            }

            //JogadaEspecia En Passant
            if (p is Pawn)
            {
                if (origin.Column != destiny.Column && catchPiece == null)
                {
                    Position posPwn;
                    if (p.Color == Color.White)
                    {
                        posPwn = new Position(3, destiny.Column);
                    }
                    else
                    {
                        posPwn = new Position(4, destiny.Column);
                    }
                    catchPiece = Board.RemovePiece(posPwn);
                    Catches.Add(catchPiece);
                }
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

            //#JogadaEspecial Roque Pequeno
            if (p is King && destiny.Column == origin.Column + 2)
            {
                Position originTwr = new Position(origin.Line, origin.Column + 3);
                Position destinyTwr = new Position(origin.Line, origin.Column + 1);
                Piece Twr = Board.RemovePiece(destinyTwr);
                Twr.DecrementNumMoves();
                Board.AddPiece(Twr, originTwr);
            }

            //#JogadaEspecial Roque Grande
            if (p is King && destiny.Column == origin.Column - 2)
            {
                Position originTwr = new Position(origin.Line, origin.Column - 4);
                Position destinyTwr = new Position(origin.Line, origin.Column - 1);
                Piece Twr = Board.RemovePiece(destinyTwr);
                Twr.DecrementNumMoves();
                Board.AddPiece(Twr, originTwr);
            }

            //#JogadaEspecial En Passant
            if (p is Pawn)
            {
                if (origin.Column != destiny.Column && catchPiece == VunerableEnPassant)
                {
                    Piece pawn = Board.RemovePiece(destiny);
                    Position posPwn;
                    if (p.Color == Color.White)
                    {
                        posPwn = new Position(3, destiny.Column);
                    }
                    else
                    {
                        posPwn = new Position(4, destiny.Column);
                    }
                    Board.AddPiece(pawn, posPwn);
                }
            }
        }

        public void RealizeMove(Position origin, Position destiny)
        {
            Piece catchPiece = ExeMove(origin, destiny);

            if (InCheck(CurrentPlayer))
            {
                UndoMove(origin, destiny, catchPiece);
                throw new BoardException("Você não pode se colocar em xeque!");
            }

            Piece p = Board.Piece(destiny);

            //#JogadaEspecial Promoção
            if (p is Pawn)
            {
                if ((p.Color == Color.White && destiny.Line == 0) || (p.Color == Color.Black && destiny.Line == 7))
                {
                    p = Board.RemovePiece(destiny);
                    Pieces.Remove(p);
                    Piece queen = new Queen(p.Color, Board);
                    Board.AddPiece(queen, destiny);
                    Pieces.Add(queen);
                }
            }

            if (InCheck(Opponent(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            if (CheckmateTest(Opponent(CurrentPlayer)))
            {
                Ending = true;
            }
            else
            {
                Turn++;
                ChangePlayer();
            }

            //#JogadaEspecial En Passant
            if (p is Pawn && (destiny.Line == origin.Line - 2 || destiny.Line == origin.Line + 2))
            {
                VunerableEnPassant = p;
            }
            else
            {
                VunerableEnPassant = null;
            }
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

        public bool CheckmateTest(Color color)
        {
            if (!InCheck(color))
            {
                return false;
            }
            foreach (Piece x in PiecesInGame(color))
            {
                bool[,] mat = x.PossibleMoves();
                for (int i = 0; i < Board.Lines; i++)
                {
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position origin = x.Position;
                            Position destiny = new Position(i, j);
                            Piece catchPiece = ExeMove(origin, destiny);
                            bool checkTest = InCheck(color);
                            UndoMove(origin, destiny, catchPiece);
                            if (!checkTest)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void AddNewPiece(char column, int line, Piece piece)
        {
            Board.AddPiece(piece, new ChessPosition(column, line).ToPosition());
            Pieces.Add(piece);
        }

        private void AddPiece()
        {
            AddNewPiece('a', 1, new Tower(Color.White, Board));
            AddNewPiece('b', 1, new Horse(Color.White, Board));
            AddNewPiece('c', 1, new Bishop(Color.White, Board));
            AddNewPiece('d', 1, new Queen(Color.White, Board));
            AddNewPiece('e', 1, new King(Color.White, Board, this));
            AddNewPiece('f', 1, new Bishop(Color.White, Board));
            AddNewPiece('g', 1, new Horse(Color.White, Board));
            AddNewPiece('h', 1, new Tower(Color.White, Board));
            AddNewPiece('a', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('b', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('c', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('d', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('e', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('f', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('g', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('h', 2, new Pawn(Color.White, Board, this));

            AddNewPiece('b', 8, new Horse(Color.Black, Board));
            AddNewPiece('a', 8, new Tower(Color.Black, Board));
            AddNewPiece('c', 8, new Bishop(Color.Black, Board));
            AddNewPiece('d', 8, new Queen(Color.Black, Board));
            AddNewPiece('e', 8, new King(Color.Black, Board, this));
            AddNewPiece('f', 8, new Bishop(Color.Black, Board));
            AddNewPiece('g', 8, new Horse(Color.Black, Board));
            AddNewPiece('h', 8, new Tower(Color.Black, Board));
            AddNewPiece('a', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('b', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('c', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('d', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('e', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('f', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('g', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('h', 7, new Pawn(Color.Black, Board, this));
        }
    }
}