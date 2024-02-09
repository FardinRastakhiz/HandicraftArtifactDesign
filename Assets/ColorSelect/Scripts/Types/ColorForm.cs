using UnityEngine;
using System.Collections;

namespace Fardin.ColorTools
{
    public class ColorForm : MonoBehaviour
    {
        private Color rgb;

        private float alpha;
        private ColorHSB hsb;
        public string hexColor;

        internal Color RGB 
        {
            get { return rgb; }
            set
            {
                isChanged = true;
                rgb = value;
            }
        }
        internal float Alpha
        {
            get { return alpha; }
            set
            {
                isChanged = true;
                alpha = value;
            }
        }
        internal ColorHSB HSB
        {
            get { return hsb; }
            set
            {
                isChanged = true;
                hsb = value;
            }
        }
        internal string HexColor
        {
            get { return hexColor; }
            set
            {
                isChanged = true;
                hexColor = value;
            }
        }

        public bool isChanged;


        public void Initialize()
        {
            rgb = new Color();
            alpha = 1.0f;
            hsb = new ColorHSB();
            hexColor = "000000";
        }

        public void Initialize(Color rgb)
        {
            Initialize();
            RGB = rgb;
            HSB = ColorConverter.RGBToHSB(RGB);
            HexColor = ColorConverter.RGBToHex(RGB);
            alpha = rgb.a;
            isChanged = true;
        }
    }
}