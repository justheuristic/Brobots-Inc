using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game.Logic
{
    public abstract class Manager
    {
        public MainGame game;
        public Manager(MainGame maingame)
        {
            game = maingame;
        }
        public abstract void Loop(float dtime);
    }
}
