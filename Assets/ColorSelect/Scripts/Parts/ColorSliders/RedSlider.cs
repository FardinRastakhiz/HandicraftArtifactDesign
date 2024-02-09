using UnityEngine;
using System.Collections;
using System;

namespace Fardin.ColorTools
{
    public class RedSlider : SliderSet
    {
        protected override void On_Color_Change(object o, OnChangeColorHandler e)
        {
            colorLeft.color = new Color(0, e.form.RGB.g, e.form.RGB.b);
            colorRight.color = new Color(255, e.form.RGB.g, e.form.RGB.b);
            value = e.form.RGB.r;
            slider.value = Mathf.Round(value * slider.maxValue);
            inputField.text = "" + Mathf.Round(value * slider.maxValue);
        }

        protected override void ValueChanged()
        {
            FillColorForm.ByRed(value, terminal.colorForm);
        }
    }

}