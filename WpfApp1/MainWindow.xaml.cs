using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UIElement Selected;
        public bool isDragging; //protected?
        public Point clickPosition; //private?
        public TranslateTransform originElement; // private?

        public static Image global_sender;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void area_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var draggableControl = e.OriginalSource as Image;
            originElement = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
            isDragging = true;
            clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
            //Image image = e.OriginalSource as Image;
            //global_sender = image;
            //DragDrop.DoDragDrop(image, image.Source, DragDropEffects.Copy);
        }

        private void area_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggableControl = e.OriginalSource as Image;
            draggableControl.ReleaseMouseCapture();
        }

        private void area_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = e.OriginalSource as Image;
            if (isDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition(this);
                var transform = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
                transform.X = originElement.X + (currentPosition.X - clickPosition.X);
                transform.Y = originElement.Y + (currentPosition.Y - clickPosition.Y);
                draggableControl.RenderTransform = new TranslateTransform(transform.X, transform.Y);
            }
        }

        private void area_Drop(object sender, DragEventArgs e)
        {
            ImageSource image = e.Data.GetData(typeof(ImageSource)) as ImageSource;
            Image imageControl = new Image()
            {
                Width = image.Width,
                Height = image.Height,
                Source = image,
            };
            Canvas.SetLeft(imageControl, e.GetPosition(this.area).X);
            Canvas.SetTop(imageControl, e.GetPosition(this.area).Y);
            this.area.Children.Add(imageControl);
        }

        private void area_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.OriginalSource as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.Copy);
        }
    }
}
