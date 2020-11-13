using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Adverthouse.Utility.ImageProcessing
{
    public class ImageProcessor : IDisposable
    {
        public bool WriteError { get; set; }
        public string ErrorMessage { get; set; }

        public ImageProcessor()
        {
            WriteError = false;
            ErrorMessage = "";
        }
        public double Ratio
        {
            get
            {
                return Math.Round(((double)Width / (double)Height), 4);
            }
        }

        public int Height { get; set; }

        public int Width { get; set; }
        private Bitmap imgPhoto;
        private Dictionary<string, double> dominantColors;

        public Dictionary<string, double> DominantsColors
        {
            get
            {
                return dominantColors;
            }
        }

        public void Load(Stream ImageStream)
        {
            imgPhoto = new Bitmap(ImageStream);
            Width = imgPhoto.Width;
            Height = imgPhoto.Height;
        }



        public void Load(string fileName, bool setDominantColors = false)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            imgPhoto = new Bitmap(System.Drawing.Image.FromStream(fs));
            Width = imgPhoto.Width;
            Height = imgPhoto.Height;
            fs.Close();
            if (setDominantColors) SetDominantColors();
        }


        public string Format
        {
            get
            {
                var bmpFormat = imgPhoto.RawFormat;
                var strFormat = "unidentified format";
                if (bmpFormat.Equals(ImageFormat.Bmp))
                {
                    strFormat = "BMP";
                }
                else
                {
                    if (bmpFormat.Equals(ImageFormat.Emf))
                    {
                        strFormat = "EMF";
                    }
                    else
                    {
                        if (bmpFormat.Equals(ImageFormat.Exif))
                        {
                            strFormat = "EXIF";
                        }
                        else
                        {
                            if (bmpFormat.Equals(ImageFormat.Gif))
                            {
                                strFormat = "GIF";
                            }
                            else
                            {
                                if (bmpFormat.Equals(ImageFormat.Icon))
                                {
                                    strFormat = "Icon";
                                }
                                else
                                {
                                    if (bmpFormat.Equals(ImageFormat.Jpeg))
                                    {
                                        strFormat = "JPEG";
                                    }
                                    else
                                    {
                                        if (bmpFormat.Equals(ImageFormat.MemoryBmp))
                                        {
                                            strFormat = "MemoryBMP";
                                        }
                                        else
                                        {
                                            if (bmpFormat.Equals(ImageFormat.Png))
                                            {
                                                strFormat = "PNG";
                                            }
                                            else
                                            {
                                                if (bmpFormat.Equals(ImageFormat.Tiff))
                                                {
                                                    strFormat = "TIFF";
                                                }
                                                else
                                                {
                                                    if (bmpFormat.Equals(ImageFormat.Wmf))
                                                    {
                                                        strFormat = "WMF";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return strFormat;
            }
        }

        public void FixedSize(int Width, int Height, bool KeepActualSize, bool MergeState, string MergeImage)
        {
            var sourceWidth = imgPhoto.Width;
            var sourceHeight = imgPhoto.Height;
            var sourceX = 0;
            var sourceY = 0;
            var destX = 0;
            var destY = 0;
            var destWidth = 0;
            var destHeight = 0;
            if (!KeepActualSize)
            {
                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;
                nPercentW = (Convert.ToSingle(Width) / Convert.ToSingle(sourceWidth));
                nPercentH = (Convert.ToSingle(Height) / Convert.ToSingle(sourceHeight));
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                    destX = System.Convert.ToInt16((Width - (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = System.Convert.ToInt16((Height - (sourceHeight * nPercent)) / 2);
                }
                destWidth = Convert.ToInt32((sourceWidth * nPercent));
                destHeight = Convert.ToInt32((sourceHeight * nPercent));
            }
            else
            {
                destX = System.Convert.ToInt16((Convert.ToSingle(Width) - Convert.ToSingle(sourceWidth)) / 2);
                destY = System.Convert.ToInt16((Convert.ToSingle(Height) - Convert.ToSingle(sourceHeight)) / 2);
                destWidth = imgPhoto.Width;
                destHeight = imgPhoto.Height;
            }
            var bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
            if (MergeState)
            {
                var imgF = Image.FromFile(MergeImage);
                grPhoto.DrawImage(imgF, sourceX, sourceY, Width, Height);
            }
            grPhoto.Dispose();
            imgPhoto = bmPhoto;
        }

        public void SetDominantColors()
        {
            dominantColors = new Dictionary<string, double>();

            Bitmap calcImage = imgPhoto.Clone(new Rectangle(0, 0, imgPhoto.Width, imgPhoto.Height), PixelFormat.Format4bppIndexed);
            ColorMath mt = new ColorMath();
            dominantColors = mt.getDominantColor(calcImage, 16, true);
        }

        public void FixedSize(int Width, bool setDominantColors = false)
        {
            var sourceX = 0;
            var sourceY = 0;
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            float nPercent = (Convert.ToSingle(Width) / Convert.ToSingle(sourceWidth));

            int destWidth = Convert.ToInt32((sourceWidth * nPercent));
            int destHeight = Convert.ToInt32((sourceHeight * nPercent));

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            Graphics grPhoto = Graphics.FromImage((Image)bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.CompositingMode = CompositingMode.SourceCopy;
            grPhoto.PixelOffsetMode = PixelOffsetMode.Half;
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);


            grPhoto.Dispose();
            imgPhoto = bmPhoto;
        }

        public void SaveImage(string fileName)
        {
            imgPhoto.Save(fileName, ImageFormat.Jpeg);
            imgPhoto.Dispose();
        }

        public Image GetImageAsStream()
        {
            return imgPhoto;
        }

        public void Dispose()
        {
            imgPhoto.Dispose();
        }
    }
}
