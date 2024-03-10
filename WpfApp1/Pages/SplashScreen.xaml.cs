using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApp1.Pages
{
    public partial class SplashScreen : Window
    {
        private List<string> imagePaths;
        private int currentImageIndex;

        public SplashScreen()
        {
            InitializeComponent();
            InitializeImages();
            ShowImage();

        }

        private void InitializeImages()
        {
            imagePaths = new List<string>
            {
                "/Images/image1.png",
                "/Images/image2.png",
                "/Images/image3.png"
            };
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            currentImageIndex = Math.Max(0, currentImageIndex - 1);
            ShowImage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentImageIndex = Math.Min(imagePaths.Count - 1, currentImageIndex + 1);
            ShowImage();
        }

        private void ShowImage()
        {
            string imagePath = imagePaths[currentImageIndex];
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            splashImage.Source = bitmapImage;

            prevArrow.Visibility = currentImageIndex > 0 ? Visibility.Visible : Visibility.Hidden;
            nextArrow.Visibility = currentImageIndex < imagePaths.Count - 1 ? Visibility.Visible : Visibility.Hidden;

            okButton.Visibility = currentImageIndex == imagePaths.Count - 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SplashImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(splashImage);
            double halfWidth = splashImage.ActualWidth / 2;

            if (mousePos.X < halfWidth)
            {
                PrevArrow_MouseLeftButtonDown(null, null);
            }
            else
            {
                NextArrow_MouseLeftButtonDown(null, null);
            }
        }

        private void PrevArrow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentImageIndex = Math.Max(0, currentImageIndex - 1);
            ShowImage();
        }

        private void NextArrow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentImageIndex = Math.Min(imagePaths.Count - 1, currentImageIndex + 1);
            ShowImage();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
            }
        }
    }
}
