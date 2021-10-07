using PexelsDotNetSDK.Api;
using PexelsDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace HelloWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int NUMBER_OF_IMAGES = 8;
        public List<string> imageIds = new List<string>();
        public Dictionary<string, string> imageUrlById = new Dictionary<string, string>();

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
            PhotoPage result = await _pex.SearchPhotosAsync(query, null, "medium", null, null, 1, NUMBER_OF_IMAGES);
            Border border;

            if (result.photos.Count == 0)
            {
                ErrorHolder.Text = "No photos found!";
                Slide_Button.IsEnabled = false;
            }
            else
            {
                int index = 0;
                foreach (Photo p in result.photos)
                {
                    string name = "id_" + p.id;
                    imageUrlById.Add(name, p.source.medium);
                    int row = (int)Math.Floor((double)index / 4);
                    int col = index % 4;

                    border = new Border();
                    border.SetValue(Grid.RowProperty, row);
                    border.SetValue(Grid.ColumnProperty, col);
                    border.SetValue(NameProperty, name);
                    border.AddHandler(MouseUpEvent, new MouseButtonEventHandler(Handle_Click));

                    Image _pic = new Image();
                    BitmapImage _myBm = new BitmapImage();
                    _myBm.BeginInit();
                    _myBm.UriSource = new Uri(p.source.medium);
                    _myBm.DecodePixelWidth = 200;
                    _myBm.EndInit();
                    _pic.Source = _myBm;

                    border.Child = _pic;
                    MyGrid.Children.Add(border);
                    index++;
                }
                Slide_Button.IsEnabled = true;
            }
        }

        private void Handle_Click(object sender, RoutedEventArgs e)
        {
            Border that = (Border)sender;
            if (that.GetValue(BorderBrushProperty) == null)
            {
                Thickness t = new Thickness(5);
                that.SetValue(BorderThicknessProperty, t);
                that.SetValue(BorderBrushProperty, Brushes.Green);
                imageIds.Add(that.Name);
            }
            else
            {
                Thickness t = new Thickness(0);
                that.SetValue(BorderThicknessProperty, t);
                that.SetValue(BorderBrushProperty, null);
                imageIds.Remove(that.Name);
            }
        }

        private void Create_Slide(object sender, RoutedEventArgs e)
        {
            Application pptApp = new Application();
            var pptPresentation = pptApp.Presentations.Add(MsoTriState.msoTrue);
            var customLayout = pptPresentation.SlideMaster.CustomLayouts[PpSlideLayout.ppLayoutPictureWithCaption];
            Slides slides;
            _Slide slide;
            PowerPoint.TextRange objText;

            slides = pptPresentation.Slides;
            slide = pptPresentation.Slides.AddSlide(1, customLayout);

            objText = slide.Shapes[1].TextFrame.TextRange;
            objText.Text = TitleText.Text;
            objText.Font.Size = 32;

            objText = slide.Shapes[2].TextFrame.TextRange;
            System.Windows.Documents.TextRange txt = new System.Windows.Documents.TextRange(TextText.Document.ContentStart, TextText.Document.ContentEnd);
            objText.Text = txt.Text;

            foreach (string xamlName in imageIds)
            {
                string url = imageUrlById[xamlName];
                PowerPoint.Shape shape = slide.Shapes[2];
                slide.Shapes.AddPicture(url, MsoTriState.msoFalse, MsoTriState.msoTrue, shape.Left, shape.Top, shape.Width, shape.Height);
            }

            pptPresentation.SaveAs(Name, PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoTrue);
        }
    }
}
