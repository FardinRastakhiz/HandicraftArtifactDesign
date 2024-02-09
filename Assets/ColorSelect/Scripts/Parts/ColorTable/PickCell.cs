using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fardin.ColorTools
{
    public class PickCell : MonoBehaviour
    {


        [SerializeField]
        protected ColorTerminal terminal;

        [SerializeField]
        AddToTable addToTable;


        void Start()
        {
            if (!terminal)
                terminal = GameObject.FindGameObjectWithTag("ColorTerminal").GetComponent<ColorTerminal>();
        }

        public void On_Cell_Click()
        {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            if (addToTable.filled[int.Parse(obj.name)])
            {
                Image img = obj.transform.Find("Color").GetComponent<Image>();
                FillColorForm.ByRGBA(img.color, terminal.colorForm);
            }
            addToTable.SetActiveCell(int.Parse(obj.name));
            terminal.colorForm.isChanged = true;
        }
    }
}