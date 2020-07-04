using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AntDesign.Colors;

namespace AntDesign.Colors.Tests
{
    [TestClass()]
    public class Playground
    {
        [TestMethod()]
        public void ColorToStringTest()
        {
            string hexString = "406A80";
            Console.WriteLine(hexString);
            Color color = ColorHelper.FromHexString(hexString);
            Console.WriteLine(color.R);
            Console.WriteLine(color.G);
            Console.WriteLine(color.B);
            (double h, double s, double v) = ColorHelper.GetHSV(color);
            Console.WriteLine(h);
            Console.WriteLine(s);
            Console.WriteLine(v);
            Color color2 = ColorHelper.FromHSV(h, s, v);
            Console.WriteLine(color2.ToHexString());
        }

        [TestMethod()]
        public void Play()
        {
            Color color = ColorHelper.FromHexString("#f5222d");
            List<Color> colors = ColorHelper.GetPallete(color);
            foreach(Color c in colors)
            {
                Console.WriteLine(c.ToHexString());
            }
        }
    }
}
