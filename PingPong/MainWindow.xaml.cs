using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PingPong
{
    public partial class MainWindow : Window
    {
        const int winScore = 11;
        bool keyBlock1 = false, keyBlock2 = false;
        int seconds;
        Ball ball;
        bool vsAi; bool? accelerate;
        Task movingP1, movingP2;
        Racket racketP1, racketP2, AI_racket1, AI_racket2;
        AppearingMenu mainMenu, pauseMenu, scoreMenu;
        MediaPlayer clickPlayer = new();
        public MainWindow()
        {
            InitializeComponent();
            AnimateTitle();
            clickPlayer.Open(new Uri("Sounds/Click.wav", UriKind.Relative));
            mainMenu = new(mainMenuBorder, mainMenuCanv, 400, 600);
            pauseMenu = new(pauseMenuBorder, pauseMenuCanv, 300, 400);
            scoreMenu = new(scoreMenuBorder, scoreMenuCanv, 400, 600);
            NoButton.Click += (sender, e) => { NoButton.Click -= ReturnToMenu; mainMenu.Close(null); };
            PlayVs2PButton.Click += (sender, e) => { vsAi = false; GetGameMode(); };
            PlayVsAiButton.Click += (sender, e) => { vsAi = true; GetGameMode(); };
            SM_LowerButton.Click += (sender, e) => { clickPlayer.Position = TimeSpan.Zero; clickPlayer.Play(); pauseMenu.Close(null); };
            SM_UpperButton.Click += (sender, e) => { clickPlayer.Position = TimeSpan.Zero; clickPlayer.Play(); pauseMenu.Close(null); };
            this.KeyDown += KeyPressed;
            LaunchBG_AI();
        }
        private void AnimateTitle()
        {
            MainLabel.RenderTransform = new RotateTransform(0);
            MainLabel.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation()
            {
                AutoReverse = true,
                Duration = TimeSpan.FromSeconds(3),
                RepeatBehavior = RepeatBehavior.Forever,
                From = -5,
                To = 5
            });
            MainLabel.BeginAnimation(FontSizeProperty, new DoubleAnimation() { AutoReverse = true, Duration = TimeSpan.FromSeconds(1.5), RepeatBehavior = RepeatBehavior.Forever, From = MainLabel.FontSize, To = 85 });
        }
        private void InDark(EventHandler after1, bool reverse, EventHandler after2)
        {
            Canvas darken = new() { Width = 900, Height = 675, Background = Brushes.Black };
            darken.SetValue(Canvas.ZIndexProperty, 3);
            Field.Children.Add(darken);
            DoubleAnimation firstHalf = AppearingMenu.GetDoubleAnimation(0, 1.0, 1.0, false);
            if (after1 != null)
                firstHalf.Completed += after1;
            firstHalf.Completed += (sender, e) =>
            {
                if (reverse)
                {
                    DoubleAnimation secondhalf = AppearingMenu.GetDoubleAnimation(1.0, 0, 1.0, false);
                    if (after2 != null)
                        secondhalf.Completed += after2;
                    secondhalf.Completed += (sender, e) => { Field.Children.Remove(darken); darken = null; };
                    darken.BeginAnimation(OpacityProperty, secondhalf);
                }
            };
            darken.Visibility = Visibility.Visible;
            darken.BeginAnimation(OpacityProperty, firstHalf);
        }
        private void LaunchBG_AI()
        {
            AI_racket1 = new(Racket.RacketSide.Left);
            AI_racket2 = new(Racket.RacketSide.Right);
            ball = new(AI_racket1, AI_racket2, (side) => { ball.Reset(); if (side == Racket.RacketSide.Left) AI_racket2.LaunchAI(ball); else AI_racket1.LaunchAI(ball); }, () => { }, AI_PlayGround, true);
            AI_racket1.Draw(AI_PlayGround);
            AI_racket2.Draw(AI_PlayGround);
            AI_racket1.LaunchAI(ball);
            AI_racket2.LaunchAI(ball);
        }
        private void DestroyBG_AI()
        {
            ball.Dispose();
            AI_PlayGround.Children.Remove(AI_racket1.image);
            AI_PlayGround.Children.Remove(AI_racket2.image);
            (AI_racket1, AI_racket2, ball) = (null, null, null);
        }
        public async void Scored(Racket.RacketSide side)
        {
            keyBlock1 = false;
            ShadowedPanel.Opacity = 0.7;
            racketP1.image.Visibility = Visibility.Hidden;
            racketP2.image.Visibility = Visibility.Hidden;
            clickPlayer.Position = TimeSpan.Zero; clickPlayer.Play();
            Canvas.SetZIndex(PlayGround, 2);
            await ball.ShowScored();
            scoreMenu.Open(null, async (sender, e) =>
            {
                foreach (UIElement element in scoreMenu.Elements)
                    element.Visibility = Visibility.Visible;
                DoubleAnimation changeScore = AppearingMenu.GetDoubleAnimation(LeftSideScore.FontSize, 200, 0.2, true);
                changeScore.Completed += ResetGame;
                await Task.Delay(1500);
                MediaPlayer player = new(); player.Open(new Uri("Sounds/TimerEnd.wav", UriKind.Relative));
                if (side == Racket.RacketSide.Left)
                {
                    LeftSideScore.Content = int.Parse(LeftSideScore.Content.ToString()) + 1;
                    player.Play();
                    LeftSideScore.BeginAnimation(FontSizeProperty, changeScore);
                }
                else
                {
                    RightSideScore.Content = int.Parse(RightSideScore.Content.ToString()) + 1;
                    player.Play();
                    RightSideScore.BeginAnimation(FontSizeProperty, changeScore);
                }
            });
        }

        private async void ResetGame(object sender, EventArgs e)
        {
            await Task.Delay(1500);
            scoreMenu.Close((sender, e) =>
            {
                if (int.Parse(LeftSideScore.Content.ToString()) == winScore || int.Parse(RightSideScore.Content.ToString()) == winScore)
                {
                    MediaPlayer player = new();
                    keyBlock1 = false;
                    EventHandler completed = (sender, e) =>
                    {
                        WinLabel.Visibility = Visibility.Hidden; LoseLabel.Visibility = Visibility.Hidden; Lose.Visibility = Visibility.Collapsed; win_GIF.Visibility = Visibility.Collapsed;
                        win_SparklesGif.Visibility = Visibility.Collapsed; win_SparklesGif2.Visibility = Visibility.Collapsed; sadGif.Visibility = Visibility.Collapsed; OpenSmallMenu();
                    };
                    if ((LeftSideName.Content.ToString() == "ИИ" && int.Parse(LeftSideScore.Content.ToString()) == winScore) || (RightSideName.Content.ToString() == "ИИ" && int.Parse(RightSideScore.Content.ToString()) == winScore))
                    {
                        player.Open(new Uri("Sounds/Lose.mp3", UriKind.Relative));
                        LoseLabel.Visibility = Visibility.Visible;
                        ColorAnimation animation = new() { From = Colors.Red, To = Colors.DarkRed, RepeatBehavior = new(3), AutoReverse = true, Duration = TimeSpan.FromSeconds(2) };
                        animation.Completed += completed;
                        LoseLabel.Foreground = new SolidColorBrush();
                        LoseLabel.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                        Lose.Visibility = Visibility.Visible;
                        Lose.Play();
                        sadGif.Visibility = Visibility.Visible;
                        WpfAnimatedGif.ImageBehavior.GetAnimationController(sadGif).Play();
                    }
                    else
                    {
                        player.Open(new Uri("Sounds/Win.mp3", UriKind.Relative));
                        foreach (Image gif in new Image[] { win_GIF, win_SparklesGif, win_SparklesGif2 })
                        {
                            gif.Visibility = Visibility.Visible;
                            WpfAnimatedGif.ImageBehavior.GetAnimationController(gif).Play();
                        }
                        if (vsAi)
                            WinLabel.Content = "Вы выиграли! :)";
                        else if (int.Parse(LeftSideScore.Content.ToString()) == winScore)
                            WinLabel.Content = "И1 выиграл! :)";
                        else
                            WinLabel.Content = "И2 выиграл! :)";

                        WinLabel.Visibility = Visibility.Visible;
                        WinLabel.RenderTransform = new RotateTransform(0);
                        WinLabel.RenderTransformOrigin = new Point(0.5, 0.5);
                        DoubleAnimation animation = new DoubleAnimation() { From = -15, To = 15, Duration = TimeSpan.FromSeconds(2), RepeatBehavior = new RepeatBehavior(3), AutoReverse = true };
                        animation.Completed += completed;
                        WinLabel.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
                        WinLabel.BeginAnimation(FontSizeProperty, new DoubleAnimation() { AutoReverse = true, RepeatBehavior = new RepeatBehavior(3), Duration = TimeSpan.FromSeconds(2), From = WinLabel.FontSize, To = 90 });
                    }
                    player.Play();
                    P1ControlsImage.Source = new BitmapImage(new Uri("Images/WS3.png", UriKind.Relative));
                    P2ControlsImage.Source = new BitmapImage(new Uri("Images/Arrows3.png", UriKind.Relative));
                }
                else
                {
                    keyBlock1 = true;
                    ShadowedPanel.Opacity = 0;
                    ball.Reset();
                    racketP1.Reset();
                    racketP2.Reset();
                    if (vsAi)
                        racketP2.LaunchAI(ball);
                }
            });
        }

        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Position = TimeSpan.FromSeconds(0.1);
        }

        private void StartGame()
        {
            InDark((sender, e) =>
           {
               if (AI_racket1 != null)
                   DestroyBG_AI();
               if (vsAi)
               {
                   AI_GIF.Visibility = Visibility.Visible;
                   WpfAnimatedGif.ImageBehavior.GetAnimationController(AI_GIF).Play();
                   if (racketP1.Side == Racket.RacketSide.Left)
                   {
                       Canvas.SetLeft(P1ControlsImage, 133.0);
                       Canvas.SetLeft(AI_GIF, 679.0);
                   }
                   else
                   {
                       Canvas.SetLeft(P1ControlsImage, 679.0);
                       Canvas.SetLeft(AI_GIF, 133.0);
                   }
               }
               else
               {
                   racketP1 = new Racket(Racket.RacketSide.Left);
                   racketP2 = new Racket(Racket.RacketSide.Right);
                   Canvas.SetLeft(P1ControlsImage, 133.0);
                   Canvas.SetLeft(P2ControlsImage, 679.0);
               }
               racketP1.Draw(PlayGround); racketP2.Draw(PlayGround);
               TimerLabel.Content = seconds = 5;
               (LeftSideScore.Content, RightSideScore.Content) = (0, 0);
               GameInfo.Content = $"Приготовьтесь!\n   Играем до {winScore}\n\n   Пауза - Esc";
               foreach (UIElement element in vsAi ? new UIElement[] { P1ControlsImage, TimerLabel, GameInfo } : new UIElement[] { P1ControlsImage, P2ControlsImage, TimerLabel, GameInfo })
               {
                   element.BeginAnimation(OpacityProperty, AppearingMenu.GetDoubleAnimation(0, 1.0, 0.2, false));
                   element.Visibility = Visibility.Visible;
               }
               DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
               timer.Tick += Countdown;
               ShadowedPanel.Opacity = 0;
               timer.Start();
           }, true, null);
        }
        private void Countdown(object sender, EventArgs e)
        {
            MediaPlayer player = new();
            if (--seconds > 0)
            {
                player.Position = TimeSpan.Zero;
                player.Open(new Uri("Sounds/TimerCur.wav", UriKind.Relative));
                DoubleAnimation tick = new();
                tick.AutoReverse = true;
                tick.From = TimerLabel.FontSize;
                tick.To = 240;
                tick.Duration = TimeSpan.FromSeconds(0.25);
                tick.Completed += (sender, e) => { TimerLabel.Content = seconds; player.Play(); };
                ImageSourceConverter converter = new();
                if (seconds > 2)
                    tick.Completed += async (sender, e) =>
                    {
                        P1ControlsImage.SetValue(Image.SourceProperty, converter.ConvertFromString(P1ControlsImage.Source.ToString()[..^5] + $"{seconds - 2}.png"));
                        racketP1.Move(seconds % 2 == 1);
                        if (!vsAi)
                            P2ControlsImage.SetValue(Image.SourceProperty, converter.ConvertFromString(P2ControlsImage.Source.ToString()[..^5] + $"{seconds - 2}.png"));
                        for (int i = 0; i < 5; i++)
                        {
                            racketP1.Move(seconds % 2 == 0);
                            if (!vsAi)
                                racketP2.Move(seconds % 2 == 0);
                            await Task.Delay(100);
                        }
                    };
                else
                {
                    tick.Completed += (sender, e) =>
                    {
                        P1ControlsImage.SetValue(Image.SourceProperty, converter.ConvertFromString(P1ControlsImage.Source.ToString()[..^5] + "3.png"));
                        if (!vsAi) P2ControlsImage.SetValue(Image.SourceProperty, converter.ConvertFromString(P2ControlsImage.Source.ToString()[..^5] + "3.png"));
                    };
                    keyBlock1 = true;
                }
                TimerLabel.BeginAnimation(FontSizeProperty, tick);
            }
            else
            {
                MediaPlayer endPlayer = new(); endPlayer.Open(new Uri("Sounds/TimerEnd.wav", UriKind.Relative));
                endPlayer.Play();
                foreach (UIElement element in vsAi ? new UIElement[] { P1ControlsImage, AI_GIF, TimerLabel, GameInfo } : new UIElement[] { P1ControlsImage, P2ControlsImage, TimerLabel, GameInfo })
                {
                    element.BeginAnimation(OpacityProperty, AppearingMenu.GetDoubleAnimation(1.0, 0, 0.5, false));
                    element.Visibility = Visibility.Collapsed;
                }
                ball = new(racketP1, racketP2, Scored, () => { keyBlock1 = false; }, PlayGround, (bool)accelerate);
                if (vsAi)
                    racketP2.LaunchAI(ball);
                (sender as DispatcherTimer).Stop();
            }
        }
        private void GetGameMode()
        {
            if (vsAi)
            {
                PlayVs2PButton.Visibility = Visibility.Hidden;
                PlayVsAiButton.Visibility = Visibility.Hidden;
                mainMenu.Close((sender, e) =>
                {
                    mainMenu.Open(null, (sender, e) =>
                    {
                        mainMenuContent.Content = "Выберите сторону:";
                        Canvas.SetLeft(mainMenuContent, 90.0);
                        mainMenuContent.Visibility = Visibility.Visible;
                        LeftSideButton.Visibility = Visibility.Visible;
                        RightSideButton.Visibility = Visibility.Visible;
                        NoButton.Visibility = Visibility.Visible;
                    });
                });
            }
            else
            {
                NoButton.Click -= ReturnToMenu;
                LeftSideName.Content = "И1";
                RightSideName.Content = "И2";
                mainMenu.Close(null);
                OpenSmallMenu();
            }
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            (PlayButton.IsEnabled, AuthorInfoButton.IsEnabled, ExitButton.IsEnabled) = (false, false, false);
            MainLabel.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Hidden;
            AuthorInfoButton.Visibility = Visibility.Hidden;
            ExitButton.Visibility = Visibility.Hidden;
            accelerate = null;
            mainMenu.Open(null, (sender, e) =>
             {
                 (PlayButton.IsEnabled, AuthorInfoButton.IsEnabled, ExitButton.IsEnabled) = (true, true, true);
                 mainMenuContent.Visibility = Visibility.Visible;
                 mainMenuContent.Content = "Выберите режим игры:"; mainMenuContent.SetValue(Canvas.LeftProperty, 50.0); mainMenuContent.SetValue(Canvas.TopProperty, 25.0);
                 NoButton.SetValue(Canvas.LeftProperty, 200.0); NoButton.Content = "Назад"; NoButton.Visibility = Visibility.Visible; NoButton.Click += ReturnToMenu;
                 PlayVsAiButton.Visibility = Visibility.Visible;
                 PlayVs2PButton.Visibility = Visibility.Visible;
             });
        }
        private void LeftSideButton_Click(object sender, RoutedEventArgs e)
        {
            racketP1 = new(Racket.RacketSide.Left);
            racketP2 = new(Racket.RacketSide.Right);
            P1ControlsImage.Source = new BitmapImage(new Uri("Images/WS3.png", UriKind.Relative));
            LeftSideName.Content = "И1";
            RightSideName.Content = "ИИ";
            mainMenu.Close(null);
            OpenSmallMenu();
        }
        private void RightSideButton_Click(object sender, RoutedEventArgs e)
        {
            racketP1 = new(Racket.RacketSide.Right);
            racketP2 = new(Racket.RacketSide.Left);
            P1ControlsImage.Source = new BitmapImage(new Uri("Images/Arrows3.png", UriKind.Relative));
            LeftSideName.Content = "ИИ";
            RightSideName.Content = "И1";
            mainMenu.Close(null);
            OpenSmallMenu();
        }
        private void OpenSmallMenu()
        {
            RoutedEventHandler upClick = null, downClick = null;
            pauseMenu.Open((sender, e) =>
            {
                if (keyBlock1)
                {
                    ShadowedPanel.Opacity = 0.7;
                    Canvas.SetZIndex(PlayGround, 0);
                    pauseMenuContent.Content = "Пауза"; Canvas.SetLeft(pauseMenuContent, 130);
                    SM_UpperButton.Content = "Продолжить"; SM_UpperButton.Width = 231; Canvas.SetLeft(SM_UpperButton, 85);
                    upClick = (sender, e) => { ball.Resume(); keyBlock2 = false; ShadowedPanel.Opacity = 0; if (vsAi) racketP2.LaunchAI(ball); Canvas.SetZIndex(PlayGround, 2); };
                    downClick = ReturnToMenu; downClick += (sender, e) => { keyBlock2 = false; Canvas.SetZIndex(PlayGround, 2); };
                    keyBlock2 = true; ball.Stop();
                    SM_LowerButton.Content = "Назад к меню"; Canvas.SetLeft(SM_LowerButton, 80); SM_LowerButton.Width = 249;
                }
                else if (accelerate != null)
                {
                    upClick = PlayButton_Click;
                    downClick = ReturnToMenu;
                    SM_UpperButton.Content = "Да"; SM_LowerButton.Content = "Нет";
                    SM_UpperButton.Width = 99; Canvas.SetLeft(SM_LowerButton, 151);
                    Canvas.SetLeft(SM_UpperButton, 151); SM_LowerButton.Width = 99;
                    pauseMenuContent.Content = "Играть заново?"; Canvas.SetLeft(pauseMenuContent, 40);
                }
                else
                {
                    SM_UpperButton.Content = "Да"; SM_LowerButton.Content = "Нет"; pauseMenuContent.Content = "Ускорение\n   мячика?"; Canvas.SetLeft(pauseMenuContent, 80);
                    upClick = (sender, e) => { accelerate = true; StartGame(); };
                    downClick = (sender, e) => { accelerate = false; StartGame(); };
                }
            },
            (sender, e) =>
            {
                foreach (UIElement element in pauseMenu.Elements)
                    element.Visibility = Visibility.Visible;
                SM_UpperButton.Click += upClick; SM_UpperButton.Click += (sender, e) => { SM_UpperButton.Click -= upClick; SM_LowerButton.Click -= downClick; };
                SM_LowerButton.Click += downClick; SM_LowerButton.Click += (sender, e) => { SM_UpperButton.Click -= upClick; SM_LowerButton.Click -= downClick; };
            });
        }
        private void ReturnToMenu(object sender, RoutedEventArgs e)
        {
            if (keyBlock1)
            {
                PlayGround.Children.Remove(racketP1.image);
                PlayGround.Children.Remove(racketP2.image);
                PlayGround.Children.Remove(ball.image);
                keyBlock1 = false;
            }
            InDark((sender, e) =>
            {
                MainLabel.Visibility = Visibility.Visible;
                PlayButton.Visibility = Visibility.Visible;
                AuthorInfoButton.Visibility = Visibility.Visible;
                ShadowedPanel.Visibility = Visibility.Visible;
                ExitButton.Visibility = Visibility.Visible;
                if (AI_racket1 == null)
                    LaunchBG_AI();
            }, true, null);
        }
        private void KeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.IsRepeat)
                return;
            if (e.Key == System.Windows.Input.Key.Escape && keyBlock1 && !keyBlock2)
                OpenSmallMenu();
            else if (e.Key == System.Windows.Input.Key.Escape && keyBlock2)
                SM_UpperButton.RaiseEvent(new(Button.ClickEvent));

            if (racketP1 != null && racketP1.controls != null && racketP1.controls.Contains(e.Key) && keyBlock1 && !keyBlock2 && (movingP1 == null || movingP1.Status != TaskStatus.Running))
                movingP1 = Task.Run(new Action(async () => { while (e.IsDown && keyBlock1) { racketP1.image.Dispatcher.Invoke(new(() => { racketP1.Move(e.Key == racketP1.controls[0]); })); await Task.Delay(75); } }));
            else if (racketP2 != null && racketP2.controls != null && racketP2.controls.Contains(e.Key) && keyBlock1 && !keyBlock2 && !vsAi && (movingP2 == null || movingP2.Status != TaskStatus.Running))
                movingP2 = Task.Run(new Action(async () => { while (e.IsDown && keyBlock1) { racketP2.image.Dispatcher.Invoke(new(() => { racketP2.Move(e.Key == racketP2.controls[0]); })); await Task.Delay(75); } }));
        }

        private void AuthorInfoButton_Click(object sender, RoutedEventArgs e)
        {
            (PlayButton.IsEnabled, AuthorInfoButton.IsEnabled, ExitButton.IsEnabled) = (false, false, false);
            mainMenu.Open(null, (sender, e) =>
            {
                mainMenuContent.Visibility = Visibility.Visible; NoButton.SetValue(Canvas.LeftProperty, 200.0); NoButton.Content = "OK"; NoButton.Visibility = Visibility.Visible;
                mainMenuContent.SetValue(Canvas.LeftProperty, 0.0); mainMenuContent.Content = "Основы программирования.\nДополнительное задание:\nПинг-понг.\nВыполнил Сазонников А.В.\n6102-020302D";
                mainMenuContent.SetValue(Canvas.TopProperty, 50.0); (PlayButton.IsEnabled, AuthorInfoButton.IsEnabled, ExitButton.IsEnabled) = (true, true, true);
            });
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            (PlayButton.IsEnabled, AuthorInfoButton.IsEnabled, ExitButton.IsEnabled) = (false, false, false);
            if (mainMenuCanv.ActualHeight > 0)
                mainMenu.Close((sender, e) =>
                {
                    mainMenu.Open(null, (sender, e) =>
                    {
                        mainMenuContent.Visibility = Visibility.Visible;
                        mainMenuContent.Content = "Вы действительно хотите\n\t      выйти?"; mainMenuContent.SetValue(Canvas.LeftProperty, 0.0); mainMenuContent.SetValue(Canvas.TopProperty, 80.0);
                        NoButton.SetValue(Canvas.LeftProperty, 339.0); NoButton.Content = "Нет"; NoButton.Visibility = Visibility.Visible; YesButton.Click += (sender, e) => Close();
                        YesButton.Visibility = Visibility.Visible; NoButton.Click += (sender, e) => YesButton.Click -= (sender, e) => Close();
                        (PlayButton.IsEnabled, AuthorInfoButton.IsEnabled, ExitButton.IsEnabled) = (true, true, true);
                    });
                });
            else
                mainMenu.Open(null, (sender, e) =>
                {
                    mainMenuContent.Visibility = Visibility.Visible;
                    mainMenuContent.Content = "Вы действительно хотите\n\t      выйти?"; mainMenuContent.SetValue(Canvas.LeftProperty, 0.0); mainMenuContent.SetValue(Canvas.TopProperty, 80.0);
                    NoButton.SetValue(Canvas.LeftProperty, 339.0); NoButton.Content = "Нет"; NoButton.Visibility = Visibility.Visible; YesButton.Click += (sender, e) => Close();
                    YesButton.Visibility = Visibility.Visible; (PlayButton.IsEnabled, AuthorInfoButton.IsEnabled, ExitButton.IsEnabled) = (true, true, true);
                    NoButton.Click += (sender, e) => YesButton.Click -= (sender, e) => Close();
                });
        }
    }
}
