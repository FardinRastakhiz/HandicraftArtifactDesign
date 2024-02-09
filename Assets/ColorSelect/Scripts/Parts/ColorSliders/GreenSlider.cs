using UnityEngine;
using System.Collections;
using System;

namespace Fardin.ColorTools
{
    public class GreenSlider : SliderSet
    {
        protected override void On_Color_Change(object o, OnChangeColorHandler e)
        {
            colorLeft.color = new Color(e.form.RGB.r, 0, e.form.RGB.b);
            colorRight.color = new Color(e.form.RGB.r, 255, e.form.RGB.b);
            value = e.form.RGB.g;
            slider.value = Mathf.Round(value * slider.maxValue);
            inputField.text = "" + Mathf.Round(value * slider.maxValue);
        }

        protected override void ValueChanged()
        {
            FillColorForm.ByGreen(value, terminal.colorForm);
        }
    }

}