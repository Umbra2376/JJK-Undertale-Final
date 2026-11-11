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
        private Matrix _transformMatrix;
        Texture2D titleImg, dodgeBoxT, soulSpirteT, charHBarT, enemyHBarT, charCEBarT, emptyBox, punchSelection, weaponSelection, skillSelection, barTexture;
        SpriteFont titleFont, stagesFont;
        Song titleSong;
        bool punchSelectingB = true, weaponSelectingB, skillSelectingB, skillSelecting, stageSelecting = true, weaponSelecting, showSkillMenu, showWeaponMenu, showStagesMenu;
        bool stage1Selecting, stage2Selecting, stage3Selecting, stage4Selecting, stage5Selecting, stage6Selecting, stage7Selecting, stage8Selecting, stage9Selecting, stage10Selecting;
        bool stage2Unlock, stage3Unlock, stage4Unlock, stage5Unlock, stage6Unlock, stage7Unlock, stage8Unlock, stage9Unlock, stage10Unlock;
        Rectangle window, dodgeBoxR, soulSpriteR, charHR, charHBarR, charCEBarR, enemyHBarR, healthBox, battleMenu, skillBox, weaponBox;
        Rectangle stagesMenuR, skillsMenuR, weaponsMenuR;
        float titleO, titleTime, invincibilityTime;
        Vector2 openingText, controlsText, skipOption, startOption, skillSelectText, weaponSelectText;
        GamePadState gamePadCurrent, gamePadOld;
        charAttack playerAttack;
        ParticleEngine particleEngine;
        List<Texture2D> particles;
        enum Screen
        {
            Title, Opening, Menu, Battle, Select, BattleRewards, Win, Lose, Quit
        }
        Screen screen = Screen.Title;
        enum Battle
        {
            b1, b2, b3, b4, b5, b6, b7, b8, b9, b10
        }
        Battle battle;
        List<string> skillList = new List<string>();
        List<string> equippedSkills = new List<string>();
        int skillMenuIndex = 0;
        List<string> weaponList = new List<string>();
        int weaponMenuIndex = 0;
        string selectedWeapon = "";
        List<Texture2D> skillIcons = new List<Texture2D>();
        List<Texture2D> weaponIcons = new List<Texture2D>();
        List<Texture2D> enemyTextures = new List<Texture2D>();
        List<Texture2D> backgrounds = new List<Texture2D>();
        List<Texture2D> punchAnimation = new List<Texture2D>();
        List<Texture2D> weaponAnimation = new List<Texture2D>();
        List<Texture2D> BFanimation = new List<Texture2D>();
        List<Texture2D> menuOptions = new List<Texture2D>();
        List<Texture2D> enemyAttacks = new List<Texture2D>();
        List<b1Projectile> b1Projectiles = new List<b1Projectile>();
        List<b2Projectile> b2Projectiles = new List<b2Projectile>();

        int battleSkillIndex = 0;
        bool choosingBattleSkill = false;
        float speedBoostTime = 0f;
        float defenceBoostTime = 0f;


        float b2AttackTime = 8f;
        float b2AttackInterval = 2.5f;
        float b2AttackAngle = 0f;

        float projectileSpeed = 300f;
        float projectileTime = 0f;
        float projectileInterval = 0.8f;
        float particleTime = 0f;

        float soulSpeed = 300f;
        float dodgeTime = 1f;
        int charHealth = 100, charHealthMax = 100;
        bool dodgePhase = true;
        int enemyHealth = 100, enemyHealthMax = 100;
        Rectangle enemyHR;
        Rectangle punchOptionR = new Rectangle(50, 550, 400, 120);
        Rectangle skillOptionR = new Rectangle(500, 550, 400, 120);
        Rectangle weaponOptionR = new Rectangle(950, 550, 400, 120);

        int obtainedSkill, obtainedWeapon, skillRarity, weaponRarity;
        bool rewardsGenerated = false, rewardsAccepted = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            particles = new List<Texture2D>();

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
            healthBox = new Rectangle(-40, 750, 1675, 250);
            battleMenu = new Rectangle(-60, 400, 1625, 650);
            enemyHR = new Rectangle(525, -15, 450, 150);
            enemyHBarR = new Rectangle(537, 48, 425, 30);

            skillBox = new Rectangle(100, 200, 500, 500);
            weaponBox = new Rectangle(800, 200, 500, 500);
            skillSelectText = new Vector2(140, 750);
            weaponSelectText = new Vector2(900, 750);

            stagesMenuR = new Rectangle(60, 20, 500, 230);
            skillsMenuR = new Rectangle(60, 340, 500, 230);
            weaponsMenuR = new Rectangle(60, 640, 500, 230);

            soulSpriteR = new Rectangle(dodgeBoxR.Center.X - 15, dodgeBoxR.Center.Y - 15, 50, 50);
            base.Initialize();

            particleEngine = new ParticleEngine(particles, Vector2.Zero, false, true);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _transformMatrix = Matrix.CreateScale(GraphicsDevice.Viewport.Width / (float)window.Width, GraphicsDevice.Viewport.Height / (float)window.Height, 1f);

            particles.Add(Content.Load<Texture2D>("tile"));

            titleImg = Content.Load<Texture2D>("titleScreen");
            titleSong = Content.Load<Song>("Sounds/titleTheme");
            titleFont = Content.Load<SpriteFont>("titleFont");
            stagesFont = Content.Load<SpriteFont>("stagesFont");
            dodgeBoxT = Content.Load<Texture2D>("dodgeBox");
            soulSpirteT = Content.Load<Texture2D>("soulSprite");
            emptyBox = Content.Load<Texture2D>("emptySlot");

            enemyTextures.Add(Content.Load<Texture2D>("Enemies/firstBattleCurse"));
            enemyTextures.Add(Content.Load<Texture2D>("Enemies/Grasshopper"));

            charHBarT = Content.Load<Texture2D>("charHealth");
            charCEBarT = Content.Load<Texture2D>("CEbar");
            barTexture = Content.Load<Texture2D>("tile");
            punchSelection = Content.Load<Texture2D>("Options/punchOption");
            weaponSelection = Content.Load<Texture2D>("Options/weaponOption");
            skillSelection = Content.Load<Texture2D>("Options/skillOption");
            enemyHBarT = Content.Load<Texture2D>("enemyHealth");

            backgrounds.Add(Content.Load<Texture2D>("Backgrounds/menuBg"));
            backgrounds.Add(Content.Load<Texture2D>("Backgrounds/battle1Bg"));
            backgrounds.Add(Content.Load<Texture2D>("Backgrounds/b2Background"));

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

            menuOptions.Add(Content.Load<Texture2D>("MenuOptions/SkillsMenu"));
            menuOptions.Add(Content.Load<Texture2D>("MenuOptions/StagesMenu"));
            menuOptions.Add(Content.Load<Texture2D>("MenuOptions/WeaponsMenu"));
            for (int i = 1; i < 11; i++)
                menuOptions.Add(Content.Load<Texture2D>("MenuOptions/stage" + i + "Selection"));
            menuOptions.Add(Content.Load<Texture2D>("MenuOptions/lockedStage"));

            enemyAttacks.Add(Content.Load<Texture2D>("EnemyMoves/b1Attack"));
            enemyAttacks.Add(Content.Load<Texture2D>("EnemyMoves/b2Attack"));

            for (int i = 1; i < 6; i++)
                punchAnimation.Add(Content.Load<Texture2D>("charAttacks/punch" + i));
            for (int i = 1; i < 6; i++)
                BFanimation.Add(Content.Load<Texture2D>("charAttacks/BF" + i));
            for (int i = 1; i < 6; i++)
                weaponAnimation.Add(Content.Load<Texture2D>("charAttacks/weapon" + i));

            playerAttack = new charAttack();
            playerAttack.AddAttack("punch", punchAnimation, 20);
            playerAttack.AddAttack("blackFlash", BFanimation, 500);

            playerAttack.AddAttack("slaughterDemon", weaponAnimation, 25);
            playerAttack.AddAttack("handSword", weaponAnimation, 30);
            playerAttack.AddAttack("basicKatana", weaponAnimation, 35);
            playerAttack.AddAttack("cursedSpear", weaponAnimation, 40);
            playerAttack.AddAttack("ratioKnife", weaponAnimation, 50);
            playerAttack.AddAttack("cursedSword", weaponAnimation, 60);
            playerAttack.AddAttack("mallet", weaponAnimation, 45);
            playerAttack.AddAttack("dragonBone", weaponAnimation, 75);
            playerAttack.AddAttack("splitSoul", weaponAnimation, 80);
            playerAttack.AddAttack("festeringLife", weaponAnimation, 90);
            playerAttack.AddAttack("playfulCloud", weaponAnimation, 100);
            playerAttack.AddAttack("invertedSpear", weaponAnimation, 110);
            RegisterSkills();
            MediaPlayer.Volume = 0.6f;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0)
                screen = Screen.Quit;
            if (screen == Screen.Quit && gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                Exit();
            else if (screen == Screen.Quit && gamePadCurrent.Buttons.A == ButtonState.Pressed && gamePadOld.Buttons.A == ButtonState.Released)
                screen = Screen.Menu;

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
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                speedBoostTime = Math.Max(0f, speedBoostTime - dt);
                defenceBoostTime = Math.Max(0f, defenceBoostTime - dt);
                float currentSpeed = speedBoostTime > 0f ? soulSpeed * 1.5f : soulSpeed;

                float battleTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                particleEngine.Update();
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

                if (battle == Battle.b1 && dodgePhase)
                {
                    Vector2 movement = gamePadCurrent.ThumbSticks.Left;
                    movement.Y *= -1;

                    if (movement.LengthSquared() > 1f)
                        movement.Normalize();

                    soulSpriteR.X += (int)(movement.X * currentSpeed * battleTime);
                    soulSpriteR.Y += (int)(movement.Y * currentSpeed * battleTime);

                    soulSpriteR.X = Math.Clamp(soulSpriteR.X, dodgeBoxR.Left, dodgeBoxR.Right - 50);
                    soulSpriteR.Y = Math.Clamp(soulSpriteR.Y, dodgeBoxR.Top, dodgeBoxR.Bottom - 50);

                    projectileTime += battleTime;
                    if (projectileTime >= projectileInterval)
                    {
                        Spawnb1Projectiles();
                        projectileTime = 0f;
                    }

                    for (int i = b1Projectiles.Count - 1; i >= 0; i--)
                    {
                        b1Projectile projectile = b1Projectiles[i];
                        projectile.Position += projectile.Direction * projectileSpeed * battleTime;
                        Rectangle projectileR = new Rectangle((int)projectile.Position.X - 12, (int)projectile.Position.Y - 12, 24, 24);
                        if (projectileR.Intersects(soulSpriteR) && invincibilityTime <= 0f)
                        {
                            int incomingDamage = 5;
                            if (defenceBoostTime > 0f)
                                incomingDamage = (int)Math.Ceiling(incomingDamage * 0.5f);
                            charHealth -= incomingDamage;
                            invincibilityTime = 1f;
                            b1Projectiles.RemoveAt(i);
                            particleEngine.Enabled = true;
                            particleEngine.EmitterLocation = new Vector2(soulSpriteR.Center.X, soulSpriteR.Center.Y);
                            particleTime = 0;
                            continue;
                        }
                        if (particleEngine.Enabled)
                        {
                            particleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (particleTime >= 0.1f)
                                particleEngine.Enabled = false;
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
                        titleTime = 0f;
                    }
                }
                else if (battle == Battle.b2 && dodgePhase)
                {
                    Vector2 movement = gamePadCurrent.ThumbSticks.Left;
                    movement.Y *= -1;

                    if (movement.LengthSquared() > 1f)
                        movement.Normalize();

                    soulSpriteR.X += (int)(movement.X * currentSpeed * battleTime);
                    soulSpriteR.Y += (int)(movement.Y * currentSpeed * battleTime);

                    soulSpriteR.X = Math.Clamp(soulSpriteR.X, dodgeBoxR.Left, dodgeBoxR.Right - 20);
                    soulSpriteR.Y = Math.Clamp(soulSpriteR.Y, dodgeBoxR.Top, dodgeBoxR.Bottom - 20);

                    b2AttackTime += battleTime;
                    if (b2AttackTime >= b2AttackInterval)
                    {
                        SpawnB2Projectiles();
                        b2AttackTime = 0f;
                    }

                    for (int i = b2Projectiles.Count - 1; i >= 0; i--)
                    {
                        b2Projectile projectile = b2Projectiles[i];
                        projectile.Update(gameTime);

                        Rectangle b2SoulHitbox = new Rectangle(soulSpriteR.X + 4, soulSpriteR.Y + 4, 12, 20);

                        if (projectile.Hitbox.Intersects(b2SoulHitbox) && invincibilityTime <= 0f)
                        {
                            int incomingDamage = 10;
                            if (defenceBoostTime > 0f)
                                incomingDamage = (int)Math.Ceiling(incomingDamage * 0.5f);
                            charHealth -= incomingDamage;
                            invincibilityTime = 1f;

                            b2Projectiles.RemoveAt(i);
                            continue;
                        }

                        if (projectile.Position.X < dodgeBoxR.Left - 300 || projectile.Position.X > dodgeBoxR.Right + 300 || projectile.Position.Y < dodgeBoxR.Top - 300 || projectile.Position.Y > dodgeBoxR.Bottom + 300)
                            b2Projectiles.RemoveAt(i);
                    }

                    if (invincibilityTime > 0f)
                        invincibilityTime -= battleTime;

                    dodgeTime -= battleTime;
                    if (charHealth <= 0)
                        screen = Screen.Lose;
                    else if (dodgeTime <= 0f)
                    {
                        dodgePhase = false;
                        b2Projectiles.Clear();
                        screen = Screen.Select;
                        titleTime = 0f;
                    }
                }
            }

            else if (screen == Screen.Select)
            {
                float stickX = gamePadCurrent.ThumbSticks.Left.X;
                float oldStickX = gamePadOld.ThumbSticks.Left.X;

                if (!playerAttack.IsAttacking && !choosingBattleSkill && stickX > 0.5f && oldStickX <= 0.5f)
                {
                    if (punchSelectingB)
                    {
                        punchSelectingB = false;
                        skillSelectingB = true;
                    }
                    else if (skillSelectingB)
                    {
                        skillSelectingB = false;
                        weaponSelectingB = true;
                    }
                }
                else if (!playerAttack.IsAttacking && !choosingBattleSkill && stickX < -0.5f && oldStickX >= -0.5f)
                {
                    if (weaponSelectingB)
                    {
                        weaponSelectingB = false;
                        skillSelectingB = true;
                    }
                    else if (skillSelectingB)
                    {
                        skillSelectingB = false;
                        punchSelectingB = true;
                    }
                }

                bool confirmPressed = gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released;
                if (choosingBattleSkill && gamePadCurrent.Buttons.A == ButtonState.Pressed &&
                    gamePadOld.Buttons.A == ButtonState.Released)
                    choosingBattleSkill = false;

                if (!playerAttack.IsAttacking)
                {
                    if (punchSelectingB && confirmPressed)
                        playerAttack.StartPunch();
                    else if (weaponSelectingB && confirmPressed)
                    {
                        if (!string.IsNullOrEmpty(selectedWeapon))
                            playerAttack.StartWeapon(selectedWeapon);
                    }
                    else if (skillSelectingB)
                    {
                        if (!choosingBattleSkill && confirmPressed)
                        {
                            if (equippedSkills.Count > 0)
                            {
                                choosingBattleSkill = true;
                                battleSkillIndex = 0;
                            }
                        }
                        else if (choosingBattleSkill)
                        {
                            float stickY = gamePadCurrent.ThumbSticks.Left.Y;
                            float oldStickY = gamePadOld.ThumbSticks.Left.Y;

                            if (stickY < -0.5f && oldStickY >= -0.5f)
                                battleSkillIndex = (battleSkillIndex + 1) % equippedSkills.Count;
                            else if (stickY > 0.5f && oldStickY <= 0.5f)
                                battleSkillIndex = (battleSkillIndex - 1 + equippedSkills.Count) % equippedSkills.Count;

                            if (confirmPressed)
                            {
                                string skill = equippedSkills[battleSkillIndex];
                                if (playerAttack.StartSkill(skill))
                                    choosingBattleSkill = false;
                            }
                        }
                    }
                }



                if (playerAttack.Update(gameTime))
                {
                    enemyHealth -= playerAttack.Damage;
                    enemyHealth = Math.Max(0, enemyHealth);

                    switch (playerAttack.CurrentEffect)
                    {
                        case charAttack.EffectType.Heal:
                            charHealth = Math.Min(charHealthMax, charHealth + 30);
                            break;

                        case charAttack.EffectType.Speed:
                            speedBoostTime = 15f;
                            break;

                        case charAttack.EffectType.Defence:
                            defenceBoostTime = 15f;
                            break;
                    }

                    if (enemyHealth <= 0)
                    {
                        if (battle == Battle.b1)
                        {
                            stage2Unlock = true;
                            charHealthMax = 150; 
                        }
                        rewardsGenerated = false;
                        rewardsAccepted = false;
                        screen = Screen.BattleRewards;
                    }
                    else
                    {
                        choosingBattleSkill = false;
                        screen = Screen.Battle;
                        dodgePhase = true;
                        dodgeTime = 15f;
                        projectileTime = 0f;

                        b1Projectiles.Clear();
                        b2Projectiles.Clear();

                        b2AttackTime = 0f;
                        b2AttackAngle = 0f;
                    }
                }

            }
            else if (screen == Screen.BattleRewards)
            {
                if (!rewardsGenerated)
                    GenerateRewards();
                AddSkillandWeapons();
                if (rewardsGenerated && rewardsAccepted && gamePadCurrent.Buttons.BigButton == ButtonState.Pressed && gamePadOld.Buttons.BigButton == ButtonState.Released)
                    screen = Screen.Menu;
            }
            else if (screen == Screen.Menu)
            {
                if (!showSkillMenu && !showStagesMenu && !showWeaponMenu)
                    MoveInMenu();
                if (stageSelecting && gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                {
                    stageSelecting = false;
                    showStagesMenu = true;
                    stage1Selecting = true;
                }
                else if (weaponSelecting && gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                {
                    showWeaponMenu = true;
                    weaponSelecting = false;
                    weaponMenuIndex = 0;
                }
                else if (skillSelecting && gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                {
                    showSkillMenu = true;
                    skillSelecting = false;
                    skillMenuIndex = 0;
                }
                else if (showStagesMenu)
                {
                    if (gamePadCurrent.Buttons.A == ButtonState.Pressed && gamePadOld.Buttons.A == ButtonState.Released)
                    {
                        showStagesMenu = false;
                        stageSelecting = true;
                    }
                    MoveStagesSelection();
                    if (stage1Selecting && gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                    {

                        showStagesMenu = false;
                        stage1Selecting = false;
                        stageSelecting = true;
                        screen = Screen.Battle;
                        battle = Battle.b1;
                        dodgePhase = true;
                        dodgeTime = 15f;
                        projectileTime = 0f;
                        invincibilityTime = 0f;

                        b1Projectiles.Clear();
                        enemyHealthMax = 100;
                        charHealth = charHealthMax;
                        enemyHealth = enemyHealthMax;
                        playerAttack.Reset();
                        choosingBattleSkill = false;
                        speedBoostTime = 0f;
                        defenceBoostTime = 0f;
                    }
                    if (stage2Selecting && gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                    {

                        showStagesMenu = false;
                        stage2Selecting = false;
                        stageSelecting = true;
                        screen = Screen.Battle;
                        battle = Battle.b2;
                        dodgePhase = true;
                        dodgeTime = 15f;
                        projectileTime = 0f;
                        invincibilityTime = 0f;

                        b2Projectiles.Clear();
                        enemyHealthMax = 150;
                        charHealth = charHealthMax;
                        enemyHealth = enemyHealthMax;
                        playerAttack.Reset();
                        choosingBattleSkill = false;
                        speedBoostTime = 0f;
                        defenceBoostTime = 0f;
                    }
                }
                else if (showWeaponMenu)
                {
                    if (gamePadCurrent.Buttons.A == ButtonState.Pressed && gamePadOld.Buttons.A == ButtonState.Released)
                    {
                        showWeaponMenu = false;
                        weaponSelecting = true;
                    }

                    if (weaponList.Count > 0)
                    {
                        if (gamePadCurrent.ThumbSticks.Left.X > 0.5f && gamePadOld.ThumbSticks.Left.X <= 0.5f)
                        {
                            weaponMenuIndex++;

                            if (weaponMenuIndex >= weaponList.Count)
                                weaponMenuIndex = 0;
                        }
                        else if (gamePadCurrent.ThumbSticks.Left.X < -0.5f && gamePadOld.ThumbSticks.Left.X >= -0.5f)
                        {
                            weaponMenuIndex--;

                            if (weaponMenuIndex < 0)
                                weaponMenuIndex = weaponList.Count - 1;
                        }

                        if (gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f)
                        {
                            weaponMenuIndex += 4;

                            if (weaponMenuIndex >= weaponList.Count)
                                weaponMenuIndex = 0;
                        }
                        else if (gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f)
                        {
                            weaponMenuIndex -= 4;

                            if (weaponMenuIndex < 0)
                                weaponMenuIndex = weaponList.Count - 1;
                        }

                        if (gamePadCurrent.Buttons.B == ButtonState.Pressed &&
                            gamePadOld.Buttons.B == ButtonState.Released)
                        {
                            string weapon = weaponList[weaponMenuIndex];
                            selectedWeapon = selectedWeapon == weapon ? "" : weapon;
                        }
                    }
                }
                else if (showSkillMenu)
                {
                    if (gamePadCurrent.Buttons.A == ButtonState.Pressed && gamePadOld.Buttons.A == ButtonState.Released)
                    {
                        showSkillMenu = false;
                        skillSelecting = true;
                    }

                    if (skillList.Count > 0)
                    {
                        if (gamePadCurrent.ThumbSticks.Left.X > 0.5f && gamePadOld.ThumbSticks.Left.X <= 0.5f)
                        {
                            skillMenuIndex++;

                            if (skillMenuIndex >= skillList.Count)
                                skillMenuIndex = 0;
                        }
                        else if (gamePadCurrent.ThumbSticks.Left.X < -0.5f && gamePadOld.ThumbSticks.Left.X >= -0.5f)
                        {
                            skillMenuIndex--;

                            if (skillMenuIndex < 0)
                                skillMenuIndex = skillList.Count - 1;
                        }
                        if (gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f)
                        {
                            skillMenuIndex += 4;

                            if (skillMenuIndex >= skillList.Count)
                                skillMenuIndex = 0;
                        }
                        else if (gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f)
                        {
                            skillMenuIndex -= 4;

                            if (skillMenuIndex < 0)
                                skillMenuIndex = skillList.Count - 1;
                        }
                        if (gamePadCurrent.Buttons.B == ButtonState.Pressed && gamePadOld.Buttons.B == ButtonState.Released)
                        {
                            string skill = skillList[skillMenuIndex];

                            if (equippedSkills.Contains(skill))
                            {
                                equippedSkills.Remove(skill);
                            }
                            else if (equippedSkills.Count < 3)
                            {
                                equippedSkills.Add(skill);
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
            _spriteBatch.Begin(transformMatrix: _transformMatrix);

            if (screen == Screen.Title)
            {
                _spriteBatch.Draw(titleImg, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White * titleO);
                _spriteBatch.DrawString(titleFont, "Press START to start", startOption, Color.White);
            }
            else if (screen == Screen.Quit)
            {
                _spriteBatch.DrawString(titleFont, "You are trying to quit. Are you sure?", new Vector2(350, 100), Color.White);
                _spriteBatch.DrawString(titleFont, "Press A to quit and B to go back to menu", new Vector2(285, 650), Color.White);
            }
            else if (screen == Screen.Opening)
            {
                DrawOpeningText();
            }
            else if (screen == Screen.Battle)
            {
                if (battle == Battle.b1)
                {
                    _spriteBatch.Draw(backgrounds[1], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    _spriteBatch.Draw(enemyTextures[0], new Rectangle(550, 100, 400, 300), Color.White);
                    _spriteBatch.Draw(barTexture, enemyHBarR, Color.DarkRed);
                    int enemyBarWidth = (int)(enemyHBarR.Width * ((float)enemyHealth / enemyHealthMax));
                    if (enemyBarWidth > 0)
                        _spriteBatch.Draw(barTexture, new Rectangle(enemyHBarR.X, enemyHBarR.Y, enemyBarWidth, enemyHBarR.Height), Color.Red);
                    _spriteBatch.Draw(enemyHBarT, enemyHR, Color.White);
                    _spriteBatch.DrawString(titleFont, "Enemy HP: " + enemyHealth + "/" + enemyHealthMax, new Vector2(50, 35), Color.White);
                    _spriteBatch.Draw(dodgeBoxT, dodgeBoxR, Color.White * titleO);
                    foreach (b1Projectile b1projectile in b1Projectiles)
                    {
                        _spriteBatch.Draw(enemyAttacks[0], b1projectile.Position, null, Color.White, b1projectile.Rotation, new Vector2(enemyAttacks[0].Width / 2f, enemyAttacks[0].Height / 2f), 0.5f, SpriteEffects.None, 0f);
                    }
                    if (invincibilityTime <= 0f || (int)(invincibilityTime * 10) % 2 == 0)
                        _spriteBatch.Draw(soulSpirteT, soulSpriteR, Color.White);
                    particleEngine.Draw(_spriteBatch);
                    _spriteBatch.Draw(emptyBox, healthBox, Color.White);
                    _spriteBatch.Draw(barTexture, charHBarR, Color.DarkRed);
                    int playerBarWidth = (int)(charHBarR.Width * ((float)charHealth / charHealthMax));
                    if (playerBarWidth > 0)
                        _spriteBatch.Draw(barTexture, new Rectangle(charHBarR.X, charHBarR.Y, playerBarWidth, charHBarR.Height), Color.Green);
                    _spriteBatch.DrawString(titleFont, "HP: " + charHealth + "/" + charHealthMax, new Vector2(50, 800), Color.White);
                    _spriteBatch.Draw(charHBarT, charHR, Color.White);
                }
                else if (battle == Battle.b2)
                {
                    _spriteBatch.Draw(backgrounds[2], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    _spriteBatch.Draw(enemyTextures[1], new Rectangle(550, 100, 400, 300), Color.White);
                    _spriteBatch.Draw(barTexture, enemyHBarR, Color.DarkRed);
                    int enemyBarWidth = (int)(enemyHBarR.Width * ((float)enemyHealth / enemyHealthMax));
                    if (enemyBarWidth > 0)
                        _spriteBatch.Draw(barTexture, new Rectangle(enemyHBarR.X, enemyHBarR.Y, enemyBarWidth, enemyHBarR.Height), Color.Red);
                    _spriteBatch.Draw(enemyHBarT, enemyHR, Color.White);
                    _spriteBatch.DrawString(titleFont, "Enemy HP: " + enemyHealth + "/" + enemyHealthMax, new Vector2(50, 35), Color.White);
                    _spriteBatch.Draw(dodgeBoxT, dodgeBoxR, Color.White * titleO);
                    foreach (b2Projectile projectile in b2Projectiles)
                    {
                        projectile.Draw(_spriteBatch);
                    }
                    if (invincibilityTime <= 0f || (int)(invincibilityTime * 10) % 2 == 0)
                        _spriteBatch.Draw(soulSpirteT, new Rectangle(soulSpriteR.X, soulSpriteR.Y, 20, 20), Color.White);
                    particleEngine.Draw(_spriteBatch);
                    _spriteBatch.Draw(emptyBox, healthBox, Color.White);
                    _spriteBatch.Draw(barTexture, charHBarR, Color.DarkRed);
                    int playerBarWidth = (int)(charHBarR.Width * ((float)charHealth / charHealthMax));
                    if (playerBarWidth > 0)
                        _spriteBatch.Draw(barTexture, new Rectangle(charHBarR.X, charHBarR.Y, playerBarWidth, charHBarR.Height), Color.Green);
                    _spriteBatch.DrawString(titleFont, "HP: " + charHealth + "/" + charHealthMax, new Vector2(50, 800), Color.White);
                    _spriteBatch.Draw(charHBarT, charHR, Color.White);
                }
            }
            else if (screen == Screen.Select)
            {
                if (battle == Battle.b1)
                {
                    _spriteBatch.Draw(backgrounds[1], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
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
                    if (punchSelectingB)
                        _spriteBatch.Draw(punchSelection, punchOptionR, Color.Yellow);
                    else
                        _spriteBatch.Draw(punchSelection, punchOptionR, Color.White);
                    if (weaponSelectingB)
                        _spriteBatch.Draw(weaponSelection, weaponOptionR, Color.Yellow);
                    else
                        _spriteBatch.Draw(weaponSelection, weaponOptionR, Color.White);
                    if (skillSelectingB)
                        _spriteBatch.Draw(skillSelection, skillOptionR, Color.Yellow);
                    else
                        _spriteBatch.Draw(skillSelection, skillOptionR, Color.White);
                    playerAttack.Draw(_spriteBatch);
                    DrawBattleSkillSelector();
                }
                else if (battle == Battle.b2)
                {
                    _spriteBatch.Draw(backgrounds[2], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    _spriteBatch.Draw(enemyTextures[1], new Rectangle(550, 100, 400, 300), Color.White);
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
                    if (punchSelectingB)
                        _spriteBatch.Draw(punchSelection, punchOptionR, Color.Yellow);
                    else
                        _spriteBatch.Draw(punchSelection, punchOptionR, Color.White);
                    _spriteBatch.Draw(weaponSelection, weaponOptionR, weaponSelectingB ? Color.Yellow : Color.White);
                    _spriteBatch.Draw(skillSelection, skillOptionR, skillSelectingB ? Color.Yellow : Color.White);
                    playerAttack.Draw(_spriteBatch);
                    DrawBattleSkillSelector();
                }
            }
            else if (screen == Screen.BattleRewards)
            {
                if (battle == Battle.b1)
                    _spriteBatch.DrawString(titleFont, "You have defeated the grade 4 curse and got rewards!", new Vector2(150, 35), Color.White);
                else if (battle == Battle.b2)
                    _spriteBatch.DrawString(titleFont, "You have defeated the Grasshopper Curse and got rewards!", new Vector2(140, 35), Color.White);
                _spriteBatch.DrawString(titleFont, "Press B to accept rewards then START to continue !", new Vector2(200, 90), Color.White);
                DrawRewards();
            }
            else if (screen == Screen.Menu)
            {
                _spriteBatch.Draw(backgrounds[0], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                if (skillSelecting)
                    _spriteBatch.Draw(menuOptions[0], skillsMenuR, Color.Yellow);
                else
                    _spriteBatch.Draw(menuOptions[0], skillsMenuR, Color.White);
                if (stageSelecting)
                    _spriteBatch.Draw(menuOptions[1], stagesMenuR, Color.Yellow);
                else
                    _spriteBatch.Draw(menuOptions[1], stagesMenuR, Color.White);
                if (weaponSelecting)
                    _spriteBatch.Draw(menuOptions[2], weaponsMenuR, Color.Yellow);
                else
                    _spriteBatch.Draw(menuOptions[2], weaponsMenuR, Color.White);
                if (showStagesMenu)
                {
                    if (stage1Selecting)
                        _spriteBatch.Draw(menuOptions[3], new Rectangle(700, 25, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[3], new Rectangle(700, 25, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Grade 4 Curse", new Vector2(750, 160), Color.White);
                    if (stage2Selecting)
                        _spriteBatch.Draw(menuOptions[4], new Rectangle(700, 200, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[4], new Rectangle(700, 200, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Grasshopper Curse", new Vector2(720, 335), Color.White);
                    if (stage3Selecting)
                        _spriteBatch.Draw(menuOptions[5], new Rectangle(700, 375, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[5], new Rectangle(700, 375, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Brothers Curses", new Vector2(730, 510), Color.White);
                    if (stage4Selecting)
                        _spriteBatch.Draw(menuOptions[6], new Rectangle(700, 550, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[6], new Rectangle(700, 550, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Choso", new Vector2(800, 685), Color.White);
                    if (stage5Selecting)
                        _spriteBatch.Draw(menuOptions[7], new Rectangle(700, 725, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[7], new Rectangle(700, 725, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Agito", new Vector2(800, 855), Color.White);
                    if (stage6Selecting)
                        _spriteBatch.Draw(menuOptions[8], new Rectangle(1150, 25, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[8], new Rectangle(1150, 25, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Mahito", new Vector2(1255, 160), Color.White);
                    if (stage7Selecting)
                        _spriteBatch.Draw(menuOptions[9], new Rectangle(1150, 200, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[9], new Rectangle(1150, 200, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Jogo", new Vector2(1255, 335), Color.White);
                    if (stage8Selecting)
                        _spriteBatch.Draw(menuOptions[10], new Rectangle(1150, 375, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[10], new Rectangle(1150, 375, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Kenj aku", new Vector2(1235, 510), Color.White);
                    if (stage9Selecting)
                        _spriteBatch.Draw(menuOptions[11], new Rectangle(1150, 550, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[11], new Rectangle(1150, 550, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Mahoraga", new Vector2(1235, 685), Color.White);
                    if (stage10Selecting)
                        _spriteBatch.Draw(menuOptions[12], new Rectangle(1150, 725, 300, 130), Color.Yellow);
                    else
                        _spriteBatch.Draw(menuOptions[12], new Rectangle(1150, 725, 300, 130), Color.White);
                    _spriteBatch.DrawString(stagesFont, "Sukuna", new Vector2(1220, 855), Color.White);
                    if (!stage3Unlock)
                        _spriteBatch.Draw(menuOptions[13], new Rectangle(700, 375, 300, 130), Color.White * 0.7f);
                    if (!stage4Unlock)
                        _spriteBatch.Draw(menuOptions[13], new Rectangle(700, 550, 300, 130), Color.White * 0.7f);
                    if (!stage5Unlock)
                        _spriteBatch.Draw(menuOptions[13], new Rectangle(700, 725, 300, 130), Color.White * 0.7f);
                    if (!stage6Unlock)
                        _spriteBatch.Draw(menuOptions[13], new Rectangle(1150, 25, 300, 130), Color.White * 0.7f);
                    if (!stage7Unlock)
                        _spriteBatch.Draw(menuOptions[13], new Rectangle(1150, 200, 300, 130), Color.White * 0.7f);
                    if (!stage8Unlock)
                        _spriteBatch.Draw(menuOptions[13], new Rectangle(1150, 375, 300, 130), Color.White * 0.7f);
                    if (!stage9Unlock)
                        _spriteBatch.Draw(menuOptions[13], new Rectangle(1150, 550, 300, 130), Color.White * 0.7f);
                    if (!stage10Unlock)
                        _spriteBatch.Draw(menuOptions[13], new Rectangle(1150, 725, 300, 130), Color.White * 0.7f);
                }
                else if (showWeaponMenu)
                    DrawWeaponMenu();
                else if (showSkillMenu)
                    DrawSkillMenu();
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
        void Spawnb1Projectiles()
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
        void SpawnB2Projectiles()
        {
            Vector2 soulCenter = new Vector2(soulSpriteR.X + 10, soulSpriteR.Y + 10);

            int projectileCount = 12;
            float radius = 250f;

            for (int i = 0; i < projectileCount; i++)
            {
                float angle = MathHelper.TwoPi * i / projectileCount + b2AttackAngle;

                Vector2 spawnPosition = soulCenter + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                Vector2 direction = soulCenter - spawnPosition;
                direction.Normalize();

                float rotation = (float)Math.Atan2(direction.Y, direction.X);

                b2Projectiles.Add(new b2Projectile(spawnPosition, direction, rotation, enemyAttacks[1]));
            }
            b2AttackAngle += 0.25f;
        }
        void AddSkillandWeapons()
        {
            if (gamePadCurrent.Buttons.A == ButtonState.Pressed && gamePadOld.Buttons.A == ButtonState.Released)
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
                    else if (obtainedWeapon == 2 && !weaponList.Contains("splitSoul"))
                        weaponList.Add("splitSoul");
                    else if (obtainedWeapon == 3 && !weaponList.Contains("festeringLife"))
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
                rewardsAccepted = true;
            }
        }
        void DrawRewards()
        {
            _spriteBatch.Draw(dodgeBoxT, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            if (skillRarity <= 55)
            {
                if (obtainedSkill == 1)
                {
                    if (skillList.Contains("explodingFlesh"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
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
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
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
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[2], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Miracle", skillSelectText, Color.White);
                    }
                }
            }
            else if (skillRarity <= 80 && skillRarity > 55)
            {
                if (obtainedSkill == 1)
                {
                    if (skillList.Contains("ice"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[3], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Ice", skillSelectText, Color.Blue);
                    }
                }
                else if (obtainedSkill == 2)
                {
                    if (skillList.Contains("blood"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[4], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Blood", skillSelectText, Color.Blue);
                    }
                }
                else if (obtainedSkill == 3)
                {
                    if (skillList.Contains("projection"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[5], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Projection", skillSelectText, Color.Blue);
                    }
                }
                else if (obtainedSkill == 4)
                {
                    if (skillList.Contains("ratio"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[6], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Ratio", skillSelectText, Color.Blue);
                    }
                }
            }
            else if (skillRarity <= 95 && skillRarity > 80)
            {
                if (obtainedSkill == 1)
                {
                    if (skillList.Contains("RCT"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[7], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "RCT", skillSelectText, Color.Purple);
                    }
                }
                else if (obtainedSkill == 2)
                {
                    if (skillList.Contains("curseControl"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[8], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Curse Control", skillSelectText, Color.Purple);
                    }
                }
                else if (obtainedSkill == 3)
                {
                    if (skillList.Contains("bugArmour"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[9], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Bug Armour", skillSelectText, Color.Purple);
                    }
                }
                else if (obtainedSkill == 4)
                {
                    if (skillList.Contains("graniteBlast"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[10], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Granite Blast", skillSelectText, Color.Purple);
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
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[11], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Dismantle", skillSelectText, Color.Yellow);
                    }
                }
                else if (obtainedSkill == 2)
                {
                    if (skillList.Contains("cleave"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[12], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Cleave", skillSelectText, Color.Yellow);
                    }
                }
                else if (obtainedSkill == 3)
                {
                    if (skillList.Contains("beastAmber"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[13], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Beast Amber", skillSelectText, Color.Yellow);
                    }
                }
                else if (obtainedSkill == 4)
                {
                    if (skillList.Contains("blue"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[14], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Blue", skillSelectText, Color.Yellow);
                    }
                }
                else if (obtainedSkill == 5)
                {
                    if (skillList.Contains("red"))
                    {
                        _spriteBatch.Draw(emptyBox, skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this skill", new Vector2(skillSelectText.X - 100, skillSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(skillIcons[15], skillBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Red", skillSelectText, Color.Yellow);
                    }
                }
            }
            if (weaponRarity <= 55)
            {
                if (obtainedWeapon == 1)
                {
                    if (weaponList.Contains("slaughterDemon"))
                    {
                        _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
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
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
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
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
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
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(weaponIcons[3], weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Cursed Spear", weaponSelectText, Color.White);
                    }
                }
            }
            else if (weaponRarity <= 80 && weaponRarity > 55)
            {
                if (obtainedWeapon == 1)
                {
                    if (weaponList.Contains("ratioKnife"))
                    {
                        _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(weaponIcons[4], weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Ratio Knife", weaponSelectText, Color.Blue);
                    }
                }
                else if (obtainedWeapon == 2)
                {
                    if (weaponList.Contains("cursedSword"))
                    {
                        _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(weaponIcons[5], weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Cursed Sword", weaponSelectText, Color.Blue);
                    }
                }
                else if (obtainedWeapon == 3)
                {
                    if (weaponList.Contains("mallet"))
                    {
                        _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(weaponIcons[6], weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Mallet", weaponSelectText, Color.Blue);
                    }
                }
            }
            else if (weaponRarity <= 95 && weaponRarity > 80)
            {
                if (obtainedWeapon == 1)
                {
                    if (weaponList.Contains("dragonBone"))
                    {
                        _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(weaponIcons[7], weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Dragon Bone", weaponSelectText, Color.Purple);
                    }
                }
                else if (obtainedWeapon == 2)
                {
                    if (weaponList.Contains("splitSoul"))
                    {
                        _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(weaponIcons[8], weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Split Soul", weaponSelectText, Color.Purple);
                    }
                }
                else if (obtainedWeapon == 3)
                {
                    if (weaponList.Contains("festeringLife"))
                    {
                        _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(weaponIcons[9], weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Festering Life", weaponSelectText, Color.Purple);
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
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(weaponIcons[10], weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Playful Cloud", weaponSelectText, Color.Yellow);
                    }
                }
                else if (obtainedWeapon == 2)
                {
                    if (weaponList.Contains("invertedSpear"))
                    {
                        _spriteBatch.Draw(emptyBox, weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "You already have this weapon", new Vector2(weaponSelectText.X - 100, weaponSelectText.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(weaponIcons[11], weaponBox, Color.White);
                        _spriteBatch.DrawString(titleFont, "Inverted Spear", weaponSelectText, Color.Yellow);
                    }
                }
            }
        }
        void GenerateRewards()
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
        }
        void MoveInMenu()
        {
            if (stageSelecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f)
            {
                stageSelecting = false;
                skillSelecting = true;
            }
            else if (skillSelecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f)
            {
                skillSelecting = false;
                weaponSelecting = true;
            }
            else if (skillSelecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f)
            {
                stageSelecting = true;
                skillSelecting = false;
            }
            else if (weaponSelecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f)
            {
                weaponSelecting = false;
                skillSelecting = true;
            }
        }
        void MoveStagesSelection()
        {
            if (stage1Selecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f)
            {
                stage1Selecting = false;
                stage2Selecting = true;
            }
            else if (stage2Selecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f && stage3Unlock)
            {
                stage2Selecting = false;
                stage3Selecting = true;
            }
            else if (stage3Selecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f && stage4Unlock)
            {
                stage3Selecting = false;
                stage4Selecting = true;
            }
            else if (stage4Selecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f && stage5Unlock)
            {
                stage4Selecting = false;
                stage5Selecting = true;
            }
            else if (stage5Selecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f && stage4Unlock)
            {
                stage5Selecting = false;
                stage4Selecting = true;
            }
            else if (stage4Selecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f && stage3Unlock)
            {
                stage4Selecting = false;
                stage3Selecting = true;
            }
            else if (stage3Selecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f)
            {
                stage3Selecting = false;
                stage2Selecting = true;
            }
            else if (stage2Selecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f)
            {
                stage2Selecting = false;
                stage1Selecting = true;
            }
            else if (stage1Selecting && gamePadCurrent.ThumbSticks.Left.X > 0f && gamePadOld.ThumbSticks.Left.X == 0f && stage6Unlock)
            {
                stage1Selecting = false;
                stage6Selecting = true;
            }
            else if (stage2Selecting && gamePadCurrent.ThumbSticks.Left.X > 0f && gamePadOld.ThumbSticks.Left.X == 0f && stage7Unlock)
            {
                stage2Selecting = false;
                stage7Selecting = true;
            }
            else if (stage3Selecting && gamePadCurrent.ThumbSticks.Left.X > 0f && gamePadOld.ThumbSticks.Left.X == 0f && stage8Unlock)
            {
                stage3Selecting = false;
                stage8Selecting = true;
            }
            else if (stage4Selecting && gamePadCurrent.ThumbSticks.Left.X > 0f && gamePadOld.ThumbSticks.Left.X == 0f && stage9Unlock)
            {
                stage4Selecting = false;
                stage9Selecting = true;
            }
            else if (stage5Selecting && gamePadCurrent.ThumbSticks.Left.X > 0f && gamePadOld.ThumbSticks.Left.X == 0f && stage10Unlock)
            {
                stage5Selecting = false;
                stage10Selecting = true;
            }
            else if (stage6Selecting && gamePadCurrent.ThumbSticks.Left.X < 0f && gamePadOld.ThumbSticks.Left.X == 0f)
            {
                stage6Selecting = false;
                stage1Selecting = true;
            }
            else if (stage7Selecting && gamePadCurrent.ThumbSticks.Left.X < 0f && gamePadOld.ThumbSticks.Left.X == 0f)
            {
                stage7Selecting = false;
                stage2Selecting = true;
            }
            else if (stage8Selecting && gamePadCurrent.ThumbSticks.Left.X < 0f && gamePadOld.ThumbSticks.Left.X == 0f && stage3Unlock)
            {
                stage8Selecting = false;
                stage3Selecting = true;
            }
            else if (stage9Selecting && gamePadCurrent.ThumbSticks.Left.X < 0f && gamePadOld.ThumbSticks.Left.X == 0f && stage4Unlock)
            {
                stage9Selecting = false;
                stage4Selecting = true;
            }
            else if (stage10Selecting && gamePadCurrent.ThumbSticks.Left.X < 0f && gamePadOld.ThumbSticks.Left.X == 0f && stage5Unlock)
            {
                stage10Selecting = false;
                stage5Selecting = true;
            }
            else if (stage6Selecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f && stage7Unlock)
            {
                stage6Selecting = false;
                stage7Selecting = true;
            }
            else if (stage7Selecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f && stage8Unlock)
            {
                stage7Selecting = false;
                stage8Selecting = true;
            }
            else if (stage8Selecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f && stage9Unlock)
            {
                stage8Selecting = false;
                stage9Selecting = true;
            }
            else if (stage9Selecting && gamePadCurrent.ThumbSticks.Left.Y < -0.5f && gamePadOld.ThumbSticks.Left.Y >= -0.5f && stage10Unlock)
            {
                stage9Selecting = false;
                stage10Selecting = true;
            }
            else if (stage10Selecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f && stage9Unlock)
            {
                stage10Selecting = false;
                stage9Selecting = true;
            }
            else if (stage9Selecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f && stage8Unlock)
            {
                stage9Selecting = false;
                stage8Selecting = true;
            }
            else if (stage8Selecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f && stage7Unlock)
            {
                stage8Selecting = false;
                stage7Selecting = true;
            }
            else if (stage7Selecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f && stage6Unlock)
            {
                stage7Selecting = false;
                stage6Selecting = true;
            }
            else if (stage6Selecting && gamePadCurrent.ThumbSticks.Left.Y > 0.5f && gamePadOld.ThumbSticks.Left.Y <= 0.5f && stage5Unlock)
            {
                stage6Selecting = false;
                stage5Selecting = true;
            }
        }
        int GetWeaponIconIndex(string weapon)
        {
            switch (weapon)
            {
                case "slaughterDemon": return 0;
                case "handSword": return 1;
                case "basicKatana": return 2;
                case "cursedSpear": return 3;
                case "ratioKnife": return 4;
                case "cursedSword": return 5;
                case "mallet": return 6;
                case "dragonBone": return 7;
                case "splitSoul": return 8;
                case "festeringLife": return 9;
                case "playfulCloud": return 10;
                case "invertedSpear": return 11;

                default: return -1;
            }
        }
        string GetWeaponName(string weapon)
        {
            switch (weapon)
            {
                case "slaughterDemon": return "Slaughter Demon";
                case "handSword": return "Hand Sword";
                case "basicKatana": return "Basic Katana";
                case "cursedSpear": return "Cursed Spear";
                case "ratioKnife": return "Ratio Knife";
                case "cursedSword": return "Cursed Sword";
                case "mallet": return "Mallet";
                case "dragonBone": return "Dragon Bone";
                case "splitSoul": return "Split Soul";
                case "festeringLife": return "Festering Life";
                case "playfulCloud": return "Playful Cloud";
                case "invertedSpear": return "Inverted Spear";

                default: return "Unknown Weapon";
            }
        }
        void DrawWeaponMenu()
        {
            _spriteBatch.Draw(emptyBox, new Rectangle(400, 40, 1050, 820), Color.White);
            _spriteBatch.DrawString(titleFont, "WEAPON SELECTION", new Vector2(700, 70), Color.White);

            if (weaponList.Count == 0)
            {
                _spriteBatch.DrawString(titleFont, "You haven't obtained any weapons!", new Vector2(650, 400), Color.White);
                return;
            }

            for (int i = 0; i < weaponList.Count; i++)
            {
                int x = 500 + (i % 4 * 230);
                int y = 150 + (i / 4 * 210);

                Rectangle weaponR = new Rectangle(x, y, 200, 180);
                if (i == weaponMenuIndex)
                    _spriteBatch.Draw(emptyBox, weaponR, Color.Yellow);
                else
                    _spriteBatch.Draw(emptyBox, weaponR, Color.White);

                int iconIndex = GetWeaponIconIndex(weaponList[i]);
                if (iconIndex != -1)
                    _spriteBatch.Draw(weaponIcons[iconIndex], new Rectangle(x + 25, y + 15, 150, 105), Color.White);

                _spriteBatch.DrawString(stagesFont,
                    GetWeaponName(weaponList[i]),
                    new Vector2(x + 10, y + 130), Color.White);

                if (selectedWeapon == weaponList[i])
                    _spriteBatch.DrawString(stagesFont, "EQUIPPED", new Vector2(x + 10, y + 155), Color.Green);
            }

            _spriteBatch.DrawString(stagesFont, "B - Equip/Unequip     A - Back", new Vector2(700, 850), Color.White);
        }
        int GetSkillIconIndex(string skill)
        {
            switch (skill)
            {
                case "explodingFlesh": return 0;
                case "strawDoll": return 1;
                case "miracle": return 2;
                case "ice": return 3;
                case "blood": return 4;
                case "projection": return 5;
                case "ratio": return 6;
                case "RCT": return 7;
                case "curseControl": return 8;
                case "bugArmour": return 9;
                case "graniteBlast": return 10;
                case "dismantle": return 11;
                case "cleave": return 12;
                case "beastAmber": return 13;
                case "blue": return 14;
                case "red": return 15;
                case "fuga": return 16;
                case "purple": return 17;

                default: return -1;
            }
        }
        string GetSkillName(string skill)
        {
            switch (skill)
            {
                case "explodingFlesh": return "Exploding Flesh";
                case "strawDoll": return "Straw Doll";
                case "miracle": return "Miracle";
                case "ice": return "Ice";
                case "blood": return "Blood";
                case "projection": return "Projection";
                case "ratio": return "Ratio";
                case "RCT": return "RCT";
                case "curseControl": return "Curse Control";
                case "bugArmour": return "Bug Armour";
                case "graniteBlast": return "Granite Blast";
                case "dismantle": return "Dismantle";
                case "cleave": return "Cleave";
                case "beastAmber": return "Beast Amber";
                case "blue": return "Blue";
                case "red": return "Red";
                case "fuga": return "Fuga";
                case "purple": return "Purple";

                default: return "Unknown Skill";
            }
        }
        void DrawSkillMenu()
        {
            _spriteBatch.Draw(emptyBox,
                new Rectangle(400, 40, 1050, 820),
                Color.White);

            _spriteBatch.DrawString(titleFont,
                "SKILL SELECTION",
                new Vector2(700, 70),
                Color.White);

            _spriteBatch.DrawString(stagesFont,
                "Equipped: " + equippedSkills.Count + "/3",
                new Vector2(800, 150),
                Color.White);

            if (skillList.Count == 0)
            {
                _spriteBatch.DrawString(titleFont,
                    "You haven't obtained any skills!",
                    new Vector2(600, 400),
                    Color.White);

                return;
            }

            for (int i = 0; i < skillList.Count; i++)
            {
                int x = 500 + (i % 4 * 230);
                int y = 200 + (i / 4 * 175);

                Rectangle skillR = new Rectangle(x, y, 200, 155);

                // Highlight selected skill
                if (i == skillMenuIndex)
                    _spriteBatch.Draw(emptyBox, skillR, Color.Yellow);
                else
                    _spriteBatch.Draw(emptyBox, skillR, Color.White);

                int iconIndex = GetSkillIconIndex(skillList[i]);

                if (iconIndex != -1)
                {
                    _spriteBatch.Draw(skillIcons[iconIndex],
                        new Rectangle(x + 35, y + 5, 130, 95),
                        Color.White);
                }

                _spriteBatch.DrawString(stagesFont,
                    GetSkillName(skillList[i]),
                    new Vector2(x + 10, y + 105),
                    Color.White);

                // Show whether the skill is equipped
                if (equippedSkills.Contains(skillList[i]))
                {
                    _spriteBatch.DrawString(stagesFont,
                        "EQUIPPED",
                        new Vector2(x + 10, y + 130),
                        Color.Green);
                }
            }

            _spriteBatch.DrawString(stagesFont, "B - Equip/Unequip     A - Back", new Vector2(700, 850), Color.White);
        }
        private void DrawBattleSkillSelector()
        {
            if (!choosingBattleSkill || equippedSkills.Count == 0)
                return;

            _spriteBatch.Draw(barTexture, new Rectangle(420, 410, 650, 225), Color.Black * 0.9f);
            _spriteBatch.DrawString(stagesFont, "CHOOSE SKILL", new Vector2(490, 425), Color.White);

            for (int i = 0; i < equippedSkills.Count; i++)
            {
                Color color = i == battleSkillIndex ? Color.Yellow : Color.White;
                string prefix = i == battleSkillIndex ? "> " : "  ";
                _spriteBatch.DrawString(stagesFont, prefix + GetSkillName(equippedSkills[i]), new Vector2(490, 465 + i * 43), color);
            }
            _spriteBatch.DrawString(stagesFont, "B: Use   A: Cancel", new Vector2(780, 590), Color.White);
        }
        private List<Texture2D> LoadAttackFrames(string folder, string prefix, int frameCount)
        {
            List<Texture2D> frames = new List<Texture2D>();

            for (int i = 1; i <= frameCount; i++)
            {
                string path = folder + "/" + prefix + i;

                try
                {
                    frames.Add(Content.Load<Texture2D>(path));
                }
                catch (Microsoft.Xna.Framework.Content.ContentLoadException ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAILED TO LOAD: " + path);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw;
                }
            }

            return frames;
        }

        private void RegisterSkills()
        {
            TryRegisterSkill("explodingFlesh", 7, 30);
            TryRegisterSkill("strawDoll", 6, 35);
            TryRegisterSkill("miracle", 6, 0, charAttack.EffectType.Defence);
            TryRegisterSkill("ice", 6, 45);
            TryRegisterSkill("blood", 8, 50);
            TryRegisterSkill("projection", 9, 0, charAttack.EffectType.Speed);
            TryRegisterSkill("ratio", 8, 60);
            TryRegisterSkill("RCT", 9, 0, charAttack.EffectType.Heal);
            TryRegisterSkill("curseControl", 8, 70);
            TryRegisterSkill("bugArmour", 9, 0, charAttack.EffectType.Defence);
            TryRegisterSkill("graniteBlast", 8, 90);
            TryRegisterSkill("dismantle", 8, 100);
            TryRegisterSkill("cleave", 8, 110);
            TryRegisterSkill("beastAmber", 8, 125);
            TryRegisterSkill("blue", 9, 130);
            TryRegisterSkill("red", 9, 140);
            TryRegisterSkill("fuga", 9, 200);
            TryRegisterSkill("purple", 9, 350);
        }

        private void TryRegisterSkill(string name, int frames, int damage, charAttack.EffectType effect = charAttack.EffectType.None)
        {
            try
            {
                playerAttack.AddAttack(name, LoadAttackFrames("Skills", name, frames), damage, 0.12f, effect);
            }
            catch (Microsoft.Xna.Framework.Content.ContentLoadException)
            {
                System.Diagnostics.Debug.WriteLine("Missing skill animation: " + name);
            }
        }
    }
}
