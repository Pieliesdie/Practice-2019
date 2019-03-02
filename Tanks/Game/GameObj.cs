﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;

namespace Game
{
    public class GameObj
    {         
        delegate void ImageChanged();
        event ImageChanged OnImageChanged;

        public string name => this.ToString();
        public int speed;
        public PointF pos;
        protected Bitmap image;
        [Browsable(false)]//?????
        public Bitmap Image
        {
            get
            {
                if (image == null)
                {
                    return null;
                }
                Bitmap result = null;
                switch (Direction)
                {
                    case Direction.bot:
                        {
                            result = new Bitmap(RotateImage(image, RotateFlipType.Rotate90FlipNone));
                            break;
                        }
                    case Direction.left:
                        {
                            result = new Bitmap(RotateImage(image, RotateFlipType.Rotate180FlipNone));
                            break;
                        }
                    case Direction.right:
                        {
                            result = image;
                            break;
                        }
                    case Direction.top:
                        {
                            result = new Bitmap(RotateImage(image, RotateFlipType.Rotate270FlipNone));
                            break;
                        }
                }
                return result;
            }
            set
            {
                if (value != image)
                {
                    OnImageChanged?.Invoke();
                }
                image = value;
            }
        }
        public bool CanDestroy; //??????

        private Size size;
        [Browsable(false)]//??????
        public Size Size
        {
            get
            {
                if (Direction == Direction.top || Direction == Direction.bot)
                    return new Size(size.Height,size.Width);
                return size;
            }
            set => size = value;
        }

        protected Direction Direction;

        public GameObj(PointF pos, Size size, int speed = 0, Direction direction = Direction.right, Bitmap image = null)
        {
            this.pos = pos;
            this.Size = size;
            this.Direction = direction;
            this.speed = speed;
            this.Image = image;
        }

        public RectangleF HitBox => new RectangleF(pos, new SizeF(size.Width,size.Height));

        public void Update(float dt)
        {
            switch (Direction)
            {
                case Direction.bot:
                    {
                        pos.Y += (speed * dt);
                        break;
                    }
                case Direction.left:
                    {
                        pos.X -= (speed * dt);
                        break;
                    }
                case Direction.right:
                    {
                        pos.X += (speed * dt);
                        break;
                    }
                case Direction.top:
                    {
                        pos.Y -= (speed * dt);
                        break;
                    }
            }
        }

        public GameObj Fire()
        {
            return new GameObj(new PointF(pos.X + Size.Width / 2, pos.Y + Size.Height / 2), new Size(25, 15), speed * 200 / 100, Direction,Game.bulletSprite);
        }

        public static Image RotateImage(Image img, RotateFlipType flipType)
        {
            var bmp = new Bitmap(img);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.DrawImage(img, 0, 0, img.Width, img.Height);
            }

            bmp.RotateFlip(flipType);

            return bmp;
        }
    }
}
