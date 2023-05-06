using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PingPong
{
    class AppearingMenu
    {
        Border border;
        Canvas menu;
        double height, width;
        public static DoubleAnimation GetDoubleAnimation(double from, double to, double duration, bool autoReverse)
        {
            DoubleAnimation animation = new()
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(duration),
                AutoReverse = autoReverse
            };
            return animation;
        }
        public AppearingMenu(Border menuBorder, Canvas menu, double height, double width)
        {
            border = menuBorder;
            this.menu = menu;
            this.height = height;
            this.width = width;
        }
        public UIElementCollection Elements { get => menu.Children; }
        public void Open(EventHandler byCompletion1, EventHandler byCompletion2)
        {
            DoubleAnimation heightUp = GetDoubleAnimation(0, height + 10, 0.15, false);
            border.BeginAnimation(Window.HeightProperty, heightUp);
            heightUp.To = height;
            if (byCompletion1 != null)
                heightUp.Completed += byCompletion1;
            heightUp.Completed += (sender, e) =>
            {
                DoubleAnimation widthUp = GetDoubleAnimation(5, width + 10, 0.15, false);
                border.BeginAnimation(Window.WidthProperty, widthUp);
                widthUp.To = width;
                if (byCompletion2 != null)
                    widthUp.Completed += byCompletion2;
                menu.BeginAnimation(Window.WidthProperty, widthUp);
            };
            menu.BeginAnimation(Window.HeightProperty, heightUp);
            System.Windows.Media.MediaPlayer player = new();
            player.Open(new Uri("./Sounds/MenuOpen.mp3", UriKind.Relative)); player.Play();

        }
        public void Close(EventHandler byCompletion)
        {
            System.Windows.Media.MediaPlayer player = new();
            player.Open(new Uri("Sounds/MenuClose.mp3", UriKind.Relative)); player.Play();
            foreach (UIElement element in menu.Children)
                element.Visibility = Visibility.Hidden;
            DoubleAnimation widthDown = GetDoubleAnimation(border.ActualWidth, 5, 0.15, false);
            border.BeginAnimation(Window.WidthProperty, widthDown);
            widthDown.From = width;
            widthDown.Completed += (sender, e) =>
            {
                DoubleAnimation heightDown = GetDoubleAnimation(border.ActualHeight, 0, 0.15, false);
                border.BeginAnimation(Window.HeightProperty, heightDown);
                heightDown.From = height;
                if (byCompletion != null)
                    heightDown.Completed += byCompletion;
                menu.BeginAnimation(Window.HeightProperty, heightDown);
            };
            menu.BeginAnimation(Window.WidthProperty, widthDown);
        }

    }
}
