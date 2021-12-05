using System;

namespace bead2.Persistence
{
    public class GameTree : GameObject
    {
        public GameTree(int m, int n)
        {
            mPosition = new Tuple<int, int>(m, n);
        }
    }
}