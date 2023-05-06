using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PingPong
{
    public delegate void ScoreHandler(Racket.RacketSide side);
    public delegate void SmallBoundsCollideHandler();
    public class Ball
    {
        double xSpeed, ySpeed, rotateAngle, curX, curY, tempXS, tempYS;
        bool isAccelerating,invincible;
        const double d = 50, startSpeed = 4, startX = 425, startY = 276,maxSpeed = Racket.w/3-1;

        const string source = "images/ball.png"; 
        MediaPlayer mediaPlayer;
        public Image image;
        Canvas playground;

        ScoreHandler scoreHandler;
        SmallBoundsCollideHandler collideHandler;
        (Racket, Racket) rackets;
        Task draw;

        public (double, double) Location { get => (curX, curY); }
        public double Speed { get => Math.Abs(xSpeed); }

        public Ball(Racket racket1, Racket racket2, ScoreHandler scoreHandler,SmallBoundsCollideHandler collideHandler, Canvas Playground, bool isAccelerating)
        {
            rackets = (racket1.Side == Racket.RacketSide.Left) ? (racket1, racket2) : (racket2, racket1);
            
            mediaPlayer = new();
            mediaPlayer.Open(Canvas.GetZIndex(Playground) == 2 ? new Uri("Sounds/Reflect.mp3", UriKind.Relative) : null);
            mediaPlayer.MediaEnded += (sender, e) => { mediaPlayer.Stop(); };

            (curX, curY) = (startX, startY);
            (xSpeed, ySpeed) = (startSpeed, startSpeed);
            playground = Playground;
            this.isAccelerating = isAccelerating;
            invincible = false;
            this.scoreHandler = scoreHandler;
            this.collideHandler = collideHandler;

            draw = Draw();
            ReflectAndCollide();
        }
        private async Task Draw()
        {
            image = new() { Width = d, Height = d };
            image.Source = new BitmapImage(new Uri(source, UriKind.Relative));
            playground.Children.Add(image);
            Canvas.SetZIndex(image, 3);
            while (xSpeed != 0 && ySpeed != 0)
            {
                image.RenderTransform = new RotateTransform(rotateAngle += Math.Abs(xSpeed), d / 2, d / 2);
                Canvas.SetLeft(image, curX += xSpeed);
                Canvas.SetTop(image, curY += ySpeed);
                await Task.Delay(5);
            }
        }
        public async Task ShowScored()
        {
            MediaPlayer player = new(); player.Open(new Uri("Sounds/TimerCur.wav", UriKind.Relative));
            for (int i = 0; i < 6; i++)
            {
                if (i % 2 != 0)
                    image.Visibility = Visibility.Hidden;
                else
                {
                    image.Visibility = Visibility.Visible;
                    player.Position = TimeSpan.Zero; player.Play();
                }
                await Task.Delay(400);
            }
        }
        private async void ReflectAndCollide()
        {
            while (xSpeed != 0 && ySpeed != 0)
            {
                double distanceX_toLeft = Math.Abs(curX - 15 - Racket.w / 2), distanceX_toRight = Math.Abs(curX+d-860+Racket.w/2);
                double distanceY_toLeftTop = curY + d - rackets.Item1.Top, distanceY_toLeftBottom = curY - rackets.Item1.Top - Racket.h;
                double distanceY_toRightTop = curY + d - rackets.Item2.Top, distanceY_toRightBottom = curY - rackets.Item2.Top - Racket.h;
                double delta = 2*Speed;
                if (curX <= 0 || curX + d >= 875)//пропущен
                {
                    (xSpeed, ySpeed) = (0, 0);
                    await draw;
                    scoreHandler(curX <= 0 ? Racket.RacketSide.Right : Racket.RacketSide.Left);
                    break;

                }
                //
                else if (((distanceX_toLeft <= Racket.w/3 || curX+d<=15+Racket.w/2)  && distanceY_toLeftTop <= delta && distanceY_toLeftTop>=0) || (distanceX_toLeft<=Racket.w/6 && distanceY_toLeftBottom <= 0 && distanceY_toLeftBottom>=-delta) && !invincible)//отбит боком левой
                {
                    ySpeed = (distanceY_toLeftTop <= delta) ? -Math.Abs(ySpeed) : Math.Abs(ySpeed);
                    collideHandler();
                    mediaPlayer.Play();
                }
                else if (((distanceX_toRight <= Racket.w / 3 || curX >= 860-Racket.w/2) && distanceY_toRightTop <= delta && distanceY_toRightTop>=0) || (distanceX_toRight <= Racket.w / 6 && distanceY_toRightBottom >= -delta && distanceY_toRightBottom<=0) && !invincible)//отбит боком правой
                {
                    ySpeed = (distanceY_toRightTop <= delta) ? -Math.Abs(ySpeed) : Math.Abs(ySpeed);
                    collideHandler();
                    mediaPlayer.Play();

                    invincible = true;
                    _ = Task.Run(new Action(async () => { await Task.Delay((int)Math.Ceiling(maxSpeed)); invincible = false; }));
                }
                else if ((distanceX_toLeft <= Speed && curY + d > rackets.Item1.Top && curY < rackets.Item1.Top + Racket.h) || 
                (distanceX_toRight <= Speed && curY + d > rackets.Item2.Top && curY < rackets.Item2.Top + Racket.h) && !invincible)//отражен основными гранями
                {
                    mediaPlayer.Play();
                    if (isAccelerating && Speed < maxSpeed)
                    {
                        xSpeed += Math.CopySign(0.1, xSpeed);
                        ySpeed += Math.CopySign(0.1, ySpeed);
                    }
                    xSpeed = (curX < 400) ? xSpeed = Math.Abs(xSpeed) : xSpeed = -Math.Abs(xSpeed);

                    invincible = true;
                    _ = Task.Run(new Action(async () => { await Task.Delay((int)Math.Ceiling(maxSpeed)); invincible = false; }));
                }
                if (curY <= 0 || curY + d >= 630)//отражение по вертикалм
                    ySpeed = curY <= 0 ? Math.Abs(ySpeed) : -Math.Abs(ySpeed);
                await Task.Delay(1);
            }
        }
        public void Stop()
        {
            (tempXS, tempYS) = (xSpeed, ySpeed);
            (xSpeed, ySpeed) = (0, 0);
        }
        public void Resume()
        {
            (xSpeed, ySpeed) = (tempXS, tempYS);
            playground.Children.Remove(image);

            draw = Draw();
            ReflectAndCollide();
        }
        internal void Reset()
        {
            playground.Children.Remove(image);
            Random random = new();
            (xSpeed, ySpeed) = (Math.Pow(-1, random.Next(0, 2)) * startSpeed, Math.Pow(-1, random.Next(0, 2)) * startSpeed);
            (curX, curY) = (startX, startY);

            draw = Draw();
            ReflectAndCollide();
        }
        public void Dispose()
        {
            playground.Children.Remove(image);
            (xSpeed, ySpeed) = (0, 0);
        }
    }
}
