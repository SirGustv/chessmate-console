using System;

namespace GameBoard.Excepitions
{
    public class BoardException : Exception
    {
        public BoardException(string msg) : base(msg)
        {
        }
    }
}