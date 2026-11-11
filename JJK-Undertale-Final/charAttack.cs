
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace JJK_Undertale_Final
{
    public class charAttack
    {
        public enum AttackType
        {
            Punch,
            Weapon,
            Skill
        }

        public enum EffectType
        {
            None,
            Heal,
            Speed,
            Defence
        }

        private class AttackData
        {
            public List<Texture2D> Frames;
            public int Damage;
            public float FrameDuration;
            public EffectType Effect;

            public AttackData(List<Texture2D> frames, int damage, float frameDuration, EffectType effect)
            {
                Frames = frames;
                Damage = damage;
                FrameDuration = frameDuration;
                Effect = effect;
            }
        }

        private readonly Dictionary<string, AttackData>
        attacks = new Dictionary<string, AttackData>();

        private readonly Random random = new Random();

        private AttackData currentAttack;
        private int currentFrame;
        private float timer;

        private Rectangle attackRectangle = new Rectangle(570, 60, 350, 350);

        public bool IsAttacking { get; private set; }
        public int Damage { get; private set; }
        public int BF { get; private set; }
        public string CurrentAttackName { get; private set; }
        public AttackType CurrentType { get; private set; }
        public EffectType CurrentEffect { get; private set; }

        public charAttack()
        {
            Reset();
        }

        public void AddAttack(string name, List<Texture2D> frames, int damage, float frameDuration = 0.12f, EffectType effect = EffectType.None)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Invalid attack name.");

            if (frames == null || frames.Count == 0)
                throw new ArgumentException("An attack needs at least one frame.");

            if (frameDuration <= 0f)
                throw new ArgumentException("Frame duration must be positive.");

            attacks[name] = new AttackData(frames, damage, frameDuration, effect);
        }

        public bool StartPunch()
        {
            if (IsAttacking)
                return false;
            bool blackFlash = random.Next(100) == 0;

            string attackName = blackFlash ? "blackFlash" : "punch";

            bool started = StartAttack(attackName, AttackType.Punch);

            if (started)
                BF = blackFlash ? 1 : 0;

            return started;
        }

        public bool StartWeapon(string weaponName)
        {
            return StartAttack(weaponName, AttackType.Weapon);
        }

        public bool StartSkill(string skillName)
        {
            return StartAttack(skillName, AttackType.Skill);
        }

        private bool StartAttack(string name, AttackType type)
        {
            if (IsAttacking)
                return false;

            if (!attacks.TryGetValue(name, out AttackData data))
                return false;

            currentAttack = data;
            CurrentAttackName = name;
            CurrentType = type;
            CurrentEffect = data.Effect;

            Damage = data.Damage;
            currentFrame = 0;
            timer = 0f;
            BF = 0;

            IsAttacking = true;
            return true;
        }

        public bool Update(GameTime gameTime)
        {
            if (!IsAttacking || currentAttack == null)
                return false;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer >= currentAttack.FrameDuration)
            {
                timer -= currentAttack.FrameDuration;
                currentFrame++;

                if (currentFrame >= currentAttack.Frames.Count)
                {
                    IsAttacking = false;
                    currentAttack = null;
                    currentFrame = 0;
                    timer = 0f;
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsAttacking || currentAttack == null)
                return;

            spriteBatch.Draw(currentAttack.Frames[currentFrame], attackRectangle, Color.White);
        }

        public void SetAttackRectangle(Rectangle rectangle)
        {
            attackRectangle = rectangle;
        }

        public void Reset()
        {
            IsAttacking = false;
            currentAttack = null;
            currentFrame = 0;
            timer = 0f;

            Damage = 0;
            BF = 0;
            CurrentAttackName = "";
            CurrentEffect = EffectType.None;
            CurrentType = AttackType.Punch;
        }
    }
}
