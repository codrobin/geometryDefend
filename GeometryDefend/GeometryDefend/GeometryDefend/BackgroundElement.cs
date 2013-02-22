using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDefend
{
    public class BackgroundElement
    {
        public SpriteManager sprite;

        public Vector2 position;
        public Vector2 velocity;
        public int maxVelocity;
        public Color color;

        public Rectangle collisionBox;
        public bool isAlive = true;
        public bool hasVelocity = false;

        public int timeToLive = 5000;

        public bool isSpawnElement;

        public BackgroundElement(Texture2D texture, Vector2 pos, int maxVel, Color color, bool spawnElement)
        {
            position = pos;
            sprite = new SpriteManager(texture, texture.Width, texture.Height, texture.Width, texture.Height, pos, 0f, 1);
            collisionBox = new Rectangle((int)(pos.X - (texture.Width / 2) / 2), (int)(pos.Y - (texture.Height / 2) / 2), (int)(texture.Width * 0.5f), (int)(texture.Height * 0.5f)); // times 0.5 for scaling
            this.color = color;
            maxVelocity = maxVel;

            isSpawnElement = spawnElement;
        }

        public void Update(GameTime gameTime, Vector2 newPosition)
        {
            if (isSpawnElement == false)
            {
                //get movement
                HandleMovement();
            }
            else
            {
                HandleMovement(newPosition);
            }

            //movement
            position += velocity;
            collisionBox.X = (int)position.X - (sprite.sprite.Width / 2) / 2;
            collisionBox.Y = (int)position.Y - (sprite.sprite.Height / 2) / 2;
            sprite.position = position;

            //decrease time to live
            timeToLive--;

            if (timeToLive <= 0)
            {
                isAlive = false;
            }

            //basic updates
            sprite.Update(gameTime);
        }

        public void HandleMovement()
        {
            if (hasVelocity == false)
            {
                Vector2 vectorToMiddle = new Vector2(640, 400) - position;
                vectorToMiddle.Normalize();
                velocity = new Vector2(1, 1);
                velocity *= vectorToMiddle;
                hasVelocity = true;
            }
        }

        public void HandleMovement(Vector2 newPosition)
        {
            Vector2 vectorToMiddle = newPosition - position;
            vectorToMiddle.Normalize();
            velocity = new Vector2(1, 1);
            velocity *= vectorToMiddle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, color);
        }
    }
}
