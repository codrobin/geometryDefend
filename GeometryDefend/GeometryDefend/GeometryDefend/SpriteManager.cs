using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GeometryDefend
{
    public class SpriteManager
    {

        public Texture2D sprite;
        float timer = 0f;
        public float interval = 100f;

        public int currentFrame = 0;
        int numberOfFrames;
        public bool play = true;

        public int startFrame = 0;
        public int endFrame;

        int width;
        int height;
        int spriteWidth;
        int spriteHeight;

        public Rectangle sourceRect;
        public Vector2 origin;
        public Vector2 position;
        public float rotation;

        //public bool drawBox = false;
        //public int boxHalfSize = 75;
        //public Vector2 perspectiveSize = new Vector2(15, 15);
        //public bool isBuilding = false;

        GraphicsDevice graphicsDevice;

        /// <summary>
        /// needed if you want to hand draw a box around the sprite
        /// </summary>
        /// <param name="incSprite"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="spriteWidth"></param>
        /// <param name="spriteHeight"></param>
        /// <param name="pos"></param>
        /// <param name="totalFrames"></param>
        /// <param name="graphics"></param>
        public SpriteManager(Texture2D incSprite, int imageWidth, int imageHeight, int spriteWidth, int spriteHeight, Vector2 pos, int totalFrames, GraphicsDevice graphics)
        {
            sprite = incSprite;
            width = imageWidth;
            height = imageHeight;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            position = pos;
            numberOfFrames = totalFrames;
            endFrame = numberOfFrames;
            graphicsDevice = graphics;
        }

        /// <summary>
        /// use this one if your not planning to draw anything like lines around the sprite
        /// </summary>
        /// <param name="incSprite"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="spriteWidth"></param>
        /// <param name="spriteHeight"></param>
        /// <param name="pos"></param>
        /// <param name="totalFrames"></param>
        public SpriteManager(Texture2D incSprite, int imageWidth, int imageHeight, int spriteWidth, int spriteHeight, Vector2 pos, float rot, int totalFrames)
        {
            sprite = incSprite;
            width = imageWidth;
            height = imageHeight;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            position = pos;
            numberOfFrames = totalFrames;
            endFrame = numberOfFrames;
            rotation = rot;
        }

        public void Update(GameTime gameTime)
        {
            // increase the time
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // check the timer
            if ((timer > interval) && play == true)
            {
                //show the next frame
                currentFrame++;

                //reset timer
                timer = 0f;
            }

            //check for last frame
            if (currentFrame == endFrame)
            {
                currentFrame = startFrame;
            }

            //Math.Floor(currentFrame / (width / spriteWidth));

            sourceRect = new Rectangle((currentFrame % (width / spriteWidth)) * spriteWidth, (int)(currentFrame / (width / spriteWidth)) * spriteHeight, spriteWidth, spriteHeight);            
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        public void NonGameUpdate()
        {
            //sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            sourceRect = new Rectangle((currentFrame % (width / spriteWidth)) * spriteWidth, (int)(currentFrame / (width / spriteWidth)) * spriteHeight, spriteWidth, spriteHeight);
            origin = new Vector2(0, 0);            
        }

        /// <summary>
        /// for non camera games
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        public void Draw(SpriteBatch spriteBatch, Color colorToDraw)
        {
            spriteBatch.Draw(sprite, position, sourceRect, colorToDraw, rotation, origin, 0.5f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// for games that need a camera
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="newPosition">New position given by camera</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 newPosition)
        {
            spriteBatch.Draw(sprite, newPosition, sourceRect, Color.White, 0f, origin, 2.0f, SpriteEffects.None, 0);

            /*
            if (drawBox == true && isBuilding == false)
            {
                Vector2 point1;
                Vector2 point2;
                Vector2 point3;
                Vector2 point4;
                point1.X = newPosition.X - boxHalfSize;
                point1.Y = newPosition.Y - boxHalfSize;
                point2.X = newPosition.X + boxHalfSize;
                point2.Y = point1.Y;
                point3.X = point1.X;
                point3.Y = newPosition.Y + boxHalfSize;
                point4.X = point2.X;
                point4.Y = point3.Y;
                DrawLine(point1 + perspectiveSize, point2 - new Vector2(perspectiveSize.X, -perspectiveSize.Y), spriteBatch);
                DrawLine(point1 + perspectiveSize, point3, spriteBatch);
                DrawLine(point2 - new Vector2(perspectiveSize.X, -perspectiveSize.Y), point4, spriteBatch);
                DrawLine(point3, point4, spriteBatch);
            }

            if (isBuilding == true && drawBox == true)
            {
                //Console.WriteLine("got here");
                Vector2 point1;
                Vector2 point2;
                Vector2 point3;
                Vector2 point4;

                point1 = new Vector2(newPosition.X - boxHalfSize /2, newPosition.Y - boxHalfSize /2);
                point2 = new Vector2(newPosition.X + width*2 + boxHalfSize/2, newPosition.Y - boxHalfSize/2);
                point3 = new Vector2(newPosition.X - boxHalfSize/2, newPosition.Y + height*2 + boxHalfSize/2);
                point4 = new Vector2(newPosition.X + width*2 + boxHalfSize/2, newPosition.Y + height*2 + boxHalfSize/2);

                DrawLine(point1 + perspectiveSize, point2 - new Vector2(perspectiveSize.X, -perspectiveSize.Y), spriteBatch);
                DrawLine(point1 + perspectiveSize, point3, spriteBatch);
                DrawLine(point2 - new Vector2(perspectiveSize.X, -perspectiveSize.Y), point4, spriteBatch);
                DrawLine(point3, point4, spriteBatch);
            }*/
        }

        public void DrawLine(Vector2 point1, Vector2 point2, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float distance = (float)Vector2.Distance(point2, point1);
            Texture2D line = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color); // for some reason is null if called through map.
            line.SetData(new[] { Color.Yellow });
            spriteBatch.Draw(line, point1, null, Color.White, angle, Vector2.Zero, new Vector2(distance, 1), SpriteEffects.None, 0);            
        }


    }
}
