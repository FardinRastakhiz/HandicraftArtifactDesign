using UnityEngine;
using System.Collections;
using System;

namespace Fardin.ColorTools
{
    public class BrightnessSlider : SliderSet
    {
        protected override void On_Color_Change(object o, OnChangeColorHandler e)
        {
            Color col = Color.white - ColorConverter.HueToRGB(e.form.HSB.hue) / 255.0f;
            col = ColorConverter.HueToRGB(e.form.HSB.hue) / 255.0f + col * (1 - e.form.HSB.saturation);
            col.a = 1.0f;
            colorRight.color = col;
            value = e.form.HSB.brightness;
            slider.value = Mathf.Round(value * slider.maxValue);
            inputField.text = "" + Mathf.Round(value * slider.maxValue);
        }

        protected override void ValueChanged()
        {
            FillColorForm.ByBrightness(value, terminal.colorForm);
        }
    }

}