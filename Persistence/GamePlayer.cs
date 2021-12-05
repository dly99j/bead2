using System;

namespace bead2.Persistence
{
    public class GamePlayer : GameObject
    {
        public GamePlayer(int m, int n)
        {
            IsCaught = false;
            mPosition = new Tuple<int, int>(m, n);
        }

        public bool IsCaught { get; }
    }
}