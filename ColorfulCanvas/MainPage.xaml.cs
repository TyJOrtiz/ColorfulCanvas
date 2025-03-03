using ColorThiefDotNet;
using CommunityToolkit.WinUI.Animations;
using CommunityToolkit.WinUI.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Color = Windows.UI.Color;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ColorfulCanvas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private CoreApplicationViewTitleBar _coreTitleBar;
        public ObservableCollection<Preset> Colors = new ObservableCollection<Preset>()
        {
            new Preset
            {
                Name = "Ocean",
                Source = new List<Color>()
                {
                    Color.FromArgb(255, 0, 144, 150),
                    Color.FromArgb(255, 17, 179, 171),
                    Color.FromArgb(255, 127, 193, 184),
                    Color.FromArgb(255, 224, 244, 233)
                }
            },
            new Preset
            {
                Name = "Purple",
                Source = new List<Color>()
                {
                    Color.FromArgb(255, 25, 13, 121),
                    Color.FromArgb(255, 93, 32, 156),
                    Color.FromArgb(255, 143, 87, 187),
                    Color.FromArgb(255, 231, 108, 222)
                }
            },
            new Preset
            {
                Name = "Blue",
                Source = new List<Color>()
                {
                    Color.FromArgb(255, 2, 63, 138),
                    Color.FromArgb(255, 0, 151, 200),
                    Color.FromArgb(255, 0, 181, 215),
                    Color.FromArgb(255, 70, 202, 229)
                }
            },
            new Preset
            {
                Name = "Green",
                Source = new List<Color>()
                {
                    Color.FromArgb(255, 26, 78, 118),
                    Color.FromArgb(255, 28, 117, 161),
                    Color.FromArgb(255, 53, 160, 163),
                    Color.FromArgb(255, 120, 201, 145)
                }
            },
            new Preset
            {
                Name = "White",
                Source = new List<Color>()
                {
                    make(255, 255, 255),
                    make(244, 244, 244),
                    make(233, 233, 233),
                    make(222, 222, 222),
                }
            },
            new Preset
            {
                Name = "Black",
                Source = new List<Color>()
                {
                    make(0, 0, 0),
                    make(11, 11, 11),
                    make(22, 22, 22),
                    make(33, 33, 33),
                }
            },
            new Preset
            {
                Name = "Red",
                Source = new List<Color>()
                {
                    Color.FromArgb(255, 121, 19, 31),
                    Color.FromArgb(255, 164, 44, 39),
                    Color.FromArgb(255, 207, 89, 58),
                    Color.FromArgb(255, 176, 65, 61),
                }
            }
        };

        private static Color make(int v1, int v2, int v3)
        {
            return Color.FromArgb(255, (byte)v1, (byte)v2, (byte)v3);
        }

        private Preset selectedPreset;
        private List<Color> source;

        public List<Color> Source
        {
            get
            {
                return source ?? Colors.First().Source;
            }
            set
            {
                if (source != value)
                {
                    source = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Preset SelectedPreset
        {
            get
            {
                if (selectedPreset != null)
                {
                    return selectedPreset;
                }
                else
                {
                    return Colors.First();
                }
            }
            set
            {
                if (value != selectedPreset)
                {
                    selectedPreset = value;
                    this.Source = value.Source;
                    NotifyPropertyChanged();
                }
            }
        }
        public MainPage()
        {
            this.InitializeComponent(); 
            _coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            _coreTitleBar.ExtendViewIntoTitleBar = false;
            this.Loaded += MainPage_Loaded;
            //ComboBox1.SelectionChanged += ComboBox1_SelectionChanged;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ComboBox1.SelectedItem != null)
            {
                ColorView.Source = ((Preset)ComboBox1.SelectedItem).Source;
            }
            ComboBox1.SelectionChanged += ComboBox1_SelectionChanged;
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox1.SelectedItem == null)
            {
                return;
            }
            ColorView.Source = ((Preset)ComboBox1.SelectedItem).Source;
        }
    }
}
