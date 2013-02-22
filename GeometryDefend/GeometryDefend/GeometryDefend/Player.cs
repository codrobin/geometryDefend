using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GeometryDefend
{
    public class Player
    {
        public Vector2 position;
        public float rotation;
        public int radius;
        public bool playerOne = false;

        public float angleRotation;
        public float newAngleRotation;

        public float health;

        SpriteManager sprite;

        List<Bullet> bulletList;
        Texture2D bulletTexture;
        TimeSpan bulletSpawnTimer = new TimeSpan(0, 0, 0, 0, 10);//milliseconds
        TimeSpan timeElasped;
        bool createBullet = true;

        Vector2 rotateAround = new Vector2((1280 / 2), (800 / 2));

        // ------------------------------------- debugging ---------------------------------------------
        bool createWall = true;
        bool side = true;
        // ---------------------------------------------------------------------------------------------

        public Player(Texture2D texture, List<Bullet> list, Texture2D bulletTex)
        {
            position = new Vector2(1280 / 2, 800 / 2 - 80);

            //for no jump in beginning
            rotation = CalculateLookAt(position, rotateAround);
            position = Rotate(newAngleRotation, position, rotateAround);
            radius = 65;

            angleRotation = 0f;
            newAngleRotation = 0f;
            sprite = new SpriteManager(texture, texture.Width, texture.Height, texture.Width, texture.Height, position, rotation, 1);

            bulletList = list;
            bulletTexture = bulletTex;

            health = 100;
        }

        public void Update(GameTime gameTime)
        {
            //read player input
            //HandleInput(gameTime); // commented out for debug ---------------------------------------------------------------------------------------!

            //resolve the input
            rotation = CalculateLookAt(position, rotateAround);
            sprite.position = position;
            sprite.rotation = rotation;
            sprite.Update(gameTime);

            if (newAngleRotation != angleRotation)
            {
                position = Rotate(newAngleRotation, position, rotateAround);
                angleRotation = newAngleRotation;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Color.White);
        }

        /// <summary>
        /// reads input devices
        /// </summary>
        public void HandleInput(GameTime gameTime, List<Enemy> enemyList, Texture2D texture)
        {
            KeyboardState state = Keyboard.GetState();
            if (playerOne)
            {
                if (state.IsKeyDown(Keys.A))
                {
                    newAngleRotation -= .1f;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    newAngleRotation += .1f;
                }
                //bullets
                if (state.IsKeyDown(Keys.S))
                {
                    //bool createBullet = true;
                    if (createBullet == true)
                    {
                        Bullet newBullet = new Bullet(bulletTexture, position, new Vector2((float)(Math.Cos(rotation - Math.PI / 2) * 10), (float)(Math.Sin(rotation - Math.PI / 2) * 10)), rotation);
                        bulletList.Add(newBullet);
                        createBullet = false;
                        timeElasped = gameTime.TotalGameTime;
                    }
                    if (createBullet == false)
                    {
                        if (timeElasped + bulletSpawnTimer < gameTime.TotalGameTime)
                        {
                            createBullet = true;
                        }
                    }
                }
            }
            else
            {
                if (state.IsKeyDown(Keys.Z))
                {
                    newAngleRotation -= .1f;
                }
                if (state.IsKeyDown(Keys.C))
                {
                    newAngleRotation += .1f;
                }
                //bullets
                if (state.IsKeyDown(Keys.X))
                {
                    //bool createBullet = true;
                    if (createBullet == true)
                    {
                        Bullet newBullet = new Bullet(bulletTexture, position, new Vector2((float)(Math.Cos(rotation - Math.PI / 2) * 10), (float)(Math.Sin(rotation - Math.PI / 2) * 10)), rotation);
                        bulletList.Add(newBullet);
                        createBullet = false;
                        timeElasped = gameTime.TotalGameTime;
                    }
                    if (createBullet == false)
                    {
                        if (timeElasped + bulletSpawnTimer < gameTime.TotalGameTime)
                        {
                            createBullet = true;
                        }
                    }
                }
            }

            

            
            //enemies for debugging
            if (state.IsKeyDown(Keys.P))
            {
                Random random = new Random();
                Enemy newEnemy = new Enemy(texture, new Vector2(random.Next(0, 1280), random.Next(0, 800)), 0f, "basic", Color.LightSkyBlue);
                enemyList.Add(newEnemy);
            }

            if (state.IsKeyDown(Keys.O))
            {
                Random random = new Random();
                Enemy newEnemy = new Enemy(texture, new Vector2(random.Next(20, 1250), random.Next(20, 750)), 0f, "slow", Color.BlanchedAlmond);
                enemyList.Add(newEnemy);
            }

            if (state.IsKeyDown(Keys.I))
            {
                Random random = new Random();
                Enemy newEnemy = new Enemy(texture, new Vector2(random.Next(20, 1250), random.Next(20, 750)), 0f, "break", Color.Magenta);
                newEnemy.isBreak = true;
                enemyList.Add(newEnemy);
            }

            if (state.IsKeyDown(Keys.L))
            {
                if (createWall == true)
                {
                    int tempX;
                    int tempY;
                    if (side == true)
                    {
                        tempX = 1250;
                        tempY = 20;
                        side = false;
                        for (int i = 0; i < 31; i++)
                        {
                            Random random = new Random();
                            Enemy newEnemy = new Enemy(texture, new Vector2(tempX, tempY), 0f, "wall", Color.Red);
                            newEnemy.xVelocity = true;
                            //newEnemy.velocity.X = -5.0f;
                            enemyList.Add(newEnemy);
                            tempY += newEnemy.sprite.sprite.Height / 2;
                        }
                    }
                    else
                    {
                        tempX = 25;
                        tempY = 20;
                        side = true;
                        for (int i = 0; i < 50; i++)
                        {
                            Random random = new Random();
                            Enemy newEnemy = new Enemy(texture, new Vector2(tempX, tempY), 0f, "wall", Color.Red);
                            newEnemy.xVelocity = true;
                            //newEnemy.velocity.Y = 5.0f;
                            enemyList.Add(newEnemy);
                            tempX += newEnemy.sprite.sprite.Width / 2;
                        }
                    }
                    
                    createWall = false;
                }
            }
            if (state.IsKeyUp(Keys.L))
            {
                createWall = true;
            }
            //Console.WriteLine(enemyList.Count);
        }

        /// <summary>
        /// returns the point on the circle you would like to be on, pretty expensive operation, keep use low
        /// </summary>
        /// <param name="angle">angle you would like to be at</param>
        /// <param name="currentPos">your current position</param>
        /// <param name="centre">what you would like to rotate arounds center</param>
        /// <returns></returns>
        public Vector2 Rotate(float angle, Vector2 currentPos, Vector2 center)
        {
            double distance = Math.Sqrt(Math.Pow(currentPos.X - center.X, 2) + Math.Pow(currentPos.Y - center.Y, 2));
            return new Vector2((float)(distance * Math.Cos(angle)), (float)(distance * Math.Sin(angle))) + center;
        }

        public float CalculateLookAt(Vector2 position1, Vector2 position2)
        {
            return (float)(Math.Atan2(position1.Y - position2.Y, position1.X - position2.X) + Math.PI/2);
        }
    }
}
