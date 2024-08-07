using Microsoft.Identity.Client;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Archer.Extension.Images
{
    public class WatermarkHelper
    {
        /// <summary>
        /// 增加浮水印到圖片 <br />
        /// 會有文字大小自動調整功能，避免文字過大
        /// </summary>
        /// <param name="base64Images"></param>
        /// <param name="watermarkText"></param>
        /// <param name="font"></param>
        /// <param name="textColor"></param>
        /// <param name="position"></param>
        /// <returns>base64字串圖片陣列</returns>
        public string[] AddWatermarkToImages(string[] base64Images, string watermarkText, Font font, Color textColor, WatermarkPosition position, ImageFormatEnum imageFormat = ImageFormatEnum.Jpeg)
        {
            string[] watermarkedImages = new string[base64Images.Length];
            for (int i = 0; i < base64Images.Length; i++)
            {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64Images[i])))
                using (Image image = Image.FromStream(ms))
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    // 計算最大字體大小
                    // 圖片最小邊長的 1/20
                    float maxFontSize = Math.Min(image.Width, image.Height) / 20f;
                    float scaleFactor = maxFontSize / font.Size;
                    Font scaledFont = scaleFactor < 1
                        ? new Font(font.FontFamily, maxFontSize, font.Style)
                        : font;

                    SizeF textSize = graphics.MeasureString(watermarkText, scaledFont);

                    // 如果文字大小超過圖片大小的一半，進行調整
                    if (textSize.Width > image.Width / 2 || textSize.Height > image.Height / 2)
                    {
                        float adjustFactor = Math.Min(image.Width / 2 / textSize.Width, image.Height / 2 / textSize.Height);
                        float adjustedSize = scaledFont.Size * adjustFactor;
                        scaledFont = new Font(scaledFont.FontFamily, adjustedSize, scaledFont.Style);
                        textSize = graphics.MeasureString(watermarkText, scaledFont);
                    }

                    float x, y;
                    switch (position)
                    {
                        case WatermarkPosition.TopLeft:
                            x = 10;
                            y = 10;
                            break;
                        case WatermarkPosition.TopCenter:
                            x = (image.Width - textSize.Width) / 2;
                            y = 10;
                            break;
                        case WatermarkPosition.TopRight:
                            x = image.Width - textSize.Width - 10;
                            y = 10;
                            break;
                        case WatermarkPosition.MiddleLeft:
                            x = 10;
                            y = (image.Height - textSize.Height) / 2;
                            break;
                        case WatermarkPosition.MiddleCenter:
                            x = (image.Width - textSize.Width) / 2;
                            y = (image.Height - textSize.Height) / 2;
                            break;
                        case WatermarkPosition.MiddleRight:
                            x = image.Width - textSize.Width - 10;
                            y = (image.Height - textSize.Height) / 2;
                            break;
                        case WatermarkPosition.BottomLeft:
                            x = 10;
                            y = image.Height - textSize.Height - 10;
                            break;
                        case WatermarkPosition.BottomCenter:
                            x = (image.Width - textSize.Width) / 2;
                            y = image.Height - textSize.Height - 10;
                            break;
                        case WatermarkPosition.BottomRight:
                        default:
                            x = image.Width - textSize.Width - 10;
                            y = image.Height - textSize.Height - 10;
                            break;
                    }

                    using (Brush brush = new SolidBrush(Color.FromArgb(128, textColor)))
                    {
                        graphics.DrawString(watermarkText, scaledFont, brush, x, y);
                    }

                    using (MemoryStream outputMs = new MemoryStream())
                    {
                        ImageFormat imgFormat = GetImageFormatFrom(imageFormat);

                        image.Save(outputMs, imgFormat);
                        watermarkedImages[i] = Convert.ToBase64String(outputMs.ToArray());
                    }
                }
            }

            return watermarkedImages;
        }

        /// <summary>
        /// [Experimental] 移除浮水印
        /// </summary>
        /// <param name="base64Image"></param>
        /// <param name="watermarkColor"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public string RemoveWatermark(string base64Image, Color watermarkColor, int threshold)
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64Image)))
            using (Bitmap original = new Bitmap(ms))
            using (Bitmap result = new Bitmap(original.Width, original.Height))
            {
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        Color pixelColor = original.GetPixel(x, y);

                        if (IsCloseToWatermarkColor(pixelColor, watermarkColor, threshold))
                        {
                            result.SetPixel(x, y, GetAverageColor(original, x, y));
                            //result.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                        }
                        else
                        {
                            result.SetPixel(x, y, pixelColor);
                        }
                    }
                }

                using (MemoryStream outputMs = new MemoryStream())
                {
                    result.Save(outputMs, ImageFormat.Jpeg);

                    return Convert.ToBase64String(outputMs.ToArray());
                }
            }
        }

        private bool IsCloseToWatermarkColor(Color pixel, Color watermark, int threshold)
        {
            return Math.Abs(pixel.R - watermark.R) < threshold &&
                   Math.Abs(pixel.G - watermark.G) < threshold &&
                   Math.Abs(pixel.B - watermark.B) < threshold;
        }

        private Color GetAverageColor(Bitmap image, int x, int y)
        {
            int totalR = 0, totalG = 0, totalB = 0, count = 0;

            for (int i = -10; i <= 10; i++)
            {
                if (i == 0)
                    continue;

                for (int j = -10; j <= 10; j++)
                {
                    if (j == 0)
                        continue;

                    if (x + i >= 0 && x + i < image.Width && y + j >= 0 && y + j < image.Height)
                    {
                        Color neighborColor = image.GetPixel(x + i, y + j);
                        totalR += neighborColor.R;
                        totalG += neighborColor.G;
                        totalB += neighborColor.B;
                        count++;
                    }
                }
            }

            return Color.FromArgb(totalR / count, totalG / count, totalB / count);
        }

        private ImageFormat GetImageFormatFrom(ImageFormatEnum imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormatEnum.Jpeg:
                    return ImageFormat.Jpeg;
                case ImageFormatEnum.Png:
                    return ImageFormat.Png;
                case ImageFormatEnum.Gif:
                    return ImageFormat.Gif;
                case ImageFormatEnum.Bmp:
                    return ImageFormat.Bmp;
                case ImageFormatEnum.Tiff:
                    return ImageFormat.Tiff;
                case ImageFormatEnum.Icon:
                    return ImageFormat.Icon;
                case ImageFormatEnum.Emf:
                    return ImageFormat.Emf;
                case ImageFormatEnum.Wmf:
                    return ImageFormat.Wmf;
                case ImageFormatEnum.Exif:
                    return ImageFormat.Exif;
                case ImageFormatEnum.MemoryBmp:
                    return ImageFormat.MemoryBmp;
                default:
                    return ImageFormat.Jpeg;
            }
        }
    }

    public enum WatermarkPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public enum ImageFormatEnum
    {
        Jpeg,
        Png,
        Gif,
        Bmp,
        Tiff,
        Icon,
        Emf,
        Wmf,
        Exif,
        MemoryBmp
    }
}
