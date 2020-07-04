using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;


namespace AntDesign.Colors
{
    public static class ColorHelper
    {
        private static readonly double hueStep = 2; // 色相阶梯
        private static readonly double saturationStep = 0.16; // 饱和度阶梯，浅色部分
        private static readonly double saturationStep2 = 0.05; // 饱和度阶梯，深色部分
        private static readonly double brightnessStep1 = 0.05; // 亮度阶梯，浅色部分
        private static readonly double brightnessStep2 = 0.015; // 亮度阶梯，深色部分
        private static readonly int lightColorCount = 5;
        private static readonly int darkColorCount = 4;

        private static readonly int[] darkColorIndices = new int[10] { 7, 6, 5, 5, 5, 5, 4, 3, 2, 1 };
        private static readonly double[] darkColorOpacities = new double[10] { 0.15, 0.25, 0.3, 0.45, 0.65, 0.85, 0.9, 0.95, 0.97, 0.98 };

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

        /// <summary>
        /// <para>Get Hue/Saturation/Value of a color</para>
        /// <para>Notice that HSV is different the built-in HSB value in .NET. It is actually HSL pattern.</para>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static (double Hue, double Saturation, double Value) GetHSV(this Color color)
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

        /// <summary>
        /// Get a color form HSV values
        /// </summary>
        /// <param name="hue">from 0 to 360</param>
        /// <param name="saturation">from 0 to 1</param>
        /// <param name="value">from 0 to 1</param>
        /// <returns></returns>
        public static Color FromHSV(double hue, double saturation, double value)
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
            return Color.FromArgb((int)Math.Round(r * 255, 0), (int)Math.Round(g * 255, 0), (int)Math.Round(b * 255, 0));
        }

        public static List<Color> GetPallete(Color color, Color? backgroundColor = null, bool isDark = false)
        {
            List<Color> colors = new List<Color>(10);
            (double hue, double saturation, double value) = GetHSV(color);
            for (int i = lightColorCount; i >0; i--)
            {
                Color c = ColorHelper.FromHSV(
                    GetHue(hue, i, true),
                    GetSaturation(saturation, hue, i, true),
                    GetValue(value, i, true)
                    );
                colors.Add(c);
            }
            colors.Add(color);
            for(int i = 1; i<= darkColorCount; i++)
            {
                Color c = ColorHelper.FromHSV(
                    GetHue(hue, i),
                    GetSaturation(saturation, hue, i),
                    GetValue(value, i));
                colors.Add(c);
            }
            if (isDark)
            {
                return TuneDarkPallete(colors, backgroundColor);
            }
            else
            {
                return colors;
            }
        }

        private static double GetHue(double hue, int index, bool light = false)
        {
            double result;
            if (Math.Round(hue) >= 60 && Math.Round(hue) <= 240)
            {
                result = light ? Math.Round(hue) - hueStep * index : Math.Round(hue) + hueStep * index;
            }
            else
            {
                result = light ? Math.Round(hue) + hueStep * index : Math.Round(hue) - hueStep * index;
            }
            if (result < 0)
            {
                result += 360;
            }
            else if (result >= 360)
            {
                result -= 360;
            }
            return result;
        }

        private static double GetSaturation(double saturation, double hue, int index, bool light = false)
        {
            if (hue == 0 && saturation == 0)
            {
                return 0;
            }
            double result;
            if (light)
            {
                result = saturation - saturationStep * index;
            }
            else if (index == darkColorCount)
            {
                result = saturation + saturationStep;
            }
            else
            {
                result = saturation + saturationStep2 * index;
            }
            if (result > 1)
            {
                result = 1;
            }
            if (light && index == lightColorCount && result > 0.1)
            {
                result = 0.1;
            }
            if (result < 0.06)
            {
                result = 0.06;
            }
            return Math.Round(result, 2);
        }

        private static double GetValue(double value, int index, bool light = false)
        {
            double result;
            if (light)
            {
                result = value + brightnessStep1 * index;
            }
            else
            {
                result = value - brightnessStep2 * index;
            }
            if (result > 1)
            {
                result = 1;
            }
            return Math.Round(result, 2);
        }

        private static List<Color> TuneDarkPallete(List<Color> colors, Color? backgroundColor = null)
        {
            List<Color> result = new List<Color>();
            Color background = backgroundColor ?? ColorHelper.FromHexString("141414");
            for(int i = 0; i< 10; i++)
            {
                result.Add(Mix(colors[darkColorIndices[i]], background, darkColorOpacities[i]));
            }
            return result;
        }
    }
}
