using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JJK_Undertale_Final
{
    public class b2Projectile
    {
        private Vector2 _position;
        private Vector2 _direction;
        private float _rotation;
        private Texture2D _texture;

        private float _speed = 180f;

        public b2Projectile(Vector2 position, Vector2 direction, float rotation, Texture2D texture)
        {
            _position = position;
            _direction = direction;
            _direction.Normalize();
            _rotation = rotation;
            _texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            _position += _direction * _speed * deltaTime;
            _rotation += 2f * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw( _texture, _position, null, Color.White, _rotation, new Vector2(_texture.Width / 2f, _texture.Height / 2f), 0.03f, SpriteEffects.None, 0f);
        }

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)_position.X - 10, (int)_position.Y - 10, 1, 1);
            }
        }

        public Vector2 Position
        {
            get { return _position; }
        }
    }
}
