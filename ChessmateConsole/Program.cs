using System;
using GameBoard;
using Chessmate;
using GameBoard.Excepitions;

namespace ChessmateConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            try
            {
                Board board = new Board(8, 8);

                board.AddPiece(new Tower(Color.Black, board), new Position(0, 7));
                board.AddPiece(new Tower(Color.Black, board), new Position(0, 0));
                board.AddPiece(new King(Color.Black, board), new Position(2, 4));
                board.AddPiece(new King(Color.White, board), new Position(2, 3));


                Screen.PrintBoard(board);
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
