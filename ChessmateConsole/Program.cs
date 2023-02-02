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

            ChessPosition pos = new ChessPosition('a', 8);

            Console.WriteLine(pos);
            Console.WriteLine(pos.ToPosition());

            Console.ReadKey();
        }
    }
}
