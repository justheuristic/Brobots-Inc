using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using game.Logic;
using game.Objects;


namespace game.IO
{
    public class ScreenManager: Manager
    {

        public ScreenManager(MainGame game):base(game){}
        SpriteBatch batch;
		
        SortedDictionary<int, HashSet<GameObject>> objects = new SortedDictionary<int, HashSet<GameObject>>();
        public void RegisterObject(GameObject obj)
        {
            if (!objects.ContainsKey(obj.drawPriority))
                objects[obj.drawPriority] = new HashSet<GameObject>();
            objects[obj.drawPriority].Add(obj);
        }
        public void UnregisterObject(GameObject obj)
        {
            objects[obj.drawPriority].Remove(obj);
            //not trying to remove empty sets as this is saving us time for their recreation for the cost of a few memory.
        }

        public void LoadTextures(ContentManager Content, SpriteBatch _spritebatch, GraphicsDevice device)
        {
            batch = _spritebatch;
            foreach (var kvp in objects)
            {
                foreach (GameObject obj in kvp.Value)
                {
                    obj.InitTexture(Content,device);
                    
                }
            }


        }
        public override void Loop(float dtime)
        {
            HashSet<GameObject> destroyed = new HashSet<GameObject>();
            foreach (var kvp in objects)
            {
                foreach (GameObject obj in kvp.Value)
                {
                    obj.Draw(batch);
                    if (obj.destroyed)
                        destroyed.Add(obj);
                        
                }
            }
            foreach( GameObject obj in destroyed)
            {
                UnregisterObject(obj);
            }

        }
    }
}
