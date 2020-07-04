using System;
using System.Drawing;
using System.Text;


namespace AntDesign.Colors
{
    public static class AntDesignColor
    {
        /// <summary>
        /// Mix two colors according to their RGBA values.
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <param name="ratio">ratio is between 0.0 and 1.0. if ratio is less than 0.0, it is taken as 0.0. if ratio is larger than 1.0, it is taken as 1.0</param>
        /// <returns>Mixture of two colors in paticular ratio</returns>
        public static Color Mix(Color color1, Color color2, double ratio)
        {
            ratio = ratio < 0 ? 0 : (ratio > 1 ? 1 : ratio);
            int r = (int)(color1.R * ratio + color2.R * (1 - ratio));
            int g = (int)(color1.G * ratio + color2.G * (1 - ratio));
            int b = (int)(color1.B * ratio + color2.B * (1 - ratio));
            int a = (int)(color1.A * ratio + color2.A * (1 - ratio));
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Convert a color to Hex string
        /// </summary>
        /// <param name="color"></param>
        /// <param name="includeAlpha"></param>
        /// <returns></returns>
        public static string ToHexString(this Color color, bool includeAlpha = false)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("#");
            if (includeAlpha)
            {
                builder.Append(color.A.ToString("X2"));
            }
            builder.Append(color.R.ToString("X2")).Append(color.G.ToString("X2")).Append(color.B.ToString("X2"));
            return builder.ToString();
        }

        /// <summary>
        /// <para>Convert a hex string to a color.</para>
        /// <para>Hex string should be in one of the following format: </para>
        /// <para>"#AAA", "#ABABAB", "#AAAA", "#ABABABAB", "AAA", "ABABAB", "AAAA", "ABABABAB"</para>
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static Color FromHexString(string hexString)
        {
            if (hexString.Length < 3 || hexString.Length > 9)
            {
                throw new ArgumentException("Invalid hex color format");
            }
            string input = hexString;
            if (hexString.StartsWith("#"))
            {
                input = input.Substring(1);
            }
            if (input.Length <= 4)
            {
                StringBuilder build = new StringBuilder();
                foreach (char c in input)
                {
                    build.Append(c).Append(c);
                }
                input = build.ToString();
            }
            try
            {
                if (input.Length == 6)
                {
                    return ParseOpaqueHexString(input);
                }
                else if (input.Length == 8)
                {
                    return ParseFullHexString(input);
                }
                else
                {
                    throw new ArgumentException("Invalid hex color format");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid hex color format", ex);
            }
        }

        /// <summary>
        /// Parse string in ABABABAB format
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static Color ParseFullHexString(string hexString)
        {
            var span = hexString.AsSpan();
            try
            {
                byte a = byte.Parse(span.Slice(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte r = byte.Parse(span.Slice(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(span.Slice(4, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(span.Slice(6, 2), System.Globalization.NumberStyles.HexNumber);
                return Color.FromArgb(a, r, g, b);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid hex color format", ex);
            }

        }

        /// <summary>
        /// Parse string in ABABAB format
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static Color ParseOpaqueHexString(string hexString)
        {
            var span = hexString.AsSpan();
            try
            {
                byte r = byte.Parse(span.Slice(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(span.Slice(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(span.Slice(4, 2), System.Globalization.NumberStyles.HexNumber);
                return Color.FromArgb(r, g, b);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid hex color format", ex);
            }
        }

        public static (int Hue, double Saturation, double Value) GetHSV(this Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;
            double cmax = Math.Max(Math.Max(r, g), b);
            double cmin = Math.Min(Math.Min(r, g), b);
            double delta = cmax - cmin;
            double h = 0;
            if (delta != 0)
            {
                if (cmax == r)
                {
                    h = (60 * ((g - b) / delta) % 6);
                }
                else if (cmax == g)
                {
                    h = (60 * ((b - r) / delta + 2));
                }
                else if (cmax == b)
                {
                    h = (60 * ((r - g) / delta + 4));
                }
            }
            double s = cmax == 0 ? 0 : delta / cmax;
            double v = cmax;
            return ((int)Math.Round(h, 0), Math.Round(s, 4), Math.Round(v, 4));
        }

        public static Color FromHSV(int hue, double saturation, double value)
        {
            int h = (int)Math.Floor(hue / 60.0) % 6;
            double f = hue / 60.0 - h;
            double p = value * (1 - saturation);
            double q = value * (1 - f * saturation);
            double t = value * (1 - (1 - f) * saturation);
            double r, g, b;
            (r, g, b) = h switch
            {
                0 => (value, t, p),
                1 => (q, value, p),
                2 => (p, value, t),
                3 => (p, q, value),
                4 => (t, p, value),
                5 => (value, p, q),
                _ => (0, 0, 0)
            };
            return Color.FromArgb((int)Math.Round(r*255, 0), (int)Math.Round(g*255, 0), (int)Math.Round(b*255, 0));
        }
    }
}
