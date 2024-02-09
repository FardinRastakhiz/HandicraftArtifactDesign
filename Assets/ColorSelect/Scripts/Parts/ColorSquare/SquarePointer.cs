using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Fardin.ColorTools
{
    public class SquarePointer : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform pointer;
        [SerializeField]
        protected RectTransform colorSquare;

        [SerializeField]
        protected Slider slider;

        protected float xValue = 0;
        protected float yValue = 0;
        protected float sliderValue = 0;

        [SerializeField]
        protected ColorTerminal terminal;
        void Start()
        {
            if (!terminal)
                terminal = GameObject.FindGameObjectWithTag("ColorTerminal").GetComponent<ColorTerminal>();
            terminal.changedColor += On_Color_Change;
            Initial();
        }

        public void On_Square_Click()
        {
            SetPoiter(Input.mousePosition, slider.value);
            terminal.colorForm.isChanged = true;
        }

        public void On_Begin_Drag()
        {
            SetPoiter(Input.mousePosition, slider.value);
            terminal.colorForm.isChanged = true;
        }

        public void On_Drag()
        {
            SetPoiter(Fardin.UITools.RectTools.SetInRect(colorSquare, Input.mousePosition), slider.value);
            terminal.colorForm.isChanged = true;
        }

        public void On_Slider_Change()
        {
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == slider.gameObject)
            {
                SetPoiter(pointer.position, slider.value);
                terminal.colorForm.isChanged = true;
            }
        }


        protected virtual void Initial() { }
        protected virtual void SetPoiter(Vector3 position, float slide) { }

        protected virtual void On_Color_Change(object o, OnChangeColorHandler e) { }
    }
}