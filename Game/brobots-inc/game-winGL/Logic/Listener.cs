using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game.Logic
{
    /// <summary>
    /// listener is an agent that listens to some type of events and sends messages to anyone interested. Purpose: optimisation of the messaging technique.
    /// </summary>
    public abstract class Listener: Agent
    {
        public abstract class Event : Agent.Message { }
        public HashSet<Agent> recipients = new HashSet<Agent>();
        
        public override void react()
        {
            while (messages.Count != 0)
            {
                Message msg = messages.Dequeue();
                foreach (Agent agent in recipients)
                    agent.notify(msg);
            }
        }
    }

    
}
