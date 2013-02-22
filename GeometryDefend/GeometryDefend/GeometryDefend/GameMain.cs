using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDefend
{
    /// <summary>
    /// Used for the main loop and call class calls for that pretty OOP
    /// </summary>
    public class GameMain
    {
        //variables
        Texture2D playerCircle;
        Vector2 playerCirclePosition;
        CollisionManager collisionManager;

        Player playerOne;
        Player playerTwo;
        List<Bullet> bulletList = new List<Bullet>();
        List<Enemy> enemyList = new List<Enemy>();
        List<BackgroundElement> backgroundList = new List<BackgroundElement>();

        //debug---
        Texture2D enemyTexture;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;
        SpriteFont font;

        //background stuff test
        TimeSpan particleSpawnTimer = new TimeSpan(0, 0, 0, 0, 500);//milliseconds
        TimeSpan timeElasped;
        bool createParticle = true;
        Texture2D particle;
        Random random;
        //--------

        List<Texture2D> healths = new List<Texture2D>();
        Color color;
        int healthIndex = 0;

        public GameMain(ContentManager content)
        {
            playerCircle = content.Load<Texture2D>("circleShield");
            playerCirclePosition = new Vector2((1280 / 2), (800 / 2));

            playerOne = new Player(content.Load<Texture2D>("playerOne"), bulletList, content.Load<Texture2D>("whiteBullet"));
            playerOne.playerOne = true;
            playerTwo = new Player(content.Load<Texture2D>("playerTwo"), bulletList, content.Load<Texture2D>("whiteBullet"));

            enemyTexture = content.Load<Texture2D>("whiteEnemyBasic");

            collisionManager = new CollisionManager(content.Load<Texture2D>("whiteParticle1"), content.Load<Texture2D>("whiteEnemyBasicSmall"), enemyTexture);

            font = content.Load<SpriteFont>("myFont");

            for (int i = 8; i > 0; i--)
            {
                Texture2D newTexture = content.Load<Texture2D>("circleArmor" + (i).ToString());
                healths.Add(newTexture);
            }

            particle = content.Load<Texture2D>("whiteParticle1");
            random = new Random();
        }

        //main update
        public void Update(GameTime gameTime)
        {
            //background
            CreateBackground(gameTime);

            //players
            playerOne.Update(gameTime);
            playerTwo.Update(gameTime);

            //enemies
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Update(gameTime);   
            }

            //bullets
            for (int i = 0; i < bulletList.Count; i++)
            {
                bulletList[i].Update(gameTime);
            }

            //background
            for (int i = 0; i < backgroundList.Count; i++)
            {
                backgroundList[i].Update(gameTime, new Vector2(400,200));
            }

            //collisions
            collisionManager.Update(bulletList, enemyList, playerOne, backgroundList);

            //for debug testing
            playerOne.HandleInput(gameTime, enemyList, enemyTexture);

            //framerate
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }

            CleanUp();
        }

        //main draw
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            //health
            CheckHealth();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            //framerate
            _total_frames++;
            spriteBatch.DrawString(font, "FPS: " + _fps.ToString(), new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(font, "Health: " + playerOne.health.ToString(), new Vector2(100, 150), Color.White);

            //background
            spriteBatch.Draw(playerCircle, playerCirclePosition, null, Color.DeepSkyBlue, 0f, new Vector2(playerCircle.Width/2, playerCircle.Height/2), 0.5f, SpriteEffects.None, 0);
            if (healthIndex < 8)
            {
                spriteBatch.Draw(healths[healthIndex], playerCirclePosition, null, color, 0f, new Vector2(playerCircle.Width / 2, playerCircle.Height / 2), 0.5f, SpriteEffects.None, 0);
            }

            //player
            playerOne.Draw(spriteBatch);
            playerTwo.Draw(spriteBatch);

            //enemies
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Draw(spriteBatch, device);
            }

            //bullets
            for (int i = 0; i < bulletList.Count; i++)
            {
                bulletList[i].Draw(spriteBatch);
            }

            //background
            for (int i = 0; i < backgroundList.Count; i++)
            {
                backgroundList[i].Draw(spriteBatch);
            }

            //particles
            collisionManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void CreateBackground(GameTime gameTime)
        {
            if (createParticle == true)
            {
                for (int i = 0; i < 1; i++)
                {
                    byte ColorR = (byte)random.Next(0, 255);

                    byte ColorG = (byte)random.Next(0, 255);

                    byte ColorB = (byte)random.Next(0, 255);

                    //BackgroundElement newBackgroundElement = new BackgroundElement(particle, new Vector2(random.Next(20, 1260), random.Next(20, 780)), 1, new Color(ColorR, ColorG, ColorB), false);
                    //backgroundList.Add(newBackgroundElement);
                    CreateSpawnParticles(new Vector2(400, 200));
                }

                createParticle = false;
                timeElasped = gameTime.TotalGameTime;
            }
            if (createParticle == false)
            {
                if (timeElasped + particleSpawnTimer < gameTime.TotalGameTime)
                {
                    //createParticle = true;
                }
            }
        }

        public void CreateSpawnParticles(Vector2 spawnPosition)
        {
            for (int i = 0; i < 100; i++)
            {
                Vector2 createAt = new Vector2(random.Next((int)spawnPosition.X - 50, (int)spawnPosition.X + 50), random.Next((int)spawnPosition.Y - 50, (int)spawnPosition.Y + 50));
                BackgroundElement newBackgroundElement = new BackgroundElement(particle, createAt, 1, Color.White, true);
                newBackgroundElement.timeToLive = 100;
                backgroundList.Add(newBackgroundElement);
            }
        }

        public void CleanUp()
        {
            //go backwards so  you don't remove an element changing its size before it goes through them all
            for (int i = bulletList.Count - 1; i >= 0; i--)
            {
                if (bulletList[i].isAlive == false)
                {
                    bulletList.Remove(bulletList[i]);
                }
            }

            for (int i = enemyList.Count - 1; i >= 0; i--)
            {
                if (enemyList[i].isAlive == false)
                {
                    enemyList.Remove(enemyList[i]);
                }
            }

            for (int i = backgroundList.Count - 1; i >= 0; i--)
            {
                if (backgroundList[i].isAlive == false)
                {
                    backgroundList.Remove(backgroundList[i]);
                }
            }
        }

        public void CheckHealth()
        {
            if (playerOne.health == 100)
            {
                healthIndex = 0;
                color = Color.Green;
                return;
            }
            if (playerOne.health == 87.5)
            {
                healthIndex = 1;
                color = Color.GreenYellow;
                return;
            }
            if (playerOne.health == 75)
            {
                healthIndex = 2;
                color = Color.Yellow;
                return;
            }
            if (playerOne.health == 62.5)
            {
                healthIndex = 3;
                color = Color.Gold;
                return;
            }
            if (playerOne.health == 50)
            {
                healthIndex = 4;
                color = Color.Orange;
                return;
            }
            if (playerOne.health == 37.5)
            {
                healthIndex = 5;
                color = Color.DarkOrange;
                return;
            }
            if (playerOne.health == 25)
            {
                healthIndex = 6;
                color = Color.OrangeRed;
                return;
            }
            if (playerOne.health == 12.5)
            {
                healthIndex = 7;
                color = Color.Red;
                return;
            }
            if (playerOne.health <= 0)
            {
                healthIndex = 8;
                return;
            }
        }
    }
}
