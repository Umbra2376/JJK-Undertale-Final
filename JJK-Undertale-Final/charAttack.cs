using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace JJK_Undertale_Final
{
    public class charAttack
    {
        private enum Attack
        {
            punch, weapon, explodingFlesh, strawDoll, miracle, ice, blood, projection, ratio, RCT, curseControl, bugArmour, graniteBlast, dismantle, cleave, blue, red, fuga, purple
        }
        private Random _BFchance = new Random();
        private Attack _attack;
        private List<Texture2D> _frames = new List<Texture2D>();
        private List<Texture2D> _BFframes = new List<Texture2D>();
        private int _frame, _BF, _BFframe;
        private float timer;
        private float _frameDuration = 0.12f;
        public int _damage;
        public bool _isPunching { get; private set; }
        private Rectangle _punchR = new Rectangle(570, 60, 350, 350);
        
        public charAttack(List<Texture2D> frames, List<Texture2D> BFframes)
        {
            _frames = frames;
            _BFframes = BFframes;
            _attack = Attack.punch;
        }

        public void Start()
        {
            if (_isPunching || _frames.Count == 0 || _BFframes.Count == 0)
                return;

            _BF = _BFchance.Next(1, 101);

            if (_BF == 1)
                _damage = 500;
            else
                _damage = 20;

            _isPunching = true;
            _frame = 0;
            _BFframe = 0;
            timer = 0f;
        }
        public bool Update(GameTime gameTime)
        {
            if (!_isPunching)
                return false;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer >= _frameDuration)
            {
                timer -= _frameDuration;

                if (_BF == 1)
                {
                    _BFframe++;

                    if (_BFframe >= _BFframes.Count)
                    {
                        Reset();
                        return true;
                    }
                }
                else
                {
                    _frame++;

                    if (_frame >= _frames.Count)
                    {
                        Reset();
                        return true;
                    }
                }
            }

            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isPunching && _frame < _frames.Count && _BF != 1)
                spriteBatch.Draw(_frames[_frame], _punchR, Color.White);
            else if (_isPunching && _frame < _frames.Count && _BF == 1)
                spriteBatch.Draw(_BFframes[_BFframe], _punchR, Color.White);
        }
        public void Reset()
        {
            _isPunching = false;
            _frame = 0;
            timer = 0f;
        }
        public int Damage
        {
            get { return _damage; }
        }
        public int BF
        {
            get { return _BF; }
        }
    }
}
