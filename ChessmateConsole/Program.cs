using System;
using GameBoard;

namespace ChessmateConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(8, 8);

            Screen.PrintBoard(board);

            Console.ReadKey();
        }
    }
}
