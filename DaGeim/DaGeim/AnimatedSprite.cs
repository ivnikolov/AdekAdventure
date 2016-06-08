﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


    public abstract class AnimatedSprite
    {
        protected Texture2D spriteTexture;
        protected Vector2 playerPosition;
        protected Vector2 spriteDirection = Vector2.Zero;
        private Rectangle[] textureArea;
        private Vector2 shootPosition;
        protected Texture2D shootTextureRight;
        protected Texture2D shootTextureLeft;
        private bool shotCollision = false;

        private int frameIndex;
        private double timeElapsed;
        private double timeToUpdate;
        protected string currentAnimation;
        protected bool attacking = false;


        private int shoot_X_offset;
        private int shoot_Y_offset;
        private bool isShootRight = true;

    private Dictionary<string, Rectangle[]> spriteAnimations = new Dictionary<string, Rectangle[]>();

        public enum PlayerDirection {None, Up, Down,  Left, Right}
        protected PlayerDirection currentDirection = PlayerDirection.None;

        public int FramesPerSecond
        {
            set { timeToUpdate = (1f/value); }
        }

        public AnimatedSprite(Vector2 position)
        {
            playerPosition = position;
        }

        public void AddAnimation(string name, int frameCount, int yPos)
        {
            Rectangle[] animationSet = new Rectangle[frameCount];

            for (int i = 0; i < frameCount; i++)
                animationSet[i] = new Rectangle((i) * 100, yPos, 100, 100);      

            spriteAnimations.Add(name, animationSet);
        }

        public virtual void Update(GameTime gameTime)
        {

            if (!shotCollision && isShootRight)
                shootPosition.X += 3;
            else if (!shotCollision && !isShootRight)
                shootPosition.X -= 3;
            else
                attacking = false;

            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;

                if (frameIndex < (spriteAnimations[currentAnimation].Length - 1))
                    frameIndex++;
                else
                {
                    AnimationDone(currentAnimation);
                    frameIndex = 0;
                }
            }
        }

        public void InitializeRocket(Vector2 startPos, string direction)
        {
            if (direction == "right")
            {
                shoot_X_offset = 80;
                shoot_Y_offset = 35;
                isShootRight = true;
            }
            else
            {
                shoot_X_offset = -15;
                shoot_Y_offset = 35;
                isShootRight = false;
            }

            shootPosition.X = startPos.X + shoot_X_offset;
            shootPosition.Y = startPos.Y + shoot_Y_offset;
        }

    public void Draw(SpriteBatch spriteBatch)
        {
            if (attacking && isShootRight)
                spriteBatch.Draw(shootTextureRight, shootPosition, Color.White);
            else if (attacking && !isShootRight)
                spriteBatch.Draw(shootTextureLeft, shootPosition, Color.White);
            spriteBatch.Draw(spriteTexture, playerPosition, spriteAnimations[currentAnimation][frameIndex], Color.White);
        }

        public void PlayAnimation(string name)
        {
            if (currentAnimation != name)
            {
                currentAnimation = name;
                frameIndex = 0;
            }
        }
        public abstract void AnimationDone(string animation);
    }