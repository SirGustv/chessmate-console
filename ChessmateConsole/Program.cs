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
            try
            {
                ChessGame game = new ChessGame();

                while (!game.Ending)
                {
                    Console.Clear();
                    Screen.PrintBoard(game.Board);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Position origin = Screen.ReadPositionChess().ToPosition();

                    bool[,] possiblePositions = game.Board.Piece(origin).PossibleMoves();

                    Console.Clear();
                    Screen.PrintBoard(game.Board, possiblePositions);

                    Console.WriteLine();
                    Console.Write("Destino: ");
                    Position destiny = Screen.ReadPositionChess().ToPosition();

                    game.ExeMove(origin, destiny);
                }
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
