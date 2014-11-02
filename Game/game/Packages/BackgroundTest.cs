
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using game.Objects;
using game.Loaders;

namespace game.Packages.Background
{
    class backgroundBuilder
    {
        public static void addBackground(MainGame game, string tex = "BG")
        {
            backgroundObject obj = new backgroundObject(game.ScreenWidth,game.ScreenHeight,tex);
            game.addGameObject(obj);

            
        }
    }
    class backgroundObject: GameObject
    {
        

        public backgroundObject(int targWidth = int.MinValue, int targHeight = int.MinValue, string texName = "BG")
            : base()
        {
            targetHeight = targHeight;
            targetWidth = targWidth;
            drawPriority = int.MinValue;
            textureName = texName;
        }
        string textureName;
        int targetWidth, targetHeight;
        public override void InitTexture(ContentManager Content, GraphicsDevice device)
        {
            Texture = Content.Load<Texture2D>(textureName);
            base.InitTexture(Content,device);
            if (targetWidth != int.MinValue)
                size.Width = targetWidth;
            if (targetHeight != int.MinValue)
                size.Height = targetHeight;
            Position = new Vector2(size.Width/2, size.Height/2);
            
            
        }
    }
}
