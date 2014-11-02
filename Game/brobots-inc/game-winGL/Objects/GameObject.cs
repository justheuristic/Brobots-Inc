using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace game.Objects
{
    public abstract class GameObject: ObjectGeneric
    {
        /// <summary>
        /// logical and physical position
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// basic(not scaled) height and width at render
        /// </summary>
        public Rectangle size;
        /// <summary>
        /// Texture resource link
        /// </summary>
        public Texture2D Texture;
        /// <summary>
        /// a rectangle on the texture map where the object sprite is
        /// </summary>
        public Rectangle TexRectangle;
        /// <summary>
        /// texture rectangle coordinates (upper-left corner = 0,0) that are mapped to position vector
        /// </summary>
        public Vector2 textureCenter;
        /// <summary>
        /// angle 0 equals STRAIGHT RIGHT, turning clockwise with increase. Measured in radians.
        /// </summary>
        public float angle = 0;
        /// <summary>
        /// in which order do gameObjects draw their textures. maxvalue = highest, minvalue = last, 0 = default
        /// </summary>
        public int drawPriority = 0;
        /// <summary>
        /// relative objects are positioned depending on their parent's position and angle
        /// </summary>
        public bool relative = false;
        public GameObject parent;
        public HashSet<GameObject> children = new HashSet<GameObject>();

        public GameObject()
        {

        }
        public void moveForward(float dPos)
        {
            Position += new Vector2(dPos * (float)Math.Cos(angle), dPos * (float)Math.Sin(angle));
        }
        public float getAngleTo(Vector2 target, bool inGlobal = false)
        {
            
            float angToTarg = 0;

            float x = Position.X,
                  y = Position.Y,
                  a = angle;
            if(inGlobal)
            {
                globalTransform(ref x, ref y, ref a);
            }
            float tx = target.X - x,
                  ty = target.Y - y;    
            if (tx != 0 || ty != 0)
                angToTarg = (float)Math.Atan2(ty, tx) - a;
            if (angToTarg > Math.PI)
                angToTarg -= 2*(float)Math.PI;
            if (angToTarg <-Math.PI)
                angToTarg += 2*(float)Math.PI;
            
            
            return angToTarg;
        }


        public virtual void localTransform(ref float x, ref float y, ref float a)
        {

            //my coords from local child's coords
            x = x*(float)Math.Cos(angle) + y * (float)Math.Sin(angle) + Position.X;
            y = -x*(float)Math.Sin(angle) + y* (float)Math.Cos(angle) + Position.Y;
            a += angle;
            
        }
        public virtual void globalTransform(ref float x, ref float y, ref float a)
        {
            //global coords from child's coords
            localTransform(ref x, ref y, ref a);
            if (parent != null)
                parent.globalTransform(ref x, ref y, ref a);



        }
        public override void Update(MainGame game,float dtime)
        {
            base.Update(game,dtime);
        }

        public override void destruct()
        {
            parent = null;
 	        base.destruct();
            HashSet<GameObject> temp = children;
            children = null;

            foreach (ObjectGeneric obj in temp)
                obj.destruct();

        }
        public virtual Rectangle Bounds
        {
            get
            {
                Vector2 middle = Position;
                return new Rectangle((int)middle.X, (int)middle.Y, size.Width, size.Height);

            }
        }

        public SpriteEffects effects = SpriteEffects.None;
        
        public virtual void InitTexture(ContentManager Content, GraphicsDevice device)
        {
            if (Texture == null) return;
            TexRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            textureCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
            size = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }


        
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
                (
                Texture,
                Bounds,
                TexRectangle,
                Color.White,
                angle,
                textureCenter,
                effects,
                0
                );
        }



    }
    
}
