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
        /// terminal objects cannot have children, so they are handled via simplified scheme
        /// </summary>
        public bool terminal = false;
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
            {//target is set in global coords, so we transform there.
                Vector2 v = new Vector2(x,y);
                v =toGlobal(v);
                x = v.X;
                y = v.Y;
                a = toGlobal(a);

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

        #region matrices
        bool localUpdated = false;
        Matrix localTransform = Matrix.Identity;
        bool globalUpdated = false;
        Matrix globalTransform = Matrix.Identity;
        public virtual Matrix getLocalTransform()
        {//coordinate transition from child's local to my local;
            if (terminal) throw new Exception("Terminal objects do not calculate this");
            if (localUpdated) return localTransform;
            localUpdated = true;
            Matrix local = new Matrix();
            local.M11 = (float)Math.Cos(angle); //x' <- x
            local.M12 = (float)Math.Sin(angle);//x' <- y
            local.M13 = Position.X; //x' <- C

            local.M21 = -(float)Math.Sin(angle); //y' <- x
            local.M22 = (float)Math.Cos(angle);//y' <- y
            local.M23 = Position.Y; //y' <- C

            local.M33 = 1; // C <- C
            localTransform = local;
            return local;
        }

        public virtual Matrix getGlobalTransform()
        {
            if (terminal) throw new Exception("Terminal objects do not calculate this");
            if (globalUpdated) return globalTransform;
            globalUpdated = true;
            Matrix local = getLocalTransform();
            if (parent == null)
                globalTransform = local;
            else
                globalTransform = parent.getGlobalTransform() * local;//this one UPDATES the field to optimize recalculations. DO NOT TRY TO OPTIMIZE... or understand

            return globalTransform;
        }
        public static Vector2 getTransformed (Vector2 v2, Matrix transformation)
        {
            return new Vector2(transformation.M11 * v2.X + transformation.M12 * v2.Y + transformation.M13,
                               transformation.M21 * v2.X + transformation.M22 * v2.Y + transformation.M23);
        }
        public Vector2 toGlobal (Vector2 v2)
        {
            if (parent == null) return v2;
            return getTransformed(v2, parent.getGlobalTransform());            
        }
        public float toGlobal( float localAngle )
        {//child's angle to global angle
            if (parent == null) return localAngle;
            return parent.angle + parent.toGlobal(localAngle);
        }
        #endregion 
        
        public override void Update(MainGame game,float dtime)
        {
            localUpdated = false;
            globalUpdated = false;
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
        public virtual Rectangle BoundsGlobal
        {
            get
            {
                Vector2 middle = toGlobal(Position);
                return new Rectangle((int)middle.X, (int)middle.Y, size.Width, size.Height);

            }
        }
        public virtual Rectangle BoundsLocal
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
                BoundsGlobal,
                TexRectangle,
                Color.White,
                toGlobal(angle),
                textureCenter,
                effects,
                0
                );
            
        }



    }
    
}
