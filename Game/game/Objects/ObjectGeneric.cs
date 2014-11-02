using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using game.Logic;

namespace game.Objects
{
    public abstract class ObjectGeneric
    {
        public virtual void Update(MainGame game,float dtime) 
        {
            HashSet<Task> finished = new HashSet<Task>();
            foreach (Task task in tasks)
            {
                task.process(game,dtime);
                if (task.finished) finished.Add(task);

            }
            foreach (Task task in finished)
                tasks.Remove(task);

        }
        public virtual void destruct()
        {

            destroyed = true;
        }


        /// <summary>
        /// if an agent is destroyed, it is removed from all the managers, but might still be visible until something (p.e. an agent) wants it.
        /// </summary>
        public bool destroyed = false;
        public HashSet<Task> tasks = new HashSet<Task>();

    }
    public abstract class Task
    {
        public Task(ObjectGeneric obj)
        {
            subject = obj;
        }
        public ObjectGeneric subject;
        public bool bound = true;
        public virtual void process(MainGame game,float dtime)
        {
            if (bound)
                if (subject.destroyed)
                    finished = true;
        }
        public bool finished = false;
    }

    public class ObjectManager:Manager
    {
        public ObjectManager(MainGame game):base(game){}
        public HashSet<ObjectGeneric> objects = new HashSet<ObjectGeneric>();
        public override void Loop(float dtime)
        {
            HashSet<ObjectGeneric> destroyed = new HashSet<ObjectGeneric>();
            foreach (ObjectGeneric obj in new HashSet<ObjectGeneric>( objects))
            {
                obj.Update(game,dtime);

                if (obj.destroyed)
                    destroyed.Add(obj);
            }
            foreach( ObjectGeneric obj in destroyed)
            {
                objects.Remove(obj);
                obj.destruct();
            }

        }

    }

}
