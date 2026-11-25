using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace JJK_Undertale_Final
{
    public class b5Projectile
    {
        public enum AttackType
        {
            Swarm, Slash
        }
        public AttackType _type;
        public Vector2 _position;
        public bool _isFinished;

        private Texture2D _roachTexture;
        private Texture2D _slashTexture;
        private Texture2D _eggTexture;
        private Texture2D _warningTexture;

        private Rectangle _dodgeBox;
        private float _timer = 0f;
        private float _rotation;

        private float _swarmTravelTime = 2.2f;
        private float _swarmBurstTime = 2.6f;
        private float _swarmBurstSpeed = 220f;
        private bool _swarmBurst = false;
        private float _slashChargeTime = 0.5f;
        private float _slashAttackTime = 0.15f;
        private float _slashThickness = 30f;
        private float _eggHatchTime = 0.8f;
        private float _eggProjectileSpeed = 200f;

        private Vector2 _slashStart;
        private Vector2 _slashEnd;
        private Vector2 _slashDirection;
        private float _slashLength;
        private bool _eggsHatched = false;
        private List<Vector2> _eggs = new List<Vector2>();
        private List<Cockroach> _cockroaches = new List<Cockroach>();

        private int _damage = 12;

        public int Damage
        {
            get { return _damage; }
        }

        private class Cockroach
        {
            public Vector2 Position;
            public Vector2 Start;
            public Vector2 Control;
            public Vector2 Target;
            public Vector2 Direction;

            public float Rotation;
            public float Delay;
            public float Speed;

            public bool Curved;
            public bool Active;
            public bool Finished;
        }
        public b5Projectile(Vector2 position, float angle, Texture2D roachTexture, Rectangle dodgeBox)
        {
            _type = AttackType.Swarm;
            _position = position;
            _rotation = angle;
            _roachTexture = roachTexture;
            _dodgeBox = dodgeBox;
            CreateSwarm();
        }
        public b5Projectile(Vector2 position, float angle, Texture2D slashTexture, Texture2D roachTexture, Texture2D eggTexture, Texture2D warningTexture, Rectangle dodgeBox)
        {
            _type = AttackType.Slash;
            _position = position;
            _rotation = angle;
            _slashTexture = slashTexture;
            _eggTexture = eggTexture;
            _roachTexture = roachTexture;
            _warningTexture = warningTexture;
            _dodgeBox = dodgeBox;
            _slashDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            CalculateSlash();
        }
        private void CreateSwarm()
        {
            Vector2 center = new Vector2(_dodgeBox.Center.X, _dodgeBox.Center.Y);
            Vector2 direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));
            Vector2 perpendicular = new Vector2(direction.Y, direction.X);
            float distance = (float)Math.Sqrt(_dodgeBox.Width * _dodgeBox.Width + _dodgeBox.Height * _dodgeBox.Height) / 2f + 60f;

            for (int side = -1; side <= 1; side += 2)
            {
                for (int i = 0; i < 15; i++)
                {
                    int lane = i % 5 - 2;
                    int row = i / 5;
                    Vector2 start = center + direction * distance * side + perpendicular * lane * 25f;
                    Vector2 control = center + direction * (120f + lane * 20f) * side + perpendicular * (220f + lane * 35f) * side;

                    Cockroach roach = new Cockroach
                    {
                        Position = start,
                        Start = start,
                        Control = control,
                        Target = _position,
                        Delay = row * 0.12f + (i % 5) * 0.025f,
                        Curved = true,
                        Active = false,
                        Finished = false
                    };
                    _cockroaches.Add(roach);
                }
            }
        }
        private Vector2 Curve(Vector2 start, Vector2 control, Vector2 end, float t)
        {
            float u = 1f - t;
            return u * u * start + 2f * u * t * control + t * t * end;
        }
        private void CalculateSlash()
        {
            float minimum = float.NegativeInfinity;
            float maximum = float.PositiveInfinity;

            if (Math.Abs(_slashDirection.X) > 0.0001f)
            {
                float t1 = (_dodgeBox.Left - _position.X) / _slashDirection.X;
                float t2 = (_dodgeBox.Right - _position.X) / _slashDirection.X;

                minimum = Math.Max(minimum, Math.Min(t1, t2));
                maximum = Math.Min(maximum, Math.Max(t1, t2));
            }

            if (Math.Abs(_slashDirection.Y) > 0.0001f)
            {
                float t1 = (_dodgeBox.Top - _position.Y) / _slashDirection.Y;
                float t2 = (_dodgeBox.Bottom - _position.Y) / _slashDirection.Y;

                minimum = Math.Max(minimum, Math.Min(t1, t2));
                maximum = Math.Min(maximum, Math.Max(t1, t2));
            }

            _slashStart = _position + _slashDirection * minimum;
            _slashEnd = _position + _slashDirection * maximum;

            _slashLength = Vector2.Distance(_slashStart, _slashEnd);

            for (int i = 1; i <= 5; i++)
            {
                Vector2 eggPosition = Vector2.Lerp(_slashStart, _slashEnd, i / 6f);
                _eggs.Add(eggPosition);
            }
        }
        public void Update(GameTime gameTime)
        {
            if (_isFinished)
                return;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timer += dt;

            if (_type == AttackType.Swarm)
                UpdateSwarm();
            else if (_type == AttackType.Slash)
                UpdateSlash();
            UpdateCockroaches(dt);

            if (_type == AttackType.Swarm)
            {
                if (_swarmBurst && _cockroaches.Count == 0)
                    _isFinished = true;
            }
            else if (_type == AttackType.Slash)
            {
                if (_eggsHatched && _cockroaches.Count == 0)
                    _isFinished = true;
            }
        }
        private void UpdateSwarm()
        {
            if (_timer >= _swarmBurstTime && !_swarmBurst)
            {
                _swarmBurst = true;
                for (int i = 0; i < 12; i++)
                {
                    float angle = i * MathHelper.TwoPi / 12f + _rotation;
                    SpawnStraightCockroach(_position, angle, _swarmBurstSpeed);
                }
            }
        }
        private void UpdateSlash()
        {
            float hatchTime = _slashChargeTime + _slashAttackTime + _eggHatchTime;

            if (_timer >= hatchTime && !_eggsHatched)
            {
                _eggsHatched = true;

                foreach (Vector2 egg in _eggs)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float angle = _rotation + MathHelper.PiOver4 + i * MathHelper.PiOver2;
                        SpawnStraightCockroach(egg, angle, _eggProjectileSpeed);
                    }
                }
                _eggs.Clear();
            }
        }
        private void SpawnStraightCockroach(Vector2 position, float angle, float speed)
        {
            Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            Cockroach roach = new Cockroach
            {
                Position = position,
                Direction = direction,
                Rotation = angle + MathHelper.PiOver2,
                Speed = speed,
                Curved = false,
                Active = true,
                Finished = false
            };
            _cockroaches.Add(roach);
        }
        private void UpdateCockroaches(float dt)
        {
            for (int i = _cockroaches.Count - 1; i >= 0; i--)
            {
                Cockroach roach = _cockroaches[i];

                if (roach.Curved)
                {
                    if (_timer < roach.Delay)
                        continue;
                    roach.Active = true;

                    float progress = MathHelper.Clamp((_timer - roach.Delay) / _swarmTravelTime, 0f, 1f);
                    float t = (float)Math.Pow(progress, 1.35f);

                    Vector2 oldPosition = roach.Position;

                    roach.Position = Curve(roach.Start, roach.Control, roach.Target, t);
                    Vector2 movement = roach.Position - oldPosition;

                    if (movement.LengthSquared() > 0.001f)
                        roach.Rotation = (float)Math.Atan2(movement.Y, movement.X) + MathHelper.PiOver2;
                    if (progress >= 1f)
                    {
                        roach.Position = roach.Target;
                        if (_swarmBurst)
                            roach.Finished = true;
                    }
                }
                else
                {
                    roach.Position += roach.Direction * roach.Speed * dt;

                    if (roach.Position.X < _dodgeBox.Left - 40 || roach.Position.X > _dodgeBox.Right + 40 || roach.Position.Y < _dodgeBox.Top - 40 || roach.Position.Y > _dodgeBox.Bottom + 40)
                        roach.Finished = true;
                }
                if (roach.Finished)
                    _cockroaches.RemoveAt(i);
            }
        }
        public bool Collides(Rectangle soulHitbox)
        {
            if (_isFinished)
                return false;
            if (_type == AttackType.Slash)
            {
                if (_timer >= _slashChargeTime && _timer < _slashChargeTime + _slashAttackTime)
                {
                    if (SlashCollides(soulHitbox))
                    {
                        _damage = 30;
                        return true;
                    }
                }
            }
            foreach (Cockroach roach in _cockroaches)
            {
                if (!roach.Active || roach.Finished)
                    continue;
                if (!_dodgeBox.Contains((int)roach.Position.X, (int)roach.Position.Y))
                    continue;
                Rectangle roachHitbox = new Rectangle((int)roach.Position.X - 7, (int)roach.Position.Y - 9, 14, 18);
                if (roachHitbox.Intersects(soulHitbox))
                {
                    _damage = 12;
                    return true;
                }
            }
            return false;
        }
        private bool SlashCollides(Rectangle soulHitbox)
        {
            Vector2 soulCenter = new Vector2(soulHitbox.Center.X, soulHitbox.Center.Y);
            Vector2 difference = soulCenter - _slashStart;
            float projection = Vector2.Dot(difference, _slashDirection);
            projection = MathHelper.Clamp(projection, 0f, _slashLength);
            Vector2 closestPoint = _slashStart + _slashDirection * projection;
            float distance = Vector2.Distance(soulCenter, closestPoint);
            float soulRadius = Math.Max(soulHitbox.Width, soulHitbox.Height) / 2f;

            return distance <= _slashThickness / 2f + soulRadius;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isFinished)
                return;
            if (_type == AttackType.Slash)
                DrawSlash(spriteBatch);

            DrawCockroaches(spriteBatch);
        }
        private void DrawCockroaches(SpriteBatch spriteBatch)
        {
            foreach (Cockroach roach in _cockroaches)
            {
                if (!roach.Active || roach.Finished)
                    continue;
                if (!_dodgeBox.Contains((int)roach.Position.X, (int)roach.Position.Y))
                    continue;

                spriteBatch.Draw(_roachTexture, roach.Position, null, Color.White, roach.Rotation, new Vector2(_roachTexture.Width / 2f, _roachTexture.Height / 2f), new Vector2(24f / _roachTexture.Width, 30f / _roachTexture.Height), SpriteEffects.None, 0f);
            }
        }
        private void DrawSlash(SpriteBatch spriteBatch)
        {
            if (_timer < _slashChargeTime)
            {
                float progress = _timer / _slashChargeTime;
                float opacity = MathHelper.Lerp(0.25f, 0.9f, progress);

                spriteBatch.Draw(_warningTexture, _slashStart, null, Color.Red * opacity, _rotation, new Vector2(0f, _warningTexture.Height / 2f), new Vector2(_slashLength / _warningTexture.Width, 3f / _warningTexture.Height), SpriteEffects.None, 0f);
            }
            else if (_timer < _slashChargeTime + _slashAttackTime)
            {
                float progress = MathHelper.Clamp((_timer - _slashChargeTime) / _slashAttackTime, 0f, 1f);
                float opacity = MathHelper.Lerp(1f, 0.5f, progress);

                spriteBatch.Draw(_slashTexture, _slashStart, null, Color.White * opacity, _rotation, new Vector2(0f, _slashTexture.Height / 2f), new Vector2(_slashLength / _slashTexture.Width, _slashThickness / _slashTexture.Height), SpriteEffects.None, 0f);
            }
            else if (!_eggsHatched)
            {
                float progress = MathHelper.Clamp((_timer - _slashChargeTime - _slashAttackTime) / _eggHatchTime, 0f, 1f);
                float pulse = 1f + (float)Math.Sin(_timer * 25f) * 0.1f * progress;

                foreach (Vector2 egg in _eggs)
                    spriteBatch.Draw(_eggTexture, egg, null, Color.White, 0f, new Vector2(_eggTexture.Width / 2f, _eggTexture.Height / 2f), new Vector2(18f / _eggTexture.Width * pulse, 24f / _eggTexture.Height * pulse), SpriteEffects.None, 0f);
            }
        }
    }
}
