using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace game.Objects
{
    public abstract class SpriteRotation
    {
        public abstract int getNumSprites();
        public abstract void InitTexture(ContentManager Content, GraphicsDevice Device);
        public abstract void GetSprite(int i, out Texture2D tex, out Rectangle rec);
        
    }
    class EffectObject: GameObject
    {
        SpriteRotation spriteRotation;
        float timeElapsed = 0;
        float timeLength;
        public EffectObject(SpriteRotation sprites, float timelength, Vector2 position):base()
        {
            timeLength= timelength;
            Position = position;
            spriteRotation = sprites;
            terminal = true;
        }
        public override void InitTexture(ContentManager Content, GraphicsDevice Device)
        {

            spriteRotation.InitTexture(Content, Device);
            
            base.InitTexture(Content, Device);
            spriteRotation.GetSprite(0, out Texture, out TexRectangle);
            
            size = new Rectangle(0, 0, 72, 72);
            textureCenter = new Vector2(36,36);

        }
        public override void Update(MainGame game, float dtime)
        {
            timeElapsed += dtime/timeLength;
            int frameNumber = Convert.ToInt32(timeElapsed * spriteRotation.getNumSprites() / timeLength);
            if (frameNumber >= spriteRotation.getNumSprites())
            {
                frameNumber = spriteRotation.getNumSprites()-1;
                destroyed = true;
            }
            spriteRotation.GetSprite(frameNumber, out Texture, out TexRectangle);
            base.Update(game, dtime);
        }
     
    }
}
