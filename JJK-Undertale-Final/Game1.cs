using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Data;

namespace JJK_Undertale_Final
{
    public class Game1 : Game
    {
        Random random = new Random();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D titleImg, dodgeBoxT, soulSpirteT, enemy1Attack, charHBarT, enemyHBarT, charCEBarT, emptyBox, punchSelection, barTexture;
        SpriteFont titleFont;
        Song titleSong;
        Rectangle window, dodgeBoxR, soulSpriteR, charHR, charHBarR, charCEBarR, enemyHBarR, healthBox, battleMenu;
        float titleO, titleTime, invincibilityTime;
        Vector2 openingText, controlsText, skipOption, startOption;
        GamePadState gamePadCurrent, gamePadOld;
        enum Screen
        {
            Title, Opening, Menu, Battle, Select, SkillWeaponSelect, Win, Lose
        }
        Screen screen = Screen.Title;
        enum Battle
        {
            b1, b2, b3, b4, b5, b6, b7, b8, b9, b10
        }
        Battle battle;
        List<string> skillList = new List<string>();
        List<string> weaponList = new List<string>();
        List<Texture2D> enemyTextures = new List<Texture2D>();
        List<Texture2D> backgrounds = new List<Texture2D>();
        List<Texture2D> punchAnimation = new List<Texture2D>();

        List<b1Projectile> b1Projectiles = new List<b1Projectile>();
        float projectileSpeed = 300f;
        float projectileTime = 0f;
        float projectileInterval = 0.8f;

        float soulSpeed = 300f;
        float dodgeTime = 15f;
        int charHealth = 100, charHealthMax = 100;
        bool dodgePhase = true;
        int enemyHealth = 100, enemyHealthMax = 100;
        Rectangle enemyHR;
        Rectangle punchOptionR = new Rectangle(550, 550, 400, 120);
        int punchFrame;
        bool isPunching = false;
        float punchTimer = 0f;
        float punchFrameDuration = 0.12f;
        int punchDamage = 20;

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

            dodgeBoxR = new Rectangle(400, 400, 700, 300);
            charHR = new Rectangle(370, 775, 450, 150);
            charHBarR = new Rectangle(400, 805, 400, 90);
            healthBox = new Rectangle(-40, 750, 1575, 250);
            battleMenu = new Rectangle(-60, 400, 1625, 650);
            enemyHR = new Rectangle(525, -15, 450, 150);
            enemyHBarR = new Rectangle(537, 48, 425, 30);

            skillList.Add("Exploding Flesh");
            skillList.Add("Straw Doll");
            skillList.Add("Miracle");
            skillList.Add("Ice");
            skillList.Add("Blood");
            skillList.Add("Projection");
            skillList.Add("Ratio");
            skillList.Add("RCT");
            skillList.Add("Curse Control");
            skillList.Add("Bug Armour");
            skillList.Add("Granite Blast");
            skillList.Add("Dismantle");
            skillList.Add("Cleave");
            skillList.Add("Red");
            skillList.Add("Blue");
            skillList.Add("Beast Amber");
            skillList.Add("Fuga");
            skillList.Add("Hollow Purple");

            weaponList.Add("Slaughter Demon");
            weaponList.Add("Hand Sword");
            weaponList.Add("Basic Katana");
            weaponList.Add("Cursed Spear");
            weaponList.Add("Ratio Knife");
            weaponList.Add("Very Cursed Sword");
            weaponList.Add("Mallet");
            weaponList.Add("Dragon-Bone");
            weaponList.Add("Split Soul");
            weaponList.Add("Festering Life");
            weaponList.Add("Playful Cloud");
            weaponList.Add("Inverted Spear");
            
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
            emptyBox = Content.Load<Texture2D>("emptySlot");

            enemyTextures.Add(Content.Load<Texture2D>("Enemies/firstBattleCurse"));

            charHBarT = Content.Load<Texture2D>("charHealth");
            charCEBarT = Content.Load<Texture2D>("CEbar");
            barTexture = Content.Load<Texture2D>("tile");
            punchSelection = Content.Load<Texture2D>("Options/punchOption");
            enemyHBarT = Content.Load<Texture2D>("enemyHealth");

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
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                titleTime += deltaTime;
                if (titleTime <= 22f)
                    openingText.Y -= 120f * deltaTime;
                if (gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                    titleTime = 22.1f;

                if (titleTime > 22f && gamePadCurrent.Buttons.BigButton == ButtonState.Pressed && gamePadOld.Buttons.BigButton == ButtonState.Released)
                {
                    screen = Screen.Battle;
                    battle = Battle.b1;

                    dodgePhase = true;
                    dodgeTime = 15f;
                    projectileTime = 0f;
                    invincibilityTime = 0f;

                    b1Projectiles.Clear();
                    charHealth = charHealthMax;
                    enemyHealth = enemyHealthMax;
                    isPunching = false;
                    punchFrame = 0;
                    punchTimer = 0f;
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

                    if (soulSpriteR.Left < dodgeBoxR.Left)
                        soulSpriteR.X = dodgeBoxR.Left;
                    if (soulSpriteR.Right > dodgeBoxR.Right)
                        soulSpriteR.X = dodgeBoxR.Right - soulSpriteR.Width;
                    if (soulSpriteR.Top < dodgeBoxR.Top)
                        soulSpriteR.Y = dodgeBoxR.Top;
                    if (soulSpriteR.Bottom > dodgeBoxR.Bottom)
                        soulSpriteR.Y = dodgeBoxR.Bottom - soulSpriteR.Height;

                    projectileTime += battleTime;
                    if (projectileTime >= projectileInterval)
                    {
                        SpawnProjectiles();
                        projectileTime = 0f;
                    }

                    for (int i = b1Projectiles.Count - 1; i >= 0; i--)
                    {
                        b1Projectile projectile = b1Projectiles[i];
                        projectile.Position += projectile.Direction * projectileSpeed * battleTime;
                        Rectangle projectileR = new Rectangle((int)projectile.Position.X - 12, (int)projectile.Position.Y - 12, 24, 24);
                        if (projectileR.Intersects(soulSpriteR) && invincibilityTime <= 0f)
                        {
                            charHealth -= 10;
                            invincibilityTime = 1f;
                            b1Projectiles.RemoveAt(i);
                            continue;
                        }

                        if (projectile.Position.X < dodgeBoxR.Left - 60 || projectile.Position.X > dodgeBoxR.Right + 60 || projectile.Position.Y < dodgeBoxR.Top - 60 || projectile.Position.Y > dodgeBoxR.Bottom + 60)
                            b1Projectiles.RemoveAt(i);
                    }

                    if (invincibilityTime > 0f)
                        invincibilityTime -= battleTime;
                    dodgeTime -= battleTime;

                    if (charHealth <= 0)
                        screen = Screen.Lose;
                    else if (dodgeTime <= 0f)
                    {
                        dodgePhase = false;
                        b1Projectiles.Clear();
                        screen = Screen.Select;
                    }
                }
            }

            else if (screen == Screen.Select)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!isPunching && gamePadCurrent.Buttons.A == ButtonState.Pressed && gamePadOld.Buttons.A == ButtonState.Released)
                {
                    isPunching = true;
                    punchFrame = 0;
                    punchTimer = 0f;
                }

                if (isPunching)
                {
                    punchTimer += deltaTime;
                    if (punchTimer >= punchFrameDuration)
                    {
                        punchTimer = 0f;
                        punchFrame++;

                        if (punchFrame >= punchAnimation.Count)
                        {
                            isPunching = false;
                            punchFrame = 0;
                            enemyHealth -= punchDamage;

                            if (enemyHealth < 0)
                                enemyHealth = 0;

                            if (enemyHealth <= 0)
                                screen = Screen.Win;
                            else
                            {
                                screen = Screen.Battle;
                                dodgePhase = true;
                                dodgeTime = 15f;
                                projectileTime = 0f;

                                b1Projectiles.Clear();
                            }
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
                    _spriteBatch.Draw(enemyTextures[0], new Rectangle(550, 100, 400, 300), Color.White);
                    _spriteBatch.Draw(barTexture, enemyHBarR, Color.DarkRed);
                    int enemyBarWidth = (int)(enemyHBarR.Width * ((float)enemyHealth / enemyHealthMax));
                    if (enemyBarWidth > 0)
                        _spriteBatch.Draw(barTexture, new Rectangle(enemyHBarR.X, enemyHBarR.Y, enemyBarWidth, enemyHBarR.Height), Color.Red);
                    _spriteBatch.Draw(enemyHBarT, enemyHR, Color.White);
                    _spriteBatch.DrawString(titleFont, "Enemy HP: " + enemyHealth + "/" + enemyHealthMax, new Vector2(50, 35), Color.White);
                    _spriteBatch.Draw(dodgeBoxT, dodgeBoxR, Color.White);
                    foreach (b1Projectile b1projectile in b1Projectiles)
                    {
                        _spriteBatch.Draw(enemy1Attack, b1projectile.Position, null, Color.White, b1projectile.Rotation, new Vector2(enemy1Attack.Width / 2f, enemy1Attack.Height / 2f), 0.5f, SpriteEffects.None, 0f);
                    }
                    if (invincibilityTime <= 0f || (int)(invincibilityTime * 10) % 2 == 0)
                        _spriteBatch.Draw(soulSpirteT, soulSpriteR, Color.White);
                    _spriteBatch.Draw(emptyBox, healthBox, Color.White);
                    _spriteBatch.Draw(barTexture, charHBarR, Color.DarkRed);
                    int playerBarWidth = (int)(charHBarR.Width * ((float)Math.Max(0, charHealth) / charHealthMax));
                    if (playerBarWidth > 0)
                        _spriteBatch.Draw(barTexture, new Rectangle(charHBarR.X, charHBarR.Y, playerBarWidth, charHBarR.Height), Color.Green);
                    _spriteBatch.DrawString(titleFont, "HP: " + charHealth + "/" + charHealthMax, new Vector2(50, 800), Color.White);
                    _spriteBatch.Draw(charHBarT, charHR, Color.White);
                }
            }
            if (screen == Screen.Select)
            {
                if (battle == Battle.b1)
                {
                    _spriteBatch.Draw(backgrounds[1], window, Color.White);
                    _spriteBatch.Draw(enemyTextures[0], new Rectangle(550, 100, 400, 300), Color.White);
                    _spriteBatch.Draw(barTexture, enemyHBarR, Color.DarkRed);
                    int enemyBarWidth = (int)(enemyHBarR.Width * ((float)enemyHealth / enemyHealthMax));

                    if (enemyBarWidth > 0)
                        _spriteBatch.Draw(barTexture, new Rectangle(enemyHBarR.X, enemyHBarR.Y, enemyBarWidth, enemyHBarR.Height), Color.Red);
                        _spriteBatch.Draw(enemyHBarT, enemyHR, Color.White);
                    _spriteBatch.DrawString(titleFont, "Enemy HP: " + enemyHealth + "/" + enemyHealthMax, new Vector2(50, 35), Color.White);
                    _spriteBatch.Draw(emptyBox, battleMenu, Color.White);
                    _spriteBatch.Draw(barTexture, charHBarR, Color.DarkRed);
                    int playerBarWidth = (int)(charHBarR.Width * ((float)Math.Max(0, charHealth) / charHealthMax));
                    if (playerBarWidth > 0)
                        _spriteBatch.Draw(barTexture, new Rectangle(charHBarR.X, charHBarR.Y, playerBarWidth, charHBarR.Height), Color.Green);
                    _spriteBatch.DrawString(titleFont, "HP: " + charHealth + "/" + charHealthMax, new Vector2(50, 800), Color.White);
                    _spriteBatch.Draw(charHBarT, charHR, Color.White);
                    if (isPunching)
                        _spriteBatch.Draw(punchSelection, punchOptionR, Color.Yellow);
                    else
                        _spriteBatch.Draw(punchSelection, punchOptionR, Color.White);
                    if (isPunching && punchAnimation.Count > 0)
                        _spriteBatch.Draw(punchAnimation[punchFrame], new Rectangle(550, 100, 400, 300), Color.White);
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
                    spawnPosition = new Vector2(dodgeBoxR.Right + 40, random.Next(dodgeBoxR.Top, dodgeBoxR.Bottom));
                    break;
            }
            Vector2 soulCenter = new Vector2(soulSpriteR.Center.X, soulSpriteR.Center.Y);
            Vector2 direction = soulCenter - spawnPosition;
            direction.Normalize();

            float rotation = (float)Math.Atan2(direction.Y, direction.X);

            b1Projectiles.Add(new b1Projectile(spawnPosition, direction, rotation));
        }
    }
}
