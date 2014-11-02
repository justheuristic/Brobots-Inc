using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using game.Objects;
using game.Logic;
using game.Loaders;
using game.IO;

namespace game.Packages.RobotTest
{
    
    class robotBuilder
    {
        public static void addRobot(MainGame game, Vector2 position, float orientation, RobotTest.RobotParams param)
        {
            RobotTest obj = new RobotTest(position, orientation, param);
            RobotAgent agent = new RobotAgent(obj);
            game.addMaterialAgent(agent);
            game.inputManager.RegisterInput(MouseListener.MouseEventMask.LeftClickMask, agent);

        }
    }
    class RobotTest : GameObject
    {
        //logic params
        public struct RobotParams
        {
            public int width;
            public int height;
            public float moveAcc;
            public float turnSpeed;
            public float inertion;
            public string textureName;
            public float circleFrequency;
            public static RobotParams defaultParams = new RobotParams 
            { 
                height = 100,
                width = 60,
                moveAcc = 120,
                circleFrequency = 0.03f,
                turnSpeed = 1.2f,
                inertion = 0.4f,
                textureName= "rob1" 
            };
        }
        public RobotParams robotParams;
        public Vector2 velocity;

        public void accelerateForward(float acceleration)
        {
            velocity += new Vector2(acceleration * (float)Math.Cos(angle), acceleration * (float)Math.Sin(angle));
        }
        public RobotTest(Vector2 pos,float angle, RobotParams rparams)
            : base()
        {
            Position = pos;
            robotParams = rparams;
            velocity = new Vector2();
        }

        public override void InitTexture(ContentManager Content, GraphicsDevice Device)
        {

            Texture = Content.Load<Texture2D>(robotParams.textureName);
            TexRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            base.InitTexture(Content, Device);
            size = new Rectangle(0, 0, robotParams.height, robotParams.width);
            
        }

        float timeSinceLastCircle = 0;
        public override void Update(MainGame game,float dtime)
        {
            Position += velocity * dtime;
            velocity *= (float)Math.Pow(robotParams.inertion, dtime);
            timeSinceLastCircle += dtime;
            if (timeSinceLastCircle > robotParams.circleFrequency)
            {
                timeSinceLastCircle = 0;
                EffectObject eobj = new EffectObject(new trailSprite(), 0.8f, new Vector2(Position.X, Position.Y));
                eobj.InitTexture(game.Content, game.GraphicsDevice);
                eobj.drawPriority = drawPriority -1;
                
                game.screenManager.RegisterObject(eobj);
                game.objectManager.objects.Add(eobj);
                
            }
            base.Update(game, dtime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
        }
        
    }

    
    class RobotAgent:MaterialAgent
    {
        
        public RobotAgent(RobotTest robot)
        {
            source = robot;
        }
        public class RobotMotionTask: Task
        {
            Vector2 target;
            float finishDistance;
            public RobotMotionTask(Vector2 targ, RobotTest robot, float endDistance = 30): base(robot)
            {
                target = targ;
                finishDistance= endDistance;
            }
            public override void process(MainGame game,float dtime)
            {
                RobotTest robot = subject as RobotTest;
                robot.accelerateForward(robot.robotParams.moveAcc * dtime);
                float dAngle = robot.getAngleTo(target);
                if (Math.Abs( dAngle) > dtime * robot.robotParams.turnSpeed)
                    dAngle = Math.Sign(dAngle)* dtime * robot.robotParams.turnSpeed;
                robot.angle += dAngle;
                base.process(game,dtime);
                if ((target - robot.Position).LengthSquared() < finishDistance*finishDistance)
                    //why: euclidian distance is sqrt(x*x + y*y), so it is easier to compute it's square as x*x+y*y and compare with squared distance, than to compute sqrt.
                    finished = true;
            }

        }

        public override void react()
        {
            while (messages.Count != 0)
            {
                Agent.Message msg = messages.Dequeue();
                
                if (msg.GetType() ==  typeof(MouseListener.MouseMessage) )
                {
                    MouseState mState = (msg as MouseListener.MouseMessage).mState;
                    Vector2 target = new Vector2( mState.Position.X, mState.Position.Y);
                    foreach (Task tsk in source.tasks)
                        tsk.finished = true;
                    source.tasks.Add(new RobotMotionTask(target, source as RobotTest));
                }
                
            }
            base.react();
        }
    }
    public class trailSprite: SpriteRotation
    {
        Texture2D texture;
        public override int getNumSprites()
        {
            return 10;
        }
        public override void InitTexture(ContentManager Content, GraphicsDevice Device)
        {
            texture = Content.Load<Texture2D>("trail");

        }

        public override void GetSprite(int i, out Texture2D tex, out Rectangle rec)
        {
            tex = texture;
            rec = new Rectangle((i/5) *72, 72 * (i%5), 72, 72);
            
        }
    }
}
