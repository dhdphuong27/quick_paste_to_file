using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PasteToFile.MVVM.Model
{
    public static class ScreenshotHelper
    {
        /// <summary>
        /// Captures the entire screen and returns it as a BitmapSource
        /// </summary>
        public static BitmapSource CaptureFullScreen()
        {
            // Get screen dimensions
            int screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            int screenHeight = (int)SystemParameters.PrimaryScreenHeight;

            // Create bitmap to hold screen capture
            using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight, PixelFormat.Format32bppArgb))
            {
                // Create graphics object from bitmap
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Copy screen to bitmap
                    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                }

                // Convert System.Drawing.Bitmap to BitmapSource
                return ConvertBitmapToBitmapSource(bitmap);
            }
        }

        /// <summary>
        /// Captures a specific region of the screen
        /// </summary>
        public static BitmapSource CaptureRegion(int x, int y, int width, int height)
        {
            using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
                }

                return ConvertBitmapToBitmapSource(bitmap);
            }
        }

        /// <summary>
        /// Converts System.Drawing.Bitmap to WPF BitmapSource
        /// </summary>
        private static BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Make it cross-thread accessible

                return bitmapImage;
            }
        }
    }
}