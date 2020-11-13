using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Adverthouse.Utility.ImageProcessing
{
    public class ColorMath
    {
        private Dictionary<string, double> mostUsed = new Dictionary<string, double>();
        public Dictionary<string, double> getDominantColor(Bitmap bmp, int top, bool perc = false)
        {

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);
                    string hex = HexConverter(clr);
                    if (mostUsed.ContainsKey(hex))
                    {
                        mostUsed[hex]++;
                    }
                    else
                    {
                        mostUsed.Add(hex, 1);
                    }
                }
            }

            return mostUsed.OrderByDescending(o => o.Value).Skip(0).Take(top).ToDictionary(v => v.Key,
                (v => v.Value == 0 ? 0 : perc
                ? Math.Round(((100 * v.Value) / mostUsed.Sum(a => a.Value)), 2) : v.Value)).Where(a => a.Value != 0).ToDictionary(a => a.Key, a => a.Value);
        }

        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private static String RGBConverter(System.Drawing.Color c)
        {
            return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
        }
    }
}
