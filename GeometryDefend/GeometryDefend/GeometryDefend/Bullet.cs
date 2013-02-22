using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryDefend
{
    public class Bullet
    {
        public Vector2 position;
        public Vector2 velocity;
        public float rotation;

        public SpriteManager sprite;

        public bool isAlive = true;

        public Rectangle collisionBox;

        public Bullet(Texture2D texture, Vector2 pos, Vector2 vel, float rot)
        {
            sprite = new SpriteManager(texture, texture.Width, texture.Height, texture.Width, texture.Height, pos, rot, 1);
            position = pos;
            velocity = vel;

            collisionBox = new Rectangle((int)(pos.X - (texture.Width / 2) /2), (int)(pos.Y - (texture.Height / 2)/2), (int)(texture.Width * 0.5f), (int)(texture.Height * 0.5f));
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
            collisionBox.X = (int)position.X - sprite.sprite.Width / 2;
            collisionBox.Y = (int)position.Y - sprite.sprite.Height / 2;

            sprite.position = position;
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Color.LawnGreen);
        }
    }
}
