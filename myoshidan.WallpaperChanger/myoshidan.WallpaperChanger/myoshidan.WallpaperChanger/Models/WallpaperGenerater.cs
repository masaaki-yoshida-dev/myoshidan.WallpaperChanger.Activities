using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myoshidan.WallpaperChanger.Models
{
    /// <summary>
    /// GenerateWallpaper
    /// </summary>
    public class WallpaperGenerater
    {
        /// <summary>
        /// GenerateWallpaper
        /// </summary>
        public WallpaperGenerater()
        {
        }

        /// <summary>
        /// GenerateWallPaperFromSolidColor
        /// </summary>
        /// <param name="bgColor"></param>
        /// <param name="txtColor"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontName"></param>
        /// <param name="outputFilePath"></param>
        public void GenerateWallPaperFromSolidColor(KnownColor bgColor,
                                                    KnownColor txtColor,
                                                    string text,
                                                    int fontSize,
                                                    string fontName,
                                                    string outputFilePath)
        {

            var height = Screen.PrimaryScreen.Bounds.Height;
            var width = Screen.PrimaryScreen.Bounds.Width;
            var img = new Bitmap(width, height);
            var bgColorBrash = new SolidBrush(Color.FromKnownColor(bgColor));
            var graphic = Graphics.FromImage(img);

            graphic.FillRectangle(bgColorBrash, graphic.VisibleClipBounds);

            if (!string.IsNullOrEmpty(text))
            {
                var textBrash = new SolidBrush(Color.FromKnownColor(txtColor));
                var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Near;
                var font = new Font(fontName, fontSize);
                graphic.DrawString(text, font, textBrash, new Rectangle(0, 0, img.Width, img.Height), format);
            }

            graphic.Dispose();
            img.Save(outputFilePath, ImageFormat.Png);
            img.Dispose();
        }

        /// <summary>
        /// GenerateWallPaperFromFile
        /// </summary>
        /// <param name="bgFilePath"></param>
        /// <param name="txtColor"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontName"></param>
        /// <param name="outputFilePath"></param>
        public void GenerateWallPaperFromFile(string bgFilePath,
                                              KnownColor txtColor,
                                              string text,
                                              int fontSize,
                                              string fontName,
                                              string outputFilePath)
        {
            var height = Screen.PrimaryScreen.Bounds.Height;
            var width = Screen.PrimaryScreen.Bounds.Width;
            var img = new Bitmap(width, height);
            var bgColorBrash = new SolidBrush(Color.Black);
            var graphic = Graphics.FromImage(img);

            graphic.FillRectangle(bgColorBrash, graphic.VisibleClipBounds);

            if (!string.IsNullOrEmpty(bgFilePath))
            {
                var pic = new Bitmap(bgFilePath);
                graphic.DrawImage(pic, img.Width / 2 - pic.Width / 2, img.Height / 2 - pic.Height / 2, pic.Width, pic.Height);
            }

            if (!string.IsNullOrEmpty(text))
            {
                var textBrash = new SolidBrush(Color.FromKnownColor(txtColor));
                var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Near;
                var font = new Font(fontName, fontSize);
                graphic.DrawString(text, font, textBrash, new Rectangle(0, 0, img.Width, img.Height), format);
            }

            graphic.Dispose();
            img.Save(outputFilePath, ImageFormat.Png);
            img.Dispose();
        }

    }
}
