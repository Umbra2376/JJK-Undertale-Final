using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace JJK_Undertale_Final
{
    public class b1Projectile
    {
        private Vector2 _position;
        private Vector2 _direction;
        private float _rotation;

        public b1Projectile(Vector2 position, Vector2 direction, float rotation)
        {
            _position = position;
            _direction = direction;
            _rotation = rotation;
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
    }
}
