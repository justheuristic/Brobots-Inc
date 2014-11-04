using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using game.Logic;

namespace game.IO
{
    public class InputManager: Manager
    {

        public InputManager(MainGame game):base(game){}

        
        void ProcessKeyboardInput()
        {
            throw new NotImplementedException();
        }

        MouseState mState;
        MouseState lastMS;
        void ProcessMouseInput()
        {
#warning not implemented mouse buttons but for left click;
            mState = Mouse.GetState();
            MouseListener.MouseEventMask mask = MouseListener.MouseEventMask.LeftClickMask;
            if (mState.LeftButton == ButtonState.Pressed)
            {
                if (lastMS.LeftButton == ButtonState.Released)
                    sendMouseNotes(mask);
            }
            lastMS = mState;
        }
        
        
        void sendMouseNotes (MouseListener.MouseEventMask mask)
        {
            if (listeners.ContainsKey(mask))
               listeners[mask].notify(new MouseListener.MouseMessage(mState, listeners[mask]));
        }

        void ProcessTouchInput()
        {
            throw new NotImplementedException();
        }

        public override void Loop(float dtime)
        {
            ProcessMouseInput();
        }

        Dictionary<MouseListener.MouseEventMask, MouseListener> listeners = new Dictionary<MouseListener.MouseEventMask, MouseListener>();
        public void RegisterInput(MouseListener.MouseEventMask mask, Agent Agent)
        {
            if (!listeners.ContainsKey(mask))
            {
                listeners[mask] = new MouseListener(mask);
                game.agentManager.agents.Add(listeners[mask]);
            }

            listeners[mask].recipients.Add(Agent);
        }
        public void UnregisterInput(MouseListener.MouseEventMask mask, Agent agent)
        {
            listeners[mask].recipients.Remove(agent);
        }
    }
    public class MouseListener:Listener
    {
        public enum MouseButton { left, right,center, any}
        public enum MouseEvent { pressed, released, isDown, isUp, any}
        public class MouseMessage: Message
        {
            public MouseState mState;
            public MouseMessage(MouseState state, Agent origin)
            {
                header = "mouseEvent";
                sender = origin;
                mState = state;

            }
        }
        public struct MouseEventMask
        {
            public MouseButton allowedBtn;
            public MouseEvent allowedEvent;
            public Rectangle allowedArea;
            public static MouseEventMask LeftClickMask = new MouseEventMask()
            {
                allowedArea = new Rectangle(0, 0, int.MaxValue, int.MaxValue),
                allowedEvent = MouseEvent.pressed,
                allowedBtn = MouseButton.left
            };
        }
        
        
        public MouseListener(MouseEventMask evtmask)
        {
        }
        public MouseButton mouseButton;
        public MouseEvent mouseEvent;
        
    }

}
