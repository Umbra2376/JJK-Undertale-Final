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
        public Vector2 _position;
        public Vector2 _direction;
        public ProjectileType _type;
        private Texture2D _texture;
        private float _speed;
        private float _lifetime;
        private float _rotation;
    }
}
