using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using game.Logic;

namespace game.IO
{
    public class SoundManager: Manager
    {
        public SoundManager(MainGame game):base(game){}
        public static SoundEffect BallWallCollisionSoundEffect;
        public static SoundEffect PaddleBallCollisionSoundEffect;

        public static void LoadSounds(ContentManager Content)
        {
            BallWallCollisionSoundEffect = Content.Load<SoundEffect>("BallWallCollision");
            PaddleBallCollisionSoundEffect = Content.Load<SoundEffect>("PaddleBallCollision");
        }

        public override void Loop(float dtime)
        {
            ;
        }
    }

    
}
