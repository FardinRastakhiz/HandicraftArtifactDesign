using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace Fardin.ColorTools
{
    public class ColorHex : MonoBehaviour
    {
        [SerializeField]
        protected ColorTerminal terminal;
        string hex;
        UnityEngine.UI.InputField hexField;

        void Start()
        {
            if (!terminal)
                terminal = GameObject.FindGameObjectWithTag("ColorTerminal").GetComponent<ColorTerminal>();
            terminal.changedColor += On_Color_Change;
            hexField = GetComponent<UnityEngine.UI.InputField>();
        }

        public void On_Value_Change()
        {
            hex = hexField.text;
            hex.ToLower();
            hex = Regex.Replace(hex, @"[0-9a-f]", "");
        }



        public void On_End_Edit()
        {
            hex = hexField.text;
            hex.ToLower();
            hex = Regex.Replace(hex, @"[^0-9a-f]*$", "");
            hex = CompleteHex(hex);

            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == hexField.gameObject)
            {
                FillColorForm.ByHex(hex, terminal.colorForm);
                terminal.colorForm.isChanged = true;
            }
        }


        protected virtual void On_Color_Change(object o, OnChangeColorHandler e)
        {
            hexField.text = e.form.hexColor;
        }


        string CompleteHex(string hex)
        {
            string newHex = hex;
            int less = 6 - newHex.Length;
            for (int i = 0; i < less; i++)
                newHex = "0" + newHex;
            return newHex;
        }
    }
}