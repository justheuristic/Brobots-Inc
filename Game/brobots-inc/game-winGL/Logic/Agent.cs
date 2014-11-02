using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using game.Objects;

namespace game.Logic
{
    public abstract class Agent
    {
        public abstract class Message
        {
            public string header;
            public Agent sender;
        }
        public Queue<Message> messages = new Queue<Message>();
        public virtual void notify(Message msg)
        {
            messages.Enqueue(msg);
        }
        public virtual void react() 
        {
            if (parent != null && parent.dead) dead = true;
        }
        public bool dead = false;
        public Agent parent;
    }
    public abstract class MaterialAgent : Agent
    {
        public GameObject source;
        public override void react()
        {

            if (source.destroyed)
                dead = true;
            base.react();
        }
    }
    public class AgentManager: Manager
    {
        public AgentManager(MainGame game):base(game){}
        public HashSet<Agent> agents = new HashSet<Agent>();
        public override void Loop(float dtime)
        {
            HashSet<Agent> dead = new HashSet<Agent>();
            foreach (Agent agent in agents)
            {
                agent.react();
                if (agent.dead) dead.Add(agent);
            }
            foreach (Agent agent in dead)
                agents.Remove(agent);
            
        }

    }
}
