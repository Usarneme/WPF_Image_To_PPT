using PexelsDotNetSDK.Api;
using PexelsDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
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
        }

        private string StringOfBoldWordsFromRichTextBox(RichTextBox rtb)
        {
            List<string> boldWords = new List<string>();
            foreach (Paragraph p in rtb.Document.Blocks)
            {
                foreach (Run r in p.Inlines)
                {
                    //Debug.WriteLine("r.text {0}. r.fontweight {1}", r.Text, r.FontWeight);
                    if (r.FontWeight == FontWeights.Bold)
                    {
                        boldWords.Add(r.Text);
                    }
                }
            }
            return string.Join(" ", boldWords.ToArray());
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MyGrid.Children.Clear();
            ErrorHolder.Text = "";
            PexelsClient _pex = new PexelsClient("563492ad6f917000010000019a625093f14641ecb876ff0b65cc8627");
            string boldWords = StringOfBoldWordsFromRichTextBox(TextText);
            string query = TitleText.Text + " " + boldWords;
            Debug.WriteLine("Button clicked...getting results of query: {0}", query);
            PhotoPage result = await _pex.SearchPhotosAsync(query, null, "medium", null, null, 1, NUMBER_OF_IMAGES);
            //Debug.WriteLine("Got result from pexels: {0} - length: {1}", result, result.photos.Count);
            StackPanel sp;
            Border border;

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
                    //Debug.WriteLine("ROW {0}, COL {1}, image: {2}", row, col, p.url);
                    //sp = new StackPanel();
                    //sp.SetValue(Grid.RowProperty, row);
                    //sp.SetValue(Grid.ColumnProperty, col);
                    //sp.SetValue(NameProperty, "id_" + p.id);
                    //sp.AddHandler(MouseUpEvent, new MouseButtonEventHandler(Handle_Click));

                    border = new Border();
                    border.SetValue(Grid.RowProperty, row);
                    border.SetValue(Grid.ColumnProperty, col);
                    border.SetValue(NameProperty, "id_" + p.id);
                    border.AddHandler(MouseUpEvent, new MouseButtonEventHandler(Handle_Click));

                    Thickness t = new Thickness(5);
                    border.SetValue(BorderThicknessProperty, t);
                    border.SetValue(BorderBrushProperty, Brushes.Red);

                    Image _pic = new Image();
                    BitmapImage _myBm = new BitmapImage();
                    _myBm.BeginInit();
                    _myBm.UriSource = new Uri(p.source.medium);
                    _myBm.DecodePixelWidth = 200;
                    _myBm.EndInit();
                    _pic.Source = _myBm;
                    //sp.Children.Add(_pic);
                    //MyGrid.Children.Add(sp);
                    border.Child = _pic;
                    MyGrid.Children.Add(border);
                    index++;
                }
            }
        }

        public void Handle_Click(object sender, RoutedEventArgs e)
        {
            Border that = (Border)sender;
            Debug.WriteLine("Image clicked on: {0} {1}", that.Name, that.Style);
            Thickness t = new Thickness(5);
            that.SetValue(BorderThicknessProperty, t);
            that.SetValue(BorderBrushProperty, Brushes.Green);
            //Type myType = sender.GetType();
            //Debug.WriteLine("Got type of {0}", myType);
            //IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            //foreach (PropertyInfo p in props)
            //{
            //    object propValue = p.GetValue(sender, null);
            //    Debug.WriteLine("Got keys {0} and values {1}", p, propValue);
            //}
            // Got keys System.String Name and values id_4672553
        }
    }
}
