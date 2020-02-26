using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace RichTextEditorWpf.Services
{
    public class ImageService : IImageService
    {
        public void AddImgCommand(RichTextBox rt)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg,*.gif, *.png) | *.jpg; *.jpeg; *.gif; *.png";
            var result = dlg.ShowDialog();
            if (result.Value)
            {
                Uri uri = new Uri(dlg.FileName, UriKind.Relative);
                BitmapImage bitmapImg = new BitmapImage(uri);
                Image image = new Image();
                image.Stretch = Stretch.Fill;
                image.Width = 250;
                image.Height = 200;
                image.Source = bitmapImg;
                var tp = rt.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
                new InlineUIContainer(image, tp);
            }
        }
    }
    public interface IImageService
    {
        void AddImgCommand(RichTextBox rt);
    }
}
