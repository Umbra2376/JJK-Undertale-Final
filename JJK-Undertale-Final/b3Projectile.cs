
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JJK_Undertale_Final
{
    public class b3Projectile
    {
        public enum ProjectileType
        {
            WingKing, RottingBlood
        }

        public Vector2 _position;
        public Vector2 _direction;
        public ProjectileType _type;

        private Texture2D _texture;
        private float _speed;
        private float _lifetime;
        private float _rotation;

        public bool IsPoisonZone { get; private set; }
        public bool IsFinished { get; private set; }

        public Rectangle Hitbox
        {
            get
            {
                int size = IsPoisonZone ? 105 : _type == ProjectileType.WingKing ? 24 : 35;

                return new Rectangle((int)_position.X - size / 2, (int)_position.Y - size / 2, size, size);
            }
        }

        public b3Projectile(Vector2 position, Vector2 direction, ProjectileType type, Texture2D texture)
        {
            _position = position;
            _direction = direction;
            _type = type;
            _texture = texture;

            _speed = type == ProjectileType.WingKing ? 320f : 115f;
            _lifetime = type == ProjectileType.WingKing ? 4f : 2.2f;
            _rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsFinished)
                return;
            _lifetime -= dt;

            if (IsPoisonZone)
            {
                if (_lifetime <= 0f)
                    IsFinished = true;
                return;
            }

            _position += _direction * _speed * dt;

            if (_lifetime <= 0f)
            {
                if (_type == ProjectileType.RottingBlood)
                {
                    IsPoisonZone = true;
                    _lifetime = 2.5f;
                }
                else
                    IsFinished = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsFinished)
                return;

            if (IsPoisonZone)
            {
                spriteBatch.Draw(_texture, Hitbox, Color.MediumPurple * 0.65f);
            }
            else
            {
                Color tint = _type == ProjectileType.WingKing ? Color.White : Color.LightGreen;

                spriteBatch.Draw(_texture, Hitbox, null, tint, _rotation, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }
    }
}
