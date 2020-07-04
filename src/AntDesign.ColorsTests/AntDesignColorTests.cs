using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntDesign.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AntDesign.Colors.Tests
{
    [TestClass()]
    public class AntDesignColorTests
    {

        public static IEnumerable<object[]> GenerateHexStringParsingTestCase()
        {
            Random random = new Random();
            byte[] bytes = new byte[4];
            for (int i = 0; i < 10; i++)
            {
                random.NextBytes(bytes);
                Color color = Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
                yield return new object[] { color };
            }
        }
        [TestMethod()]
        [DynamicData(nameof(GenerateHexStringParsingTestCase), DynamicDataSourceType.Method)]
        public void FromHexStringTest(Color color)
        {
            string sa = color.ToHexString(true);
            string s = color.ToHexString();
            Assert.AreEqual(color, AntDesignColor.FromHexString(sa));
            Color color2 = Color.FromArgb(255, color);
            Assert.AreEqual(color2, AntDesignColor.FromHexString(s));
        }


        public static IEnumerable<object[]> GetHexStringSpecialTestCase()
        {
            yield return new object[] { Color.FromArgb(255, 0, 0, 0), "000" };
            yield return new object[] { Color.FromArgb(0, 0, 0, 0), "0000" };
            yield return new object[] { Color.FromArgb(255, 0, 0, 0), "000000" };
            yield return new object[] { Color.FromArgb(0, 0, 0, 0), "00000000" };
            yield return new object[] { Color.FromArgb(255, 0, 0, 0), "#000" };
            yield return new object[] { Color.FromArgb(0, 0, 0, 0), "#0000" };
            yield return new object[] { Color.FromArgb(255, 0, 0, 0), "#000000" };
            yield return new object[] { Color.FromArgb(0, 0, 0, 0), "#00000000" };
            yield return new object[] { Color.FromArgb(255, 255, 0, 0), "F00" };
            yield return new object[] { Color.FromArgb(0, 255, 0, 0), "0F00" };
            yield return new object[] { Color.FromArgb(255, 255, 0, 0), "FF0000" };
            yield return new object[] { Color.FromArgb(0, 255, 0, 0), "00FF0000" };
            yield return new object[] { Color.FromArgb(255, 255, 0, 0), "#F00" };
            yield return new object[] { Color.FromArgb(0, 255, 0, 0), "#0F00" };
            yield return new object[] { Color.FromArgb(255, 255, 0, 0), "#FF0000" };
            yield return new object[] { Color.FromArgb(0, 255, 0, 0), "#00FF0000" };
        }

        [TestMethod()]
        [DynamicData(nameof(GetHexStringSpecialTestCase), DynamicDataSourceType.Method)]
        public void FromHexStringSpecialCaseTest(Color color, string hexString)
        {
            Assert.AreEqual(color, AntDesignColor.FromHexString(hexString));
        }

        public static IEnumerable<object[]> GenerateHexStringTestCase()
        {
            yield return new object[] { Color.FromArgb(0, 0, 0), "#000000" };
            yield return new object[] { Color.FromArgb(255, 255, 255), "#FFFFFF" };
            yield return new object[] { Color.Red, "#FF0000" };
            yield return new object[] { Color.Blue, "#0000FF" };
            yield return new object[] { Color.Lime, "#00FF00" };
        }

        [TestMethod()]
        [DynamicData(nameof(GenerateHexStringTestCase), DynamicDataSourceType.Method)]
        public void ToHexStringTests(Color color, string hexString)
        {
            Assert.AreEqual(color.ToHexString(), hexString);
        }

        public static IEnumerable<object[]> GenerateColorMixTestCase()
        {
            yield return new object[] { Color.FromArgb(10, 10, 10), Color.FromArgb(0, 0, 0), 0.5, Color.FromArgb(5, 5, 5) };
            yield return new object[] { Color.FromArgb(10, 10, 10), Color.FromArgb(0, 0, 0), 0.3, Color.FromArgb(3, 3, 3) };
        }

        [TestMethod()]
        [DynamicData(nameof(GenerateColorMixTestCase), DynamicDataSourceType.Method)]
        public void MixTests(Color color1, Color color2, double ratio, Color result)
        {
            Assert.AreEqual(result, AntDesignColor.Mix(color1, color2, ratio));
        }

        [TestMethod()]
        public void FromHSVTest()
        {
            Color color = AntDesignColor.FromHSV(201, 0.5, 0.52);
            Console.WriteLine(color.ToHexString());
        }
    }
}