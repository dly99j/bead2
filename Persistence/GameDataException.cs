using System;

namespace bead2.Persistence
{
    public class GameDataException : Exception
    {
        public GameDataException()
        {
        }
        public GameDataException(string message) : base(message)
        {
        }
    }
}