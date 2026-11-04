using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace JJK_Undertale_Final
{
    public class charAttack
    {
        private enum Attack
        {
            punch, weapon, explodingFlesh, strawDoll, miracle, ice, blood, projection, ratio, RCT, curseControl, bugArmour, graniteBlast, dismantle, cleave, blue, red, fuga, purple
        }
        private Attack _attack;
        private List<Texture2D> _frames = new List<Texture2D>();
        private int _frame;
        private float timer;
        private float _frameDuration = 0.12f;
        public int _damage { get; } = 20;
        public bool _isPunching { get; private set; }
        private Rectangle _punchR = new Rectangle(700, 100, 200, 300);
        
        public charAttack(List<Texture2D> frames)
        {
            _frames = frames;
            _attack = Attack.punch;
        }

        public void Start()
        {
            if (_isPunching || _frames.Count == 0)
                return;

            _isPunching = true;
            _frame = 0;
            timer = 0f;
        }

        // Returns true when the punch finishes
        public bool Update(GameTime gameTime)
        {
            if (!_isPunching)
                return false;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer >= _frameDuration)
            {
                timer -= _frameDuration;
                _frame++;

                if (_frame >= _frames.Count)
                {
                    Reset();
                    return true;
                }
            }

            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isPunching && _frame < _frames.Count)
                spriteBatch.Draw(_frames[_frame], _punchR, Color.White);
        }
        public void Reset()
        {
            _isPunching = false;
            _frame = 0;
            timer = 0f;
        }
    }
}
