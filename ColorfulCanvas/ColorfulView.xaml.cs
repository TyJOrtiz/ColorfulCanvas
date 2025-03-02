using CommunityToolkit.WinUI.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using ColorThiefDotNet;
using Color = Windows.UI.Color;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Markup;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ColorfulCanvas
{
    [ContentProperty(Name = nameof(Colors))]
    public sealed partial class ColorfulView : UserControl, INotifyPropertyChanged
    {
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            ColorfulView_Loaded(this, null);
        }
        public ColorfulView()
        {
            this.InitializeComponent();
            this.Loaded += ColorfulView_Loaded;
        }
        private int count = 100;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if (count != value)
                {
                    count = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private List<Color> source = new List<Color>();

        public List<Color> Source
        { get { return source; }

            set
            {
                if (value != null && source != value)
                {
                    source = value;
                    Colors = value.ToList();
                    NotifyPropertyChanged();
                }
            }
        }
        public List<Color> Colors { get; set; } = new List<Color>();
        //{

        //    Color.FromArgb(255, (byte)(0.9586862922 * 255), (byte)(0.660125792 * 255), (byte)(0.8447988033 * 255)),
        //    Color.FromArgb(255, (byte)(0.8714533448 * 255), (byte)(0.723166883 * 255), (byte)(0.9342088699 * 255)),
        //    Color.FromArgb(255, (byte)(0.7458761334 * 255), (byte)(0.7851135731 * 255), (byte)(0.9899476171 * 255)),
        //    Color.FromArgb(255, (byte)(0.4398113191 * 255), (byte)(0.8953480721 * 255), (byte)(0.9796616435 * 255)),
        //    Color.FromArgb(255, (byte)(0.3484552801 * 255), (byte)(0.933657825 * 255), (byte)(0.9058339596 * 255)),
        //    Color.FromArgb(255, (byte)(0.5567936897 * 255), (byte)(0.9780793786 * 255), (byte)(0.6893508434 * 255))
        //};
        private List<Storyboard> animations = new List<Storyboard>();
        private void ColorfulView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Colors == null || !Colors.Any())
            { return; }
            //this.Background = new SolidColorBrush(Colors[0]);
            Canvas1.Children.Clear();
            hasher.Clear();
            foreach (var item in animations)
            {
                item.Stop();
            }
            animations.Clear();
            var r = new Random();
            while (Colors.Count < count)
            {
                Colors.AddRange(Colors.OrderBy(x => r.Next()).ToList());
            }
            if (Colors.Count > count)
            {
                while (Colors.Count > count)
                {
                    Colors.RemoveAt(Colors.Count - 1);
                }
            }
            Debug.Assert(Colors.Count == count);
            var list1 = new List<(double, double, double)>();
            while (list1.Count < count)
            {
                var config = Randomize(this.ActualSize);
                if (hasher.Add(config))
                {
                    list1.Add(config);
                }
            }
            foreach (var h in hasher)
            {
                var item = GenerateItem(h, hasher.ToList().IndexOf(h));
                Canvas1.Children.Add(item);
                GenerateAnimation(item);
            }
            this.SizeChanged += ColorfulView_SizeChanged;
        }

        private void ColorfulView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Colors == null || !Colors.Any())
            { return; }
            hasher.Clear();
            foreach (var item in animations)
            {
                item.Stop();
            }
            animations.Clear();
            Canvas1.Children.Clear();
            var r = new Random();
            while (Colors.Count < count)
            {
                Colors.AddRange(Colors.OrderBy(x => r.Next()).ToList());
            }
            if (Colors.Count > count)
            {
                while (Colors.Count > count)
                {
                    Colors.RemoveAt(Colors.Count - 1);
                }
            }
            Debug.Assert(Colors.Count == count);
            var list1 = new List<(double, double, double)>();
            while (list1.Count < count)
            {
                var config = Randomize(this.ActualSize);
                if (hasher.Add(config))
                {
                    list1.Add(config);
                }
            }
            foreach (var h in hasher)
            {
                var item = GenerateItem(h, hasher.ToList().IndexOf(h));
                Canvas1.Children.Add(item);
                GenerateAnimation(item);

            }
        }
        private DispatcherTimer Timer;
        private void GenerateAnimation(UIElement item, DispatcherTimer t = null)
        {
            var ellpise = item as Ellipse;
            Storyboard storyboard = new Storyboard();

            var ransom = new Random();
            var color = Colors[ransom.Next(Colors.Count)];
            var animation = new Windows.UI.Xaml.Media.Animation.ColorAnimation();
            animation.BeginTime = TimeSpan.FromSeconds(0);
            animation.Duration = TimeSpan.FromSeconds(5);
            animation.EnableDependentAnimation = true;
            animation.From = (ellpise.Fill as SolidColorBrush).Color;
            animation.To = color;
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, ellpise);
            Storyboard.SetTargetProperty(animation, "(Ellipse.Fill).(SolidColorBrush.Color)");
            animation.Completed += (a, c) =>
            {
                animation = null;
                storyboard.Stop();
                ellpise.Fill = new SolidColorBrush(color);
                animations.Remove(storyboard);
                storyboard = null;
                GenerateAnimation(item);
            };
           animations.Add(storyboard);
            storyboard.Begin();
        }

        private UIElement GenerateItem((double, double, double) h, int v)
        {
            var ellipse = new Ellipse
            {
                Width = h.Item1,
                Height = h.Item1,
                Fill = new SolidColorBrush(Colors[v]),
            };
            Canvas.SetLeft(ellipse, (this.ActualWidth / 2) + h.Item2 - (ellipse.Width / 2));
            Canvas.SetTop(ellipse, (this.ActualHeight / 2) + h.Item3 - (ellipse.Width / 2));
            return ellipse;
        }

        private (double, double, double) Randomize(Vector2 actualSize)
        {
            double diameter = 0;
            double offsetX = 0;
            double offsetY = 0;
            var decision = (actualSize.X + actualSize.Y) / 4;
            var andom = new Random();
            diameter = NextDouble(andom, decision * .25, decision * .75);
            offsetX = NextDouble(andom, -(actualSize.X / 2), (actualSize.X / 2));
            offsetY = NextDouble(andom, -(actualSize.Y / 2), (actualSize.Y / 2)); 
            return (diameter, offsetX, offsetY);
        }

        public double NextDouble(Random RandGenerator, double MinValue, double MaxValue)
        {
            return RandGenerator.NextDouble() * (MaxValue - MinValue) + MinValue;
        }
        public HashSet<(double, double, double)> hasher = new System.Collections.Generic.HashSet<(double, double, double)>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ((Grid)sender).Background = new BackdropBlurBrush
            {
                Amount = 100,
                Opacity = 1,
                FallbackColor = Windows.UI.Colors.Transparent
            };
        }
    }
}
