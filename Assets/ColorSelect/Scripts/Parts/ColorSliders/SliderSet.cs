using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Fardin.ColorTools
{
    public class SliderSet : MonoBehaviour
    {

        [SerializeField]
        protected Slider slider;

        [SerializeField]
        protected InputField inputField;

        [SerializeField]
        protected Image colorLeft;
        [SerializeField]
        protected Image colorRight;


        [HideInInspector]
        protected float value;

        [SerializeField]
        protected ColorTerminal terminal;

        void Start()
        {
            if (!terminal)
                terminal = GameObject.FindGameObjectWithTag("ColorTerminal").GetComponent<ColorTerminal>();
            terminal.changedColor += On_Color_Change;
        }


        public void On_Slider_Change()
        {
            value = slider.value / slider.maxValue;
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == slider.gameObject)
                ValueChanged();
        }

        public void On_InputField_Change()
        {
            value = float.Parse(inputField.text) / slider.maxValue;
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == inputField.gameObject)
                ValueChanged();
        }


        protected virtual void ValueChanged()
        {

        }

        protected virtual void On_Color_Change(object o, OnChangeColorHandler e)
        {

        }
    }
}