using UnityEngine;
using System.Collections;
using System;

namespace Fardin.ColorTools
{
    public class BlueSlider : SliderSet
    {
        protected override void On_Color_Change(object o, OnChangeColorHandler e)
        {
            colorLeft.color = new Color(e.form.RGB.r, e.form.RGB.g, 0);
            colorRight.color = new Color(e.form.RGB.r, e.form.RGB.g, 255);
            value = e.form.RGB.b;
            slider.value = Mathf.Round(value * slider.maxValue);
            inputField.text = "" + Mathf.Round(value * slider.maxValue);
        }

        protected override void ValueChanged()
        {
            FillColorForm.ByBlue(value, terminal.colorForm);
        }
    }

}