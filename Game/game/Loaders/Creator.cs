using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using game.Objects;
using game.Logic;

namespace game.Loaders
{
    public static class Creator
    {
        
        public static void addGameObject(this MainGame game, GameObject obj)
        {
            game.objectManager.objects.Add(obj);
            game.screenManager.RegisterObject(obj);
        }
        public static void removeGameObject(this MainGame game,GameObject obj)
        {
            game.objectManager.objects.Remove(obj);
            game.screenManager.UnregisterObject(obj);
        }
        public static void addAgent( this MainGame game,Agent agent )
        {
            game.agentManager.agents.Add(agent);
        }
        public static void removeAgent(this MainGame game, Agent agent)
        {
            game.agentManager.agents.Remove(agent);
        }
        
    }
}
