using UnityEngine;
using System.Collections;
using System;

namespace Fardin.ColorTools
{
    public class HueSlider : SliderSet
    {
        protected override void On_Color_Change(object o, OnChangeColorHandler e)
        {
            value = e.form.HSB.hue / 360.0f;
            slider.value = Mathf.Round(value * slider.maxValue);
            inputField.text = "" + Mathf.Round(value * slider.maxValue);
        }

        protected override void ValueChanged()
        {
            FillColorForm.ByHue(value * 360.0f, terminal.colorForm);
        }
    }

}