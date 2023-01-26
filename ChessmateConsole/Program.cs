using System;
using Board;

namespace ChessmateConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Position P = new Position(3, 4);

            Console.WriteLine(P);
            Console.ReadKey();
        }
    }
}
