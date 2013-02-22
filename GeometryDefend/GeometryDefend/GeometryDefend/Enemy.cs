using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDefend
{
    public class Enemy
    {
        public SpriteManager sprite;

        public Vector2 position;
        public Vector2 velocity;
        public int maxVelocity;
        public Color color;

        public Rectangle collisionBox;

        //will be used for ai
        public string type;
        public bool isAlive = true;
        public bool isBreak = false;
        //snake ai

        //wall ai
        public bool xVelocity = false;
        public bool yVelocity = false;


        //soon
        public float rotation;

        public Random random;

        public bool hasVelocity = false;

        public Enemy(Texture2D texture, Vector2 pos, float rot, string typeOfEnemy, Color color)
        {
            position = pos;
            sprite = new SpriteManager(texture, texture.Width, texture.Height, texture.Width, texture.Height, pos, rot, 1);
            type = typeOfEnemy;
            collisionBox = new Rectangle((int)(pos.X - (texture.Width / 2) / 2), (int)(pos.Y - (texture.Height / 2) /2), (int)(texture.Width * 0.5f), (int)(texture.Height * 0.5f)); // times 0.5 for scaling
            this.color = color;
            maxVelocity = 5;

            random = new Random();
        }

        public void Update(GameTime gameTime)
        {
            //ai
            HandleAI();

            //movement
            position += velocity;
            collisionBox.X = (int)position.X - (sprite.sprite.Width /2) /2;
            collisionBox.Y = (int)position.Y - (sprite.sprite.Height /2) /2;
            sprite.position = position;

            //basic updates
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            sprite.Draw(spriteBatch, color);

            //sprite.DrawLine(new Vector2(collisionBox.X, collisionBox.Y), new Vector2(collisionBox.X + collisionBox.Width, collisionBox.Y), spriteBatch, device);
            //sprite.DrawLine(new Vector2(collisionBox.X, collisionBox.Y), new Vector2(collisionBox.X, collisionBox.Y + collisionBox.Height), spriteBatch, device);
            //sprite.DrawLine(new Vector2(collisionBox.X + collisionBox.Width, collisionBox.Y), new Vector2(collisionBox.X + collisionBox.Width, collisionBox.Y + collisionBox.Height), spriteBatch, device);
            //sprite.DrawLine(new Vector2(collisionBox.X, collisionBox.Y + collisionBox.Height), new Vector2(collisionBox.X + collisionBox.Width, collisionBox.Y + collisionBox.Height), spriteBatch, device);
        }

        public void HandleAI()
        {
            switch (type)
            {
                case "basic":
                    if (position.X < 640)
                    {
                        if (velocity.X < maxVelocity)
                        {
                            velocity.X += 0.1f;
                            //position.X += 10f;
                        }
                    }
                    if (position.X > 640)
                    {
                        if (velocity.X > -maxVelocity)
                        {
                            velocity.X -= 0.1f;
                            //position.X -= 10f;
                        }
                    }
                    if (position.Y < 400)
                    {
                        if (velocity.Y < maxVelocity)
                        {
                            velocity.Y += 0.1f;
                            //position.Y += 10f;
                        }
                    }
                    if (position.Y > 400)
                    {
                        if (velocity.Y > -maxVelocity)
                        {
                            velocity.Y -= 0.1f;
                            //position.Y -= 10f;
                        }
                    }
                    break;

                case "slow":
                    if (hasVelocity == false)
                    {
                        velocity.X = random.Next(-5, 5);
                        random = new Random();
                        velocity.Y = random.Next(-5, 5);
                        hasVelocity = true;
                    }
                    break;

                case "break":
                    if (hasVelocity == false)
                    {
                        Vector2 vectorToMiddle = new Vector2(640, 400) - position;
                        vectorToMiddle.Normalize();
                        velocity = new Vector2(1, 1);
                        velocity *= vectorToMiddle;
                        hasVelocity = true;
                    }
                    break;

                case "small":
                    Vector2 vectorToMiddle2 = new Vector2(640, 400) - position;
                    vectorToMiddle2.Normalize();
                    if (hasVelocity == false)
                    {
                        velocity = new Vector2(1, 1);
                        velocity *= -vectorToMiddle2 * 3;
                        hasVelocity = true;
                    }
                    //velocity *= vectorToMiddle2;
                    break;

                case "wall":
                    if (hasVelocity == false)
                    {
                        hasVelocity = true;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
