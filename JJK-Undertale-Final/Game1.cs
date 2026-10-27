using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.Design;
using System.Reflection.Metadata;

namespace JJK_Undertale_Final
{
    public class Game1 : Game
    {
        Random randomSkill = new Random(), randomWeapon = new Random(), random = new Random(), rarity = new Random();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D titleImg, dodgeBoxT, soulSpirteT, enemy1Attack, charHBarT, enemyHBarT, charCEBarT, emptyBox, punchSelection, barTexture;
        SpriteFont titleFont;
        Song titleSong;
        Rectangle window, dodgeBoxR, soulSpriteR, charHR, charHBarR, charCEBarR, enemyHBarR, healthBox, battleMenu, skillBox, weaponBox;
        float titleO, titleTime, invincibilityTime;
        Vector2 openingText, controlsText, skipOption, startOption, skillSelectText, weaponSelectText;
        GamePadState gamePadCurrent, gamePadOld;
        charAttack playerAttack;
        enum Screen
        {
            Title, Opening, Menu, Battle, Select, BattleRewards, Win, Lose
        }
        Screen screen = Screen.BattleRewards;
        enum Battle
        {
            b1, b2, b3, b4, b5, b6, b7, b8, b9, b10
        }
        Battle battle;
        List<string> skillList = new List<string>();
        List<string> weaponList = new List<string>();
        List<Texture2D> skillIcons = new List<Texture2D>();
        List<Texture2D> weaponIcons = new List<Texture2D>();
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

        int obtainedSkill, obtainedWeapon, skillRarity, weaponRarity;
        bool rewardsGenerated = false;

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

            skillBox = new Rectangle(100, 200, 500, 500);
            weaponBox = new Rectangle(800, 200, 500, 500);
            skillSelectText = new Vector2(40, 750);
            weaponSelectText = new Vector2(800, 750);
            
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

            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/explodingFleshIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/strawDollIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/miracleIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/iceIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/bloodIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/projectionIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/ratioIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/RCTIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/curseControlIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/bugArmourIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/graniteBlastIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/dismantleIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/cleaveIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/beastAmberIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/blueIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/redIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/fugaIcon"));
            skillIcons.Add(Content.Load<Texture2D>("SkillIcons/purpleIcon"));

            weaponIcons.Add(Content.Load<Texture2D>("Weapons/slaughterDemon"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/harutaSword"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/miwaSword"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/makiSpear"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/nanamiKnife"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/yutaSword"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/mallet"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/dragonBoneSword"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/splitSoul"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/festeringLife"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/playfulCloud"));
            weaponIcons.Add(Content.Load<Texture2D>("Weapons/invertedSpear"));

            enemy1Attack = Content.Load<Texture2D>("EnemyMoves/b1Attack");

            for (int i = 1; i < 6; i++)
            {
                punchAnimation.Add(Content.Load<Texture2D>("charAttacks/punch" + i));
            }
            playerAttack = new charAttack(punchAnimation);
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
                    playerAttack.Reset();
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
                if (!playerAttack._isPunching && gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                    playerAttack.Start();

                if (playerAttack.Update(gameTime))
                {
                    enemyHealth -= playerAttack._damage;
                    if (enemyHealth < 0)
                        enemyHealth = 0;
                    if (charHealth <= 0)
                        charHealth = 0;

                    if (enemyHealth <= 0)
                    {
                        rewardsGenerated = false;
                        screen = Screen.BattleRewards;
                    }
                    else if (charHealth <= 0)
                        screen = Screen.Lose;
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
            else if (screen == Screen.BattleRewards)
            {
                if (!rewardsGenerated)
                {
                    skillRarity = randomSkill.Next(1, 101);
                    weaponRarity = randomWeapon.Next(1, 101);

                    if (skillRarity <= 55)
                        obtainedSkill = randomSkill.Next(1, 4);
                    else if (skillRarity <= 80)
                        obtainedSkill = randomSkill.Next(1, 5);
                    else if (skillRarity <= 95)
                        obtainedSkill = randomSkill.Next(1, 5);
                    else if (skillRarity <= 100 && skillRarity > 95)
                        obtainedSkill = randomSkill.Next(1, 6);

                    if (weaponRarity <= 55)
                        obtainedWeapon = randomWeapon.Next(1, 5);
                    else if (weaponRarity <= 80)
                        obtainedWeapon = randomWeapon.Next(1, 4);
                    else if (weaponRarity <= 95)
                        obtainedWeapon = randomWeapon.Next(1, 4);
                    else if (weaponRarity <= 100)
                        obtainedWeapon = randomWeapon.Next(1, 3);
                    rewardsGenerated = true;
                    if (gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                    {
                        if (skillRarity <= 55)
                        {
                            if (obtainedSkill == 1 && !skillList.Contains("explodingFlesh"))
                                skillList.Add("explodingFlesh");
                            else if (obtainedSkill == 2 && !skillList.Contains("strawDoll"))
                                skillList.Add("strawDoll");
                            else if (obtainedSkill == 3 && !skillList.Contains("miracle"))
                                skillList.Add("miracle");
                        }
                        else if (skillRarity <= 80)
                        {
                            if (obtainedSkill == 1 && !skillList.Contains("ice"))
                                skillList.Add("ice");
                            else if (obtainedSkill == 2 && !skillList.Contains("blood"))
                                skillList.Add("blood");
                            else if (obtainedSkill == 3 && !skillList.Contains("projection"))
                                skillList.Add("projection");
                            else if (obtainedSkill == 4 && !skillList.Contains("ratio"))
                                skillList.Add("ratio");
                        }
                        else if (skillRarity <= 95)
                        {
                            if (obtainedSkill == 1 && !skillList.Contains("RCT"))
                                skillList.Add("RCT");
                            else if (obtainedSkill == 2 && !skillList.Contains("cursedControl"))
                                skillList.Add("curseControl");
                            else if (obtainedSkill == 3 && !skillList.Contains("bugArmour"))
                                skillList.Add("bugArmour");
                            else if (obtainedSkill == 4 && !skillList.Contains("graniteBlast"))
                                skillList.Add("graniteBlast");
                        }
                        else if (skillRarity <= 100 && skillRarity > 95)
                        {
                            if (obtainedSkill == 1 && !skillList.Contains("dismantle"))
                                skillList.Add("dismantle");
                            else if (obtainedSkill == 2 && !skillList.Contains("cleave"))
                                skillList.Add("cleave");
                            else if (obtainedSkill == 3 && !skillList.Contains("beastAmber"))
                                skillList.Add("beastAmber");
                            else if (obtainedSkill == 4 && !skillList.Contains("blue"))
                                skillList.Add("blue");
                            else if (obtainedSkill == 5 && !skillList.Contains("red"))
                                skillList.Add("red");
                        }

                        if (weaponRarity <= 55)
                        {
                            if (obtainedWeapon == 1 && !weaponList.Contains("slaughterDemon"))
                                weaponList.Add("slaughterDemon");
                            else if (obtainedWeapon == 2 && !weaponList.Contains("handSword"))
                                weaponList.Add("handSword");
                            else if (obtainedWeapon == 3 && !weaponList.Contains("basicKatana"))
                                weaponList.Add("basicKatana");
                            else if (obtainedWeapon == 4 && !weaponList.Contains("cursedSpear"))
                                weaponList.Add("cursedSpear");
                        }
                        else if (weaponRarity <= 80)
                        {
                            if (obtainedWeapon == 1 && !weaponList.Contains("ratioKnife"))
                                weaponList.Add("ratioKnife");
                            else if (obtainedWeapon == 2 && !weaponList.Contains("cursedSword"))
                                weaponList.Add("cursedSword");
                            else if (obtainedWeapon == 3 && !weaponList.Contains("mallet"))
                                weaponList.Add("mallet");
                        }
                        else if (weaponRarity <= 95)
                        {
                            if (obtainedWeapon == 1 && !weaponList.Contains("dragonBone"))
                                weaponList.Add("dragonBone");
                            else if (obtainedWeapon == 2 && !weaponList.Contains("soulSplit"))
                                weaponList.Add("soulSplit");
                            else if (obtainedSkill == 3 && !weaponList.Contains("festeringLife"))
                                weaponList.Add("festeringLife");
                        }
                        else if (weaponRarity <= 100)
                        {
                            if (obtainedWeapon == 1 && !weaponList.Contains("playfulCloud"))
                                weaponList.Add("playfulCloud");
                            else if (obtainedWeapon == 2 && !weaponList.Contains("invertedSpear"))
                                weaponList.Add("invertedSpear");
                        }
                        if (skillList.Contains("dismantle") && skillList.Contains("cleave") && !skillList.Contains("fuga"))
                            skillList.Add("fuga");
                        if (skillList.Contains("blue") && skillList.Contains("red") && !skillList.Contains("purple"))
                            skillList.Add("purple");
                        screen = Screen.Menu;
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
                    int playerBarWidth = (int)(charHBarR.Width * ((float)charHealth / charHealthMax));
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
                    int playerBarWidth = (int)(charHBarR.Width * ((float)charHealth / charHealthMax));
                    if (playerBarWidth > 0)
                        _spriteBatch.Draw(barTexture, new Rectangle(charHBarR.X, charHBarR.Y, playerBarWidth, charHBarR.Height), Color.Green);
                    _spriteBatch.DrawString(titleFont, "HP: " + charHealth + "/" + charHealthMax, new Vector2(50, 800), Color.White);
                    _spriteBatch.Draw(charHBarT, charHR, Color.White);
                    if (playerAttack._isPunching)
                        _spriteBatch.Draw(punchSelection, punchOptionR, Color.Yellow);
                    else
                        _spriteBatch.Draw(punchSelection, punchOptionR, Color.White);
                    playerAttack.Draw(_spriteBatch);
                }
            }
            if (screen == Screen.BattleRewards)
            {
                _spriteBatch.Draw(dodgeBoxT, window, Color.White);
                if (skillRarity < 55)
                {
                    if (obtainedSkill == 1)
                    {
                        if (skillList.Contains("explodingFlesh"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[0], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Exploding Flesh", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 2)
                    {
                        if (skillList.Contains("strawDoll"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[1], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Straw Doll", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 3)
                    {
                        if (skillList.Contains("miracle"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[2], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Miracle", skillSelectText, Color.White);
                        }
                    }
                }
                else if (skillRarity < 80 && skillRarity > 55)
                {
                    if (obtainedSkill == 1)
                    {
                        if (skillList.Contains("ice"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[3], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Ice", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 2)
                    {
                        if (skillList.Contains("blood"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[4], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Blood", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 3)
                    {
                        if (skillList.Contains("projection"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[5], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Projection", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 4)
                    {
                        if (skillList.Contains("ratio"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[6], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Ratio", skillSelectText, Color.White);
                        }
                    }
                }
                else if (skillRarity < 95 && skillRarity > 80)
                {
                    if (obtainedSkill == 1)
                    {
                        if (skillList.Contains("RCT"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[7], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "RCT", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 2)
                    {
                        if (skillList.Contains("curseControl"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[8], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Curse Control", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 3)
                    {
                        if (skillList.Contains("bugArmour"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[9], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Bug Armour", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 4)
                    {
                        if (skillList.Contains("graniteBlast"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[10], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Granite Blast", skillSelectText, Color.White);
                        }
                    }
                }
                else if (skillRarity <= 100 && skillRarity > 95)
                {
                    if (obtainedSkill == 1)
                    {
                        if (skillList.Contains("dismantle"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[11], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Dismantle", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 2)
                    {
                        if (skillList.Contains("cleave"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[12], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Cleave", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 3)
                    {
                        if (skillList.Contains("beastAmber"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[13], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Beast Amber", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 4)
                    {
                        if (skillList.Contains("blue"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[14], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Blue", skillSelectText, Color.White);
                        }
                    }
                    else if (obtainedSkill == 5)
                    {
                        if (skillList.Contains("red"))
                        {
                            _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this skill", skillSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(skillIcons[15], skillBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Red", skillSelectText, Color.White);
                        }
                    }
                }
                if (weaponRarity < 55)
                {
                    if (obtainedWeapon == 1)
                    {
                        if (weaponList.Contains("slaughterDemon"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[0], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Slaughter Demon", weaponSelectText, Color.White);
                        }
                    }
                    else if (obtainedWeapon == 2)
                    {
                        if (weaponList.Contains("handSword"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[1], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Hand Sword", weaponSelectText, Color.White);
                        }
                    }
                    else if (obtainedWeapon == 3)
                    {
                        if (weaponList.Contains("basicKatana"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[2], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Basic Katana", weaponSelectText, Color.White);
                        }
                    }
                    else if (obtainedWeapon == 4)
                    {
                        if (weaponList.Contains("cursedSpear"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[3], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Cursed Spear", weaponSelectText, Color.White);
                        }
                    }
                }
                else if (weaponRarity < 80 && weaponRarity > 55)
                {
                    if (obtainedWeapon == 1)
                    {
                        if (weaponList.Contains("ratioKnife"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[4], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Ratio Knife", weaponSelectText, Color.White);
                        }
                    }
                    else if (obtainedWeapon == 2)
                    {
                        if (weaponList.Contains("cursedKatana"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[5], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Cursed Katana", weaponSelectText, Color.White);
                        }
                    }
                    else if (obtainedWeapon == 3)
                    {
                        if (weaponList.Contains("mallet"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[6], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Mallet", weaponSelectText, Color.White);
                        }
                    }
                }
                else if (weaponRarity < 95 && weaponRarity > 80)
                {
                    if (obtainedWeapon == 1)
                    {
                        if (weaponList.Contains("dragonBone"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[7], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Dragon Bone", weaponSelectText, Color.White);
                        }
                    }
                    else if (obtainedWeapon == 2)
                    {
                        if (weaponList.Contains("splitSoul"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[8], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Split Soul", weaponSelectText, Color.White);
                        }
                    }
                    else if (obtainedWeapon == 3)
                    {
                        if (weaponList.Contains("festeringLife"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[9], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Festering Life", weaponSelectText, Color.White);
                        }
                    }
                }
                else if (weaponRarity <= 100 && weaponRarity > 95)
                {
                    if (obtainedWeapon == 1)
                    {
                        if (weaponList.Contains("playfulCloud"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[10], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Playful Cloud", weaponSelectText, Color.White);
                        }
                    }
                    else if (obtainedWeapon == 2)
                    {
                        if (weaponList.Contains("invertedSpear"))
                        {
                            _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "You already have this weapon", weaponSelectText, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(weaponIcons[11], weaponBox, Color.White);
                            _spriteBatch.DrawString(titleFont, "Inverted Spear", weaponSelectText, Color.White);
                        }
                    }
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
