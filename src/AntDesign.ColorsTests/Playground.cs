using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AntDesign.Colors;

namespace AntDesign.ColorTests
{
    [TestClass()]
    public class Playground
    {
        [TestMethod()]
        public void ColorToStringTest()
        {
            string hexString = "406A80";
            Console.WriteLine(hexString);
            Color color = AntDesignColor.FromHexString(hexString);
            Console.WriteLine(color.R);
            Console.WriteLine(color.G);
            Console.WriteLine(color.B);
            (int h, double s, double v) = AntDesignColor.GetHSV(color);
            Console.WriteLine(h);
            Console.WriteLine(s);
            Console.WriteLine(v);
            Color color2 = AntDesignColor.FromHSV(h, s, v);
            Console.WriteLine(color2.ToHexString());
        }

        [TestMethod()]
        public void Play()
        {
            Console.WriteLine((int)(200.7));
        }
    }
}
