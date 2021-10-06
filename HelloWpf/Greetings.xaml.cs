using PexelsDotNetSDK.Api;
using PexelsDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
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
            MyGrid.Children.Clear();
            ErrorHolder.Text = "";

            PexelsClient _pex = new PexelsClient("563492ad6f917000010000019a625093f14641ecb876ff0b65cc8627");
            string query = TitleText.Text + TextText.Text;
            Debug.WriteLine("Button clicked...getting results... {0}", query);
            PhotoPage result = await _pex.SearchPhotosAsync(query, null, "medium", null, null, 1, NUMBER_OF_IMAGES);
            Debug.WriteLine("Got result from pexels: {0} - length: {1}", result, result.photos.Count);
            StackPanel sp;

            if (result.photos.Count == 0)
            {
                ErrorHolder.Text = "No photos found!";
            }
            else
            {
                int index = 0;
                foreach (Photo p in result.photos)
                {
                    int row = (int)Math.Floor((double)index / 4);
                    int col = index % 4;
                    Debug.WriteLine("ROW {0}, COL {1}, image: {2}", row, col, p.url);
                    sp = new StackPanel();
                    sp.SetValue(Grid.RowProperty, row);
                    sp.SetValue(Grid.ColumnProperty, col);
                    Image _pic = new Image();
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
}
