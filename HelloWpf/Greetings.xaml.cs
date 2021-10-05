using PexelsDotNetSDK.Api;
using PexelsDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace HelloWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// pexels
    /// 563492ad6f917000010000019a625093f14641ecb876ff0b65cc8627
    public partial class MainWindow : Window
    {
        private int NUMBER_OF_IMAGES = 8;

        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("Component initialized...");
        }

        public class Photos
        {
            public ICollection<Photo> photoList { get; set; }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            PexelsClient _pex = new PexelsClient("563492ad6f917000010000019a625093f14641ecb876ff0b65cc8627");
            //var helper = new WindowInteropHelper(this);
            //var scr = Screen.FromHandle(helper.Handle);
            var width = System.Windows.SystemParameters.PrimaryScreenWidth;
            var height = System.Windows.SystemParameters.PrimaryScreenHeight;
            Debug.WriteLine("Found screen info: H {0}, W {1}", height, width);
            Debug.WriteLine("Button clicked...getting results...");
            PhotoPage result = await _pex.SearchPhotosAsync("nature", null, "medium", null, null, 1, NUMBER_OF_IMAGES);
            Debug.WriteLine("Got result from pexels: {0}", result);
            // var photoPage = JsonConvert.DeserializeObject<PhotoPage>(result);
            int index = 0;
            StackPanel sp;
            foreach (Photo p in result.photos)
            {
                // string id = p.id;
                // string url = (string)p[V];
                // string imageUrl = (string)p["src"]["small"];
                int row = (int)Math.Floor((double)index / 4);
                int col = index % 4;
                Debug.WriteLine("ROW {0}, COL {1}, image: {2}", row, col, p.url);
                sp = new StackPanel();
                sp.SetValue(Grid.RowProperty, row);
                sp.SetValue(Grid.ColumnProperty, col);
                //sp.SetValue(WidthProperty, "Auto");
                //sp.SetValue(HeightProperty, "Auto");
                // 
                // BitmapImage pic = new BitmapImage();
                // pic.BeginInit();
                // pic.UriSource = new Uri(p.source.medium);
                // pic.EndInit();
                // MyListBox.Items.Add(pic);
                // ImagesGrid.Children.Add(pic);
                // Image0.Source = pic;

                Image _pic = new Image();
                //_pic.Width = 200;
                //_pic.Height = 100;
                // L, T, R, B
                //Thickness t;
                //t.Left = (double)200 * (col - 1);
                //t.Top = (double)100 * (row - 1);
                //_pic.Margin = t;
                BitmapImage _myBm = new BitmapImage();
                _myBm.BeginInit();
                _myBm.UriSource = new Uri(p.source.medium);
                _myBm.DecodePixelWidth = 200;
                _myBm.EndInit();
                _pic.Source = _myBm;
                sp.Children.Add(_pic);
                MyGrid.Children.Add(sp);
                index++;
            }
        }
    }
}
