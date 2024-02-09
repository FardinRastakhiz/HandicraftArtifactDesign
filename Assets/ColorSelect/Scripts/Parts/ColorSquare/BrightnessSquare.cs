using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fardin.ColorTools
{
    public class BrightnessSquare : SquarePointer
    {
        UnityEngine.UI.Image colorPart1;
        UnityEngine.UI.Image sliderColor;
        UnityEngine.UI.Image sliderBrightness;
        protected override void Initial()
        {
            colorPart1 = transform.Find("Brightness").GetComponent<UnityEngine.UI.Image>();
            sliderColor = slider.transform.Find("SingleColor").GetComponent<UnityEngine.UI.Image>();
            sliderBrightness = slider.transform.Find("Brightness").GetComponent<UnityEngine.UI.Image>();
        }
        protected override void SetPoiter(Vector3 position, float slide)
        {
            pointer.position = position;
            sliderValue = slide;
            xValue = (pointer.localPosition.x + colorSquare.rect.width / 2) / colorSquare.rect.width;
            yValue = (pointer.localPosition.y + colorSquare.rect.height / 2) / colorSquare.rect.height;
            FillColorForm.ByHSB(xValue * 360.0f, yValue, sliderValue, terminal.colorForm);
        }
        protected override void On_Color_Change(object o, OnChangeColorHandler e)
        {
            SetShapesColor(e.form.HSB);
            sliderValue = e.form.HSB.brightness;
            slider.value = sliderValue;
            xValue = e.form.HSB.hue / 360.0f;
            yValue = e.form.HSB.saturation;
            Vector3 pos = pointer.localPosition;
            pos.x = xValue * colorSquare.rect.width - colorSquare.rect.width / 2;
            pos.y = yValue * colorSquare.rect.height - colorSquare.rect.height / 2;
            pointer.localPosition = pos;
        }

        void SetShapesColor(ColorHSB hsb)
        {
            Color partColor = Color.black;
            partColor.a = 1.0f - hsb.brightness;
            colorPart1.color = partColor;
            partColor = ColorConverter.HueToRGB(hsb.hue) / 255.0f;
            partColor.a = 1.0f;
            sliderColor.color = partColor;
            partColor = Color.white;
            partColor.a = 1 - hsb.saturation;
            sliderBrightness.color = partColor;
        }

    }
}