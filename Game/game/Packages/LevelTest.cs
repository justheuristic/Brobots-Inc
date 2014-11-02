using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;

using game.Objects;
using game.Packages;
using game.Loaders;


namespace game.Packages
{

 
    class LevelTest
    {
        public static void load (MainGame game)
        {
            Background.backgroundBuilder.addBackground(game);
            RobotTest.robotBuilder.addRobot(game, new Vector2(100, 100), 0, RobotTest.RobotTest.RobotParams.defaultParams);
            RobotTest.robotBuilder.addRobot(game, new Vector2(300, 300), 0, RobotTest.RobotTest.RobotParams.defaultParams);


        }
    }
}
