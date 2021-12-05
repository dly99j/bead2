using System;

namespace bead2.Persistence
{
    public class GameFood : GameObject
    {
        public GameFood(int m, int n)
        {
            mPosition = new Tuple<int, int>(m, n);
        }
    }
}