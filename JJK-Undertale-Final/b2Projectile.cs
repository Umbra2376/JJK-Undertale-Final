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
    public class b2Projectile
    {
        private Vector2 _position;
        private Vector2 _direction;
        private float _rotation;
        private Texture2D _texture;

        public b2Projectile(Vector2 position, Vector2 direction, float rotation, Texture2D texture)
        {
            _position = position;
            _direction = direction;
            _rotation = rotation;
            _texture = texture;
        }
    }
}
