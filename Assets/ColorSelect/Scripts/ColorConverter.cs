using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Fardin.ColorTools
{
    public class ColorConverter
    {
        public static ColorHSB RGBToHSB(Color rgb)
        {
            ColorHSB hsbCol = new ColorHSB();
            List<int> intRGB = new List<int>();
            intRGB.Add((int)(rgb.r * 255.0f));
            intRGB.Add((int)(rgb.g * 255.0f));
            intRGB.Add((int)(rgb.b * 255.0f));
            intRGB.Sort();
            int i = takePosition(rgb);
            float sign = 1;
            if (i == 0)
                sign = Mathf.Sign(rgb.g - rgb.b);
            else if (i == 1)
                sign = Mathf.Sign(rgb.b - rgb.r);
            else
                sign = Mathf.Sign(rgb.r - rgb.g);
            float delta1 = intRGB[1] - intRGB[0];
            float delta2 = intRGB[2] - intRGB[0];
            if (delta2 < 0.03f)
            {
                if (delta1 < 0.03f)
                    delta1 = delta2 = 1;
                else
                {
                    delta1 = 0;
                    delta2 = 1;
                }
            }
            hsbCol.hue = (120 * i) + sign * 60 * delta1 / delta2;
            hsbCol.hue = hsbCol.hue < 0 ? 360 + hsbCol.hue : hsbCol.hue;
            hsbCol.saturation = intRGB[2] == 0 ? 0 : 1 - intRGB[0] / (float)intRGB[2];
            hsbCol.brightness = intRGB[2] / 255.0f;
            hsbCol.alpha = rgb.a;
            return hsbCol;
        }

        static int takePosition(Color rgb)
        {
            int i = 2;
            if (rgb.r >= rgb.g)
            {
                if (rgb.r >= rgb.b)
                {
                    i = 0;
                }
            }
            else
            {
                if (rgb.g >= rgb.b)
                {
                    i = 1;
                }
            }
            return i;
        }

        public static string RGBToHex(Color rgb)
        {
            string r = ((int)(rgb.r * 255)).ToString("X2");
            string g = ((int)(rgb.g * 255)).ToString("X2");
            string b = ((int)(rgb.b * 255)).ToString("X2");
            return r + g + b;
        }

        public static Color HexToRGB(string hex)
        {
            Color RGB = new Color();
            RGB.r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
            RGB.g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
            RGB.b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
            RGB.a = 1.0f;
            return RGB;
        }


        public static Color HSBToRGB(ColorHSB hsb)
        {
            Color rgb = HueToRGB(hsb.hue);

            float hue = hsb.hue + 60;
            hue = hue > 360 ? hue - 360 : hue;
            int i = (int)(hue / 120.0f);
            int j = i - 1 < 0 ? 2 : (i - 1);
            int k = i + 1 > 2 ? 0 : (i + 1);
            rgb[j] += (rgb[i] - rgb[j]) * (1 - hsb.saturation);
            rgb[k] += (rgb[i] - rgb[k]) * (1 - hsb.saturation);
            rgb *= hsb.brightness / 255.0f;
            rgb.a = hsb.alpha;
            return rgb;
        }

        public static Color HueToRGB(float hue)
        {
            Color rgb = new Color();
            hue = hue + 60;
            hue = hue >= 360 ? hue - 360 : hue;
            int i = (int)(hue / 120.0f);
            rgb[i] = 255;
            int j = i - 1 < 0 ? 2 : (i - 1);
            int k = i + 1 > 2 ? 0 : (i + 1);
            if ((int)(hue / 60.0f) % 2 == 0)
            {
                rgb[j] = ((60 - (hue % 60.0f)) / 60.0f) * 255;
                rgb[k] = 0;
            }
            else
            {
                rgb[j] = 0;
                rgb[k] = ((hue % 60.0f) / 60.0f) * 255;
            }
            return rgb;
        }

        //Not Complete
        public static Vector2 RGBToHUESquare(Color rgb)
        {
            Vector2 position = Vector2.zero;

            return position;
        }

    }
}