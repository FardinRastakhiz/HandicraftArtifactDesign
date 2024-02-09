using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Fardin.ColorTools
{
    public class AddToTable : MonoBehaviour
    {

        [SerializeField]
        protected ColorTerminal terminal;
        [SerializeField]
        Transform cellsParent;

        Image imageColor;

        Transform[] colorCells;
        [HideInInspector]
        public bool[] filled;
        int activeCell = 0;


        void Start()
        {
            if (!terminal)
                terminal = GameObject.FindGameObjectWithTag("ColorTerminal").GetComponent<ColorTerminal>();
            if (transform.name == "Add Color")
            {
                terminal.changedColor += On_Color_Change;
                imageColor = transform.Find("Color").GetComponent<Image>();
            }
            int size = cellsParent.childCount;
            colorCells = new Transform[size];
            filled = new bool[size];
            for (int i = cellsParent.childCount - 1; i >= 0; i--)
            {
                filled[i] = PlayerPrefs.GetInt("ColorCell_" + i + "_Filled", 0) == 1 ? true : false;
            }
            //colorCells = (Image[])(from item in cellsParent.GetComponentsInChildren<Image>() select item);
            int cellNum = 0;
            foreach (Transform item in cellsParent)
            {
                colorCells[cellNum] = item;
                if (filled[cellNum])
                {
                    colorCells[cellNum].Find("Color").GetComponent<Image>().color = new Color(
                        PlayerPrefs.GetFloat("ColorCell_" + cellNum + "_r", 1.0f),
                        PlayerPrefs.GetFloat("ColorCell_" + cellNum + "_g", 1.0f),
                        PlayerPrefs.GetFloat("ColorCell_" + cellNum + "_b", 1.0f),
                        PlayerPrefs.GetFloat("ColorCell_" + cellNum + "_a", 1.0f)
                        );
                }
                cellNum++;
            }

            SetActiveCell(PlayerPrefs.GetInt("ColorCell_ActiveCell", 0));
        }

        public void On_Add_Color()
        {
            colorCells[activeCell].Find("Color").GetComponent<Image>().color = imageColor.color;
            PlayerPrefs.SetFloat("ColorCell_" + activeCell + "_r", imageColor.color.r);
            PlayerPrefs.SetFloat("ColorCell_" + activeCell + "_g", imageColor.color.g);
            PlayerPrefs.SetFloat("ColorCell_" + activeCell + "_b", imageColor.color.b);
            PlayerPrefs.SetFloat("ColorCell_" + activeCell + "_a", imageColor.color.a);
            PlayerPrefs.SetInt("ColorCell_" + activeCell + "_Filled", 1);
            filled[activeCell] = true;
            SetActiveCell(activeCell + 1);
        }

        public void SetActiveCell(int cellIndex)
        {
            colorCells[activeCell].GetComponent<Outline>().effectColor = Color.black;
            activeCell = cellIndex;
            activeCell = activeCell > 19 ? 19 : activeCell;
            colorCells[activeCell].GetComponent<Outline>().effectColor = Color.cyan;
            PlayerPrefs.SetInt("ColorCell_ActiveCell", activeCell);
        }

        protected virtual void On_Color_Change(object o, OnChangeColorHandler e)
        {
            imageColor.color = e.form.RGB;
        }
    }
}