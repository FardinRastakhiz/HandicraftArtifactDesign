using UnityEngine;
using System.Collections;

namespace Fardin.ColorTools
{
    public class FillColorForm : MonoBehaviour
    {

        public static void ByRGBA(Color rgb, ColorForm form)
        {
            form.RGB = rgb;
            form.Alpha = rgb.a;
            form.HSB = ColorConverter.RGBToHSB(rgb);
            form.hexColor = ColorConverter.RGBToHex(rgb);
        }
        public static void ByHSB(ColorHSB hsb, ColorForm form)
        {
            form.HSB = hsb;
            form.RGB = ColorConverter.HSBToRGB(hsb);
            form.hexColor = ColorConverter.RGBToHex(form.RGB);
        }

        public static void ByHSB(float hue, float saturation, float brightness, ColorForm form)
        {
            form.HSB.hue = hue;
            form.HSB.saturation = saturation;
            form.HSB.brightness = brightness;
            form.HSB.alpha = form.Alpha;
            form.RGB = ColorConverter.HSBToRGB(form.HSB);
            form.hexColor = ColorConverter.RGBToHex(form.RGB);
        }

        public static void ByHex(string hex, ColorForm form)
        {
            form.hexColor = hex;
            form.RGB = ColorConverter.HexToRGB(hex);
            form.HSB = ColorConverter.RGBToHSB(form.RGB);
        }

        public static void ByRed(float red, ColorForm form)
        {
            Color rgb = form.RGB;
            rgb.r = red;
            ByRGBA(rgb, form);
        }
        public static void ByGreen(float green, ColorForm form)
        {
            Color rgb = form.RGB;
            rgb.g = green;
            ByRGBA(rgb, form);
        }
        public static void ByBlue(float blue, ColorForm form)
        {
            Color rgb = form.RGB;
            rgb.b = blue;
            ByRGBA(rgb, form);
        }

        public static void ByAlpha(float alpha, ColorForm form)
        {
            form.Alpha = alpha;
            Color rgb = form.RGB;
            rgb.a = alpha;
            ByRGBA(rgb, form);
        }

        public static void ByHue(float hue, ColorForm form)
        {
            form.HSB.hue = hue;
            ByHSB(form.HSB, form);
        }
        public static void BySaturation(float saturation, ColorForm form)
        {
            form.HSB.saturation = saturation;
            ByHSB(form.HSB, form);
        }
        public static void ByBrightness(float brightness, ColorForm form)
        {
            form.HSB.brightness = brightness;
            ByHSB(form.HSB, form);
        }
    }
}