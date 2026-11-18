using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace JJK_Undertale_Final
{
    public class b4Projectile
    {
        public enum ProjectileType
        {
            PiercingBlood, Supernova
        }
        public ProjectileType _type;

        public Vector2 _position;
        private Vector2 _direction;

        private Texture2D _handsTexture;
        private Texture2D _chargeTexture;
        private Texture2D _beamTexture;
        private List<Texture2D> _supernovaFrames;

        private Rectangle _dodgeBox;

        private float _timer = 0f;
        private float _rotation;
        private float _beamLength;

        private float _piercingChargeTime = 0.65f;
        private float _piercingFireTime = 0.16f;
        private float _superNovaChargeTime = 0.36f;
        private float _superNovaFrameTime = 0.08f;

        private int _currentFrame = 0;
        public bool _isFinished = false;

        public int Damage
        {
            get
            {
                if (_type == ProjectileType.PiercingBlood)
                    return 25;
                return 15;
            }
        }

        public b4Projectile(Vector2 position, Vector2 direction, Texture2D handsTexture, Texture2D chargeTexture, Texture2D beamTexture, Rectangle dodgeBox)
        {
            _type = ProjectileType.PiercingBlood;
            _position = position;
            _direction = direction;
            _handsTexture = handsTexture;
            _chargeTexture = chargeTexture;
            _beamTexture = beamTexture;
            _dodgeBox = dodgeBox;
            _rotation = (float)Math.Atan2(direction.Y, direction.X);

            if (direction.X != 0)
                _beamLength = dodgeBox.Width;
            else
                _beamLength = dodgeBox.Height;
        }
        public b4Projectile(Vector2 position, Texture2D chargeTexture, List<Texture2D> supernoveFrames)
        {
            _type = ProjectileType.Supernova;
            _position = position;
            _chargeTexture = chargeTexture;
            _supernovaFrames = supernoveFrames;
        }
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timer += dt;

            if (_type == ProjectileType.PiercingBlood)
            {
                if (_timer >= _piercingChargeTime)
                    _isFinished = true;
            }
            else if (_type == ProjectileType.Supernova)
            {
                if (_timer >= _superNovaChargeTime)
                {
                    _currentFrame = (int)((_timer - _superNovaChargeTime) / _superNovaFrameTime);
                    if (_currentFrame >= 4)
                        _isFinished = true;
                }
            }
        }
        public Rectangle Hitbox
        {
            get
            {
                if (_isFinished)
                    return Rectangle.Empty;
                if (_type == ProjectileType.PiercingBlood)
                {
                    if (_timer < _piercingChargeTime)
                        return Rectangle.Empty;
                    int _thickness = 14;

                    if (_direction.X != 0)
                        return new Rectangle(_dodgeBox.Left, (int)_position.Y - _thickness / 2, _dodgeBox.Width, _thickness);
                    return new Rectangle((int)_position.X - _thickness / 2, _dodgeBox.Top, _thickness, _dodgeBox.Height);
                }
                if (_timer < _superNovaChargeTime)
                    return Rectangle.Empty;
                int radius = 14 + _currentFrame * 7;
                return new Rectangle((int)_position.X - radius, (int)_position.Y - radius, radius * 2, radius * 2);
            }
        }
        public bool Collides(Rectangle soulHitbox)
        {
            if (_isFinished)
                return false;
            if (_type == ProjectileType.PiercingBlood)
                return _timer >= _piercingChargeTime && Hitbox.Intersects(soulHitbox);

            if (_timer < _superNovaChargeTime)
                return false;
            float _radius = 14 + _currentFrame * 7;
            float closestX = MathHelper.Clamp(_position.X, soulHitbox.Left, soulHitbox.Right);
            float closestY = MathHelper.Clamp(_position.Y, soulHitbox.Top, soulHitbox.Bottom);
            float dx = _position.X - closestX;
            float dy = _position.Y - closestY;

            return dx * dx * dy * dy <= _radius * _radius;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isFinished)
                return;
            if (_type == ProjectileType.PiercingBlood)
                DrawPiercingBlood(spriteBatch);
            else if (_type == ProjectileType.Supernova)
                DrawSupernova(spriteBatch);
        }
        private void DrawPiercingBlood(SpriteBatch spriteBatch)
        {
            Vector2 handPosition = _position - _direction * 40f;
            spriteBatch.Draw(_handsTexture, handPosition, null, Color.White, _rotation, new Vector2(_handsTexture.Width / 2f, _handsTexture.Height / 2f), new Vector2(70f / _handsTexture.Width, 70f / _handsTexture.Height), SpriteEffects.None, 0f);
            
            if (_timer < _piercingChargeTime)
            {
                float _progress = _timer / _piercingChargeTime;
                float _size = MathHelper.Lerp(8f, 36f, _progress);

                spriteBatch.Draw(_chargeTexture, _position, null, Color.White, 0f, new Vector2(_chargeTexture.Width / 2f, _chargeTexture.Height / 2), new Vector2(_size / _chargeTexture.Width, _size / _chargeTexture.Height), SpriteEffects.None, 0f);
            }
            else
            {
                float _progress = (_timer - _piercingChargeTime) / _piercingFireTime;
                float _thickness = MathHelper.Lerp(20f, 8f, _progress);
                float _opacity = MathHelper.Lerp(1f, 0.4f, _progress);

                spriteBatch.Draw(_beamTexture, _position, null, Color.White * _opacity, _rotation, new Vector2(0f, _beamTexture.Height / 2), new Vector2(_beamLength / _beamTexture.Width ,_thickness / _beamTexture.Height), SpriteEffects.None, 0f);
            }
        }
        private void DrawSupernova(SpriteBatch spriteBatch)
        {
            if (_timer < _superNovaChargeTime)
            {
                float _progress = _timer / _superNovaChargeTime;
                float _size = MathHelper.Lerp(8f, 24f, _progress);

                spriteBatch.Draw(_chargeTexture, _position, null, Color.White, 0f, new Vector2(_chargeTexture.Width / 2, _chargeTexture.Height / 2), new Vector2(_size / _chargeTexture.Width, _size / _chargeTexture.Height), SpriteEffects.None, 0f);
            }
            else
            {
                int _frame = Math.Clamp(_currentFrame, 0, 3);
                Texture2D _texture = _supernovaFrames[_frame];
                int _size = 28 + _frame * 14;

                spriteBatch.Draw(_texture, new Rectangle((int)_position.X - _size / 2, (int)_position.Y - _size / 2, _size, _size), Color.White);
            }
        }
    }
}