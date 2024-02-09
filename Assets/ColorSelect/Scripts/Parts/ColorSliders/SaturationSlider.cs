using UnityEngine;
using System.Collections;
using System;

namespace Fardin.ColorTools
{
    public class SaturationSlider : SliderSet
    {
        protected override void On_Color_Change(object o, OnChangeColorHandler e)
        {
            Color col = Color.black;
            col.a = 1 - e.form.HSB.brightness - 0.15f;
            colorLeft.color = col;
            col = ColorConverter.HueToRGB(e.form.HSB.hue) / 255.0f;
            col.a = 1.0f;
            colorRight.color = col;
            value = e.form.HSB.saturation;
            slider.value = Mathf.Round(value * slider.maxValue);
            inputField.text = "" + Mathf.Round(value * slider.maxValue);
        }

        protected override void ValueChanged()
        {
            FillColorForm.BySaturation(value, terminal.colorForm);
        }
    }

}