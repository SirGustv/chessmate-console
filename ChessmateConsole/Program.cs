using System;
using GameBoard;
using Chessmate;

namespace ChessmateConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(8, 8);

            board.AddPiece(new Tower(Color.Black, board), new Position(0, 0));
            board.AddPiece(new King(Color.Black, board), new Position(2, 4));

            Screen.PrintBoard(board);

            Console.ReadKey();
        }
    }
}
