﻿using System;
using bead2.Persistence;

namespace bead2.Model
{
    public class GameEventArgs : EventArgs
    {
        public GameEventArgs(int time, bool isWon, int foodLeft, GameTable table)
        {
            this.Time = time;
            this.IsWon = isWon;
            this.FoodLeft = foodLeft;
            this.Table = table;
        }

        public int Time { get; }

        public bool IsWon { get; }

        public int FoodLeft { get; }

        public GameTable Table { get; }
    }
}