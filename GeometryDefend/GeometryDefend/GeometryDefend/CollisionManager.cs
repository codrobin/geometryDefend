using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDefend
{
    public class CollisionManager
    {
        //handling the particles in here
        ParticleEngine particleEngine;
        Texture2D breakEnemy;
        Texture2D normal;

        public CollisionManager(Texture2D particleTexture, Texture2D smallEnemy, Texture2D normalEnemy)
        {
            particleEngine = new ParticleEngine(particleTexture, new Vector2(100, 100));
            breakEnemy = smallEnemy;
            normal = normalEnemy;
        }

        /// <summary>
        /// pass in and set up what you need to check collisions for, that way we don't clutter gamemain
        /// </summary>
        public void Update(List<Bullet> bulletList, List<Enemy> enemyList, Player player, List<BackgroundElement> backgroundList)
        {
            //checking bullets against screen
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (CheckScreenBounds(bulletList[i].position, bulletList[i].sprite.sprite.Width, bulletList[i].sprite.sprite.Height, 0))
                {
                    //bullet is out of the screen, set to false for safe deletion. 
                    bulletList[i].isAlive = false;
                    particleEngine.EmitterLocation = bulletList[i].position;
                    particleEngine.GenerateNewParticle(5, Color.LawnGreen);
                }
            }
            //check enemies against player
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (CheckCircleCollision(player, enemyList[i].collisionBox) && enemyList[i].isAlive == true)
                {
                    player.health -= 12.5f;
                    enemyList[i].isAlive = false;
                }
            }

            //check slow enemies against walls
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].hasVelocity == true)
                {
                    CheckScreenBounds(enemyList[i], enemyList[i].sprite.sprite.Width, enemyList[i].sprite.sprite.Height, enemyList);
                }
            }

            //checking bullets against enemies
            for (int i = 0; i < enemyList.Count; i++)
            {
                for (int j = 0; j < bulletList.Count; j++)
                {
                    if (CheckCollision(enemyList[i].collisionBox, bulletList[j].collisionBox) && bulletList[j].isAlive == true)
                    {
                        if (enemyList[i].isBreak)
                        {
                            Enemy newEnemy = new Enemy(breakEnemy, enemyList[i].position, 0f, "small", Color.Magenta);
                            enemyList.Add(newEnemy);
                        }
                        enemyList[i].isAlive = false;
                        bulletList[j].isAlive = false;
                        particleEngine.EmitterLocation = enemyList[i].position;
                        particleEngine.GenerateNewParticle(25, enemyList[i].color);
                        particleEngine.GenerateNewParticle(5, Color.Lime);
                    }
                }
            }

            //check background against middle circle
            for (int i = 0; i < backgroundList.Count; i++)
            {
                if (CheckCircleCollisionBackground(player, backgroundList[i].collisionBox) && backgroundList[i].isAlive == true)
                {
                    backgroundList[i].isAlive = false;
                }
            }

            particleEngine.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            particleEngine.Draw(spriteBatch);
        }

        //built in collision
        public bool CheckCollision(Rectangle one, Rectangle two)
        {
            if (one.Intersects(two))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// returns true if out of the screen
        /// </summary>
        /// <param name="position">position</param>
        /// <returns></returns>
        public bool CheckScreenBounds(Vector2 position, float width, float height, float buffer)
        {
            if (position.X < 5 - buffer)
            {
                return true;
            }
            if (position.X + width > 1300 + buffer)
            {
                return true;
            }
            if (position.Y < 5 - buffer)
            {
                return true;
            }
            if (position.Y + height > 820 + buffer)
            {
                return true;
            }

            return false;
        }

        public void CheckScreenBounds(Enemy enemy, float width, float height, List<Enemy> enemyList)
        {
            if (enemy.collisionBox.X <= 0)
            {
                enemy.velocity.X *= -1;
                //enemy.position.X = width/2 + 5;
            }
            if (enemy.collisionBox.X + width >= 1300)
            {
                enemy.velocity.X *= -1;
                //enemy.position.X = 1280 - width/2;
            }
            if (enemy.collisionBox.Y <= 0)
            {
                enemy.velocity.Y *= -1;
                //enemy.position.Y = height / 2 + 5;
            }
            if (enemy.collisionBox.Y + height >= 820)
            {
                enemy.velocity.Y *= -1;
                //enemy.position.Y = 800 - height / 2;
            }
        }

        public bool CheckCircleCollision(Player player, Rectangle rectangle)
        {
            Vector2 distance = new Vector2(Math.Abs(630 - rectangle.X), Math.Abs(390 - rectangle.Y));

            if (distance.X > (rectangle.Width / 2 + player.radius))
            {
                return false;
            }
            if (distance.Y > (rectangle.Height / 2 + player.radius))
            {
                return false;
            }

            if (distance.X <= (rectangle.Width / 2))
            {
                return true;
            }
            if (distance.Y <= (rectangle.Height / 2))
            {
                return true;
            }

            float cornerDistance = (float)Math.Pow((distance.X - rectangle.Width / 2), 2) + (float)Math.Pow((distance.Y - rectangle.Height / 2), 2);

            return (cornerDistance <= (float)Math.Pow(player.radius, 2));
        }

        public bool CheckCircleCollisionBackground(Player player, Rectangle rectangle)
        {
            Vector2 distance = new Vector2(Math.Abs(640 - rectangle.X), Math.Abs(400 - rectangle.Y));

            if (distance.X > (rectangle.Width / 2 + player.radius - 5))
            {
                return false;
            }
            if (distance.Y > (rectangle.Height / 2 + player.radius - 5))
            {
                return false;
            }

            if (distance.X <= (rectangle.Width / 2))
            {
                return true;
            }
            if (distance.Y <= (rectangle.Height / 2))
            {
                return true;
            }

            float cornerDistance = (float)Math.Pow((distance.X - rectangle.Width / 2), 2) + (float)Math.Pow((distance.Y - rectangle.Height / 2), 2);

            return (cornerDistance <= (float)Math.Pow(player.radius - 5, 2));
        }

        //need these to be built specifly for the game this is incorporated in
        public bool CheckPixelCollision(Player player, Enemy enemy)
        {
            //TODO: do pixel collision
            return false;
        }
    }
}
