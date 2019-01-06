using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace eJay.Helper
{
    public class PrintHelper
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves an using encoder. </summary>
        ///
        /// <remarks>   Andre Beging, 13.09.2017. </remarks>
        ///
        /// <param name="fileName">     Filename of the file. </param>
        /// <param name="uiElement">    The element. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void SaveUsingEncoder(string fileName, FrameworkElement uiElement)
        {
            var encoder = new PngBitmapEncoder();

            var height = (int)uiElement.ActualHeight;
            var width = (int)uiElement.ActualWidth;

            var container = VisualTreeHelper.GetParent(uiElement) as UIElement;
            var relativeLocation = uiElement.TranslatePoint(new Point(0, 0), container);

            // These two line of code make sure that you get completed visual bitmap.
            // In case your Framework Element is inside the scroll viewer then some part which is not
            // visible gets clip.  
            uiElement.Measure(new Size(width, height));
            uiElement.Arrange(new Rect(new Point(), new Point(width, height)));

            var bitmap = new RenderTargetBitmap(width,

                height,
                96, // These decides the dpi factors 
                96,// The can be changed when we'll have preview options.
                PixelFormats.Pbgra32);




            bitmap.Render(uiElement);

            SaveUsingBitmapTargetRenderer(fileName, bitmap, encoder);

            uiElement.Arrange(new Rect(relativeLocation, new Point(relativeLocation.X+width, relativeLocation.Y+height)));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves an using bitmap target renderer. </summary>
        ///
        /// <remarks>   Andre Beging, 13.09.2017. </remarks>
        ///
        /// <param name="fileName">             Filename of the file. </param>
        /// <param name="renderTargetBitmap">   The render target bitmap. </param>
        /// <param name="bitmapEncoder">        The bitmap encoder. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void SaveUsingBitmapTargetRenderer(string fileName, RenderTargetBitmap renderTargetBitmap, BitmapEncoder bitmapEncoder)
        {
            var frame = BitmapFrame.Create(renderTargetBitmap);
            bitmapEncoder.Frames.Add(frame);
            // Save file .
            using (var stream = File.Create(fileName))
            {
                bitmapEncoder.Save(stream);
            }
        }
    }
}