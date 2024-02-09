using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fardin.ColorTools
{
    public class HUESquare : SquarePointer
    {
        UnityEngine.UI.Image colorPart1;
        UnityEngine.UI.Image colorPart2;
        protected override void Initial()
        {
            colorPart1 = transform.Find("Gradient2").GetComponent<UnityEngine.UI.Image>();
            colorPart2 = transform.Find("Gradient3").GetComponent<UnityEngine.UI.Image>();
        }
        protected override void SetPoiter(Vector3 position, float slide)
        {
            pointer.position = position;
            sliderValue = slide;
            xValue = (pointer.localPosition.x + colorSquare.rect.width / 2) / colorSquare.rect.width;
            yValue = (pointer.localPosition.y + colorSquare.rect.height / 2) / colorSquare.rect.height;

            FillColorForm.ByHSB(sliderValue * 360.0f, xValue, yValue, terminal.colorForm);
        }
        protected override void On_Color_Change(object o, OnChangeColorHandler e)
        {
            SetShapesColor(e.form.HSB.hue);
            sliderValue = e.form.HSB.hue / 360.0f;
            slider.value = sliderValue;
            xValue = e.form.HSB.saturation;
            yValue = e.form.HSB.brightness;
            Vector3 pos = pointer.localPosition;
            pos.x = xValue * colorSquare.rect.width - colorSquare.rect.width / 2;
            pos.y = yValue * colorSquare.rect.height - colorSquare.rect.height / 2;
            pointer.localPosition = pos;
        }

        void SetShapesColor(float hue)
        {
            Color partColor = ColorConverter.HueToRGB(hue) / 255.0f;
            //float min = Mathf.Min(partColor.r, partColor.g, partColor.b);
            //float max = Mathf.Max(partColor.r, partColor.g, partColor.b);
            //if (min == partColor.r)
            //    partColor.r = 0;
            //else if (min == partColor.g)
            //    partColor.g = 0;
            //else
            //    partColor.b = 0;
            partColor.a = 1.0f;
            colorPart1.color = partColor;
            colorPart2.color = partColor;
        }
    }
}