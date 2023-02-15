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
                    try
                    {
                        Console.Clear();
                        Screen.PrintBoard(game.Board);
                        Console.WriteLine();
                        Console.WriteLine("Turno: " + game.Turn);
                        Console.WriteLine("Aguardando jogada: " + game.CurrentPlayer);

                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Position origin = Screen.ReadPositionChess().ToPosition();
                        game.ValidOriginPosition(origin);

                        bool[,] possiblePositions = game.Board.Piece(origin).PossibleMoves();

                        Console.Clear();
                        Screen.PrintBoard(game.Board, possiblePositions);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Position destiny = Screen.ReadPositionChess().ToPosition();
                        game.ValidDestinyPosition(origin, destiny);

                        game.RealizeMove(origin, destiny);
                    }
                    catch (BoardException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
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
