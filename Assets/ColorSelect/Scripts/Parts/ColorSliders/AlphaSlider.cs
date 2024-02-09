using UnityEngine;
using System.Collections;
using System;

namespace Fardin.ColorTools
{
    public class AlphaSlider : SliderSet
    {
        protected override void On_Color_Change(object o, OnChangeColorHandler e)
        {
            value = e.form.Alpha;
            slider.value = Mathf.Round(value * slider.maxValue);
            inputField.text = "" + Mathf.Round(value * slider.maxValue);
        }

        protected override void ValueChanged()
        {
            FillColorForm.ByAlpha(value, terminal.colorForm);
        }
    }

}