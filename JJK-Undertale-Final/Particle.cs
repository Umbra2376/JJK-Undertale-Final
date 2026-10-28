using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJK_Undertale_Final
{
    public class Particle
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _velocity;
        private Rectangle _rect;
        private float _angle;
        private float _angularVelocity;
        private Color _color;
        private int _size;
        private int _ttl;
        private bool _gravityEnabled;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, int size, int ttl, bool gravityEnabled)
        {
            _gravityEnabled = gravityEnabled;
            _texture = texture;
            _position = position;
            _rect = new Rectangle(0, 0, size, size);
            _rect.Location = _position.ToPoint();
            _velocity = velocity;
            _angle = angle;
            _angularVelocity = angularVelocity;
            _color = color;
            _size = size;
            _ttl = ttl;
        }
        public void Update()
        {
            _position += _velocity;
            if (_gravityEnabled)
            {
                _velocity.Y += 0.15f;
            }
            _angle += _angularVelocity;
            _rect.Location = _position.ToPoint();
            _ttl--;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            spriteBatch.Draw(_texture, _rect, null, _color, _angle, origin, SpriteEffects.None, 0f);
        }
        public bool Dead
        {
            get { return _ttl <= 0; }
        }
    }
}