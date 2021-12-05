using System;

namespace bead2.Persistence
{
    public class GameObject
    {
        protected Tuple<int, int> mPosition;

        public Tuple<int, int> Position
        {
            get => mPosition;
            set => mPosition = value;
        }
    }
}