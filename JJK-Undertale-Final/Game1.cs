using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace JJK_Undertale_Final
{
    public class Game1 : Game
    {
        Random random = new Random();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D titleImg, dodgeBoxT, soulSpirteT, enemy1Attack;
        SpriteFont titleFont;
        Song titleSong;
        Rectangle window, dodgeBoxR, soulSpriteR;
        float titleO, titleTime, invincibilityTime;
        Vector2 openingText, controlsText, skipOption, startOption;
        GamePadState gamePadCurrent, gamePadOld;
        enum Screen
        {
            Title, Opening, Menu, Battle, Select, Win, Lose
        }
        Screen screen = Screen.Title;
        enum Battle
        {
            b1, b2, b3, b4, b5, b6, b7, b8, b9, b10
        }
        Battle battle;
        List<string> skillList = new List<string>();
        List<Texture2D> enemyTextures = new List<Texture2D>();
        List<Texture2D> backgrounds = new List<Texture2D>();
        List<Texture2D> punchAnimation = new List<Texture2D>();

        List<b1Projectile> b1Projectiles = new List<b1Projectile>();
        float projectileSpeed = 180f;
        float projectileTime = 0f;
        float projectileInterval = 1.3f;

        float soulSpeed = 300f;
        float dodgeTime = 15f;
        int charHealth = 100, charHealthMax = 100;
        bool dodgePhase = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            window = new Rectangle(0, 0, 1500, 900);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            openingText = new Vector2(50, 950);
            controlsText = new Vector2(50, 200);
            skipOption = new Vector2(950, 800);
            startOption = new Vector2(400, 800);

            dodgeBoxR = new Rectangle(450, 450, 600, 350);
            soulSpriteR = new Rectangle(dodgeBoxR.Center.X - 15, dodgeBoxR.Center.Y - 15, 30, 30);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            titleImg = Content.Load<Texture2D>("titleScreen");
            titleSong = Content.Load<Song>("Sounds/titleTheme");
            titleFont = Content.Load<SpriteFont>("titleFont");
            dodgeBoxT = Content.Load<Texture2D>("dodgeBox");
            soulSpirteT = Content.Load<Texture2D>("soulSprite");

            enemyTextures.Add(Content.Load<Texture2D>("Enemies/firstBattleCurse"));

            backgrounds.Add(Content.Load<Texture2D>("Backgrounds/menuBg"));
            backgrounds.Add(Content.Load<Texture2D>("Backgrounds/battle1Bg"));

            enemy1Attack = Content.Load<Texture2D>("EnemyMoves/b1Attack");

            for (int i = 1; i < 6; i++)
            {
                punchAnimation.Add(Content.Load<Texture2D>("charAttacks/punch" + i));
            }
            MediaPlayer.Volume = 0.6f;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0)
                Exit();

            // TODO: Add your update logic here
            gamePadOld = gamePadCurrent;
            gamePadCurrent = GamePad.GetState(PlayerIndex.One);
            if (screen == Screen.Title)
            {
                titleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                titleO = 0f;
                if (titleTime <= 0.5f)
                    titleO = titleTime / 0.5f;
                else if (titleTime <= 1.5f)
                    titleO = 1;
                else if (titleTime <= 2f)
                    titleO = 1f - ((titleTime - 1.5f) / 0.5f);
                else
                {
                    titleO = 0;
                    titleTime = 0;
                }

                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(titleSong);
                }
                if (gamePadCurrent.Buttons.BigButton == ButtonState.Pressed && gamePadOld.Buttons.BigButton == ButtonState.Released)
                {
                    screen = Screen.Opening;
                    titleTime = 0f;
                }
            }
            else if (screen == Screen.Opening)
            {
                titleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                openingText.Y -= 2;
                if (gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                    titleTime = 22.1f;
                if (gamePadCurrent.Buttons.BigButton == ButtonState.Pressed && gamePadOld.Buttons.BigButton == ButtonState.Released)
                {
                    screen = Screen.Battle;
                    battle = Battle.b1;
                    titleTime = 0f;
                }
            }
            else if (screen == Screen.Battle)
            {
                float battleTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (battle == Battle.b1 && dodgePhase)
                {
                    Vector2 movement = gamePadCurrent.ThumbSticks.Left;
                    movement.Y *= -1;

                    if (movement.LengthSquared() > 1f)
                        movement.Normalize();
                    soulSpriteR.X += (int)(movement.X * soulSpeed * battleTime);
                    soulSpriteR.Y += (int)(movement.Y * soulSpeed * battleTime);
                    projectileTime += battleTime;
                    
                    if (projectileTime >= projectileInterval)
                    {
                        SpawnProjectiles();
                        projectileTime = 0f;
                    }

                    for (int i = b1Projectiles.Count - 1; i >= 0; i--)
                    {
                        b1Projectile b1projectile = b1Projectiles[i];
                        b1projectile.Position += b1projectile.Direction * projectileSpeed * battleTime;

                        Rectangle b1projectileR = new Rectangle((int)b1projectile.Position.X - 12, (int)b1projectile.Position.Y - 12, 24, 24);

                        if (b1projectileR.Intersects(soulSpriteR) && invincibilityTime <= 0f)
                        {
                            charHealth -= 10;
                            invincibilityTime = 1f;

                            b1Projectiles.RemoveAt(i);
                            continue;
                        }
                    }
                }
            }
                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (screen == Screen.Title)
            {
                _spriteBatch.Draw(titleImg, window, Color.White * titleO);
                _spriteBatch.DrawString(titleFont, "Press START to start", startOption, Color.White);
            }
            if (screen == Screen.Opening)
            {
                DrawOpeningText();
            }
            if (screen == Screen.Battle)
            {
                if (battle == Battle.b1)
                {
                    _spriteBatch.Draw(backgrounds[1], window, Color.White);
                    _spriteBatch.Draw(enemyTextures[0], new Rectangle(550, 300, 400, 400), Color.White);
                    _spriteBatch.DrawString(titleFont, "A grade 4 curse has appeared!", new Vector2(startOption.X, 50), Color.White);
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        void DrawOpeningText()
        {
            if (titleTime <= 22f)
            {
                _spriteBatch.DrawString(titleFont, "Press A to skip", skipOption, Color.White * 0.2f);
                _spriteBatch.DrawString(titleFont, "Welcome to the Culling Games!", openingText, Color.White);
                _spriteBatch.DrawString(titleFont, "Curses are going around like crazy!", new Vector2(openingText.X, openingText.Y + 300), Color.White);
                _spriteBatch.DrawString(titleFont, "It's your j ob to stop them.", new Vector2(openingText.X, openingText.Y + 500), Color.White);
                _spriteBatch.DrawString(titleFont, "You will get skills and items as you fight.", new Vector2(openingText.X, openingText.Y + 700), Color.White);
                _spriteBatch.DrawString(titleFont, "Avoid attacks as best you can.", new Vector2(openingText.X, openingText.Y + 900), Color.White);
                _spriteBatch.DrawString(titleFont, "Enemies will get progressively harder!", new Vector2(openingText.X, openingText.Y + 1100), Color.White);
                _spriteBatch.DrawString(titleFont, "I'll give you an easy enemy to start.", new Vector2(openingText.X, openingText.Y + 1300), Color.White);
                _spriteBatch.DrawString(titleFont, "Good Luck!", new Vector2(openingText.X, openingText.Y + 1500), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(titleFont, "Controls:", new Vector2(controlsText.X, controlsText.Y - 90), Color.White);
                _spriteBatch.DrawString(titleFont, "Movement = Joystick", controlsText, Color.White);
                _spriteBatch.DrawString(titleFont, "Select = A", new Vector2(controlsText.X, controlsText.Y + 90), Color.White);
                _spriteBatch.DrawString(titleFont, "Back = B", new Vector2(controlsText.X, controlsText.Y + 180), Color.White);
                _spriteBatch.DrawString(titleFont, "Exit = Z (on the back)", new Vector2(controlsText.X, controlsText.Y + 270), Color.White);
                _spriteBatch.DrawString(titleFont, "Press START to start", startOption, Color.White);
            }
        }
        void SpawnProjectiles()
        {
            Vector2 spawnPosition = Vector2.Zero;
            int side = random.Next(4);
            switch (side)
            {
                case 0:
                    spawnPosition = new Vector2(random.Next(dodgeBoxR.Left, dodgeBoxR.Right), dodgeBoxR.Top - 40);
                    break;

                case 1:
                    spawnPosition = new Vector2(random.Next(dodgeBoxR.Left, dodgeBoxR.Right), dodgeBoxR.Bottom + 40);
                    break;

                case 2:
                    spawnPosition = new Vector2(dodgeBoxR.Left - 40, random.Next(dodgeBoxR.Top, dodgeBoxR.Bottom));
                    break;
                
                case 3:
                    spawnPosition = new Vector2(dodgeBoxR.Right - 40, random.Next(dodgeBoxR.Top, dodgeBoxR.Bottom));
                    break;
            }
            Vector2 soulCenter = new Vector2(soulSpriteR.Center.X, soulSpriteR.Center.Y);
            Vector2 direction = soulCenter - spawnPosition;
            direction.Normalize();

            float rotation = (float)Math.Atan2(direction.X, direction.Y);

            b1Projectiles.Add(new b1Projectile(spawnPosition, direction, rotation));
        }
    }
}
