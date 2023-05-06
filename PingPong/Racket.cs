using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PingPong
{
    public class Racket
    {
        public enum RacketSide { Left, Right }
        public const int h = 194, w = 82, speed = 25;
        public Key[] controls = null;

        const string sourceLeft = "images/redRacket.png", sourceRight = "images/blueRacket.png";
        public Image image;

        public double Top { get; set; }
        public RacketSide Side { get; private set; }
        public Racket(RacketSide side)
        {
            Side = side;
            image = new();
            ImageSourceConverter converter = new();
            if (Side == RacketSide.Left)
            {
                image.Source = new BitmapImage(new Uri(sourceLeft, UriKind.Relative));
                controls = new[] { Key.W, Key.S };
            }
            else
            {
                image.Source = new BitmapImage(new Uri(sourceRight, UriKind.Relative));
                controls = new[] { Key.Up, Key.Down };
            }
            (Top, image.Height, image.Width) = (200, h, w);
        }
        public void Move(bool Up)
        {
            if (Up)
            {
                if ((Top -= speed) <= 0)
                    Top = 0;
            }
            else if ((Top += speed) >= 635 - h)
                Top = 635 - h;
            Canvas.SetTop(image, Top);
        }
        public void Draw(Canvas Playground)
        {
            Playground.Children.Add(image);
            if (Side == RacketSide.Left)
                Canvas.SetLeft(image, 20);
            else
                Canvas.SetLeft(image, 780);
            Canvas.SetTop(image, Top);
        }
        public async void LaunchAI(Ball ball)
        {
            while (ball.Speed != 0)
            {
                (double x, double y) = ball.Location;
                double distance = Math.Abs(Canvas.GetLeft(image) - x);
                if (distance <= 400)
                    Move(Top + h / 2 > y);
                int delay = (int)((distance + 5) * 2.5 / ball.Speed);
                await Task.Delay(delay > 75 ? delay : 75);
            }
        }
        public void Reset()
        {
            image.Visibility = System.Windows.Visibility.Visible;
            Top = 200;
            Canvas.SetTop(image, Top);
        }
    }
}
