using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteBoardClick : MonoBehaviour
{
    Rect paletteBoard;
    RectTransform paletteTra;

    void Start()
    {
        paletteTra = GameObject.FindGameObjectWithTag("PaletteBoard").GetComponent<RectTransform>();
    }

    public void OnPaletteBoardClicked()
    {
        paletteBoard = Fardin.UITools.RectTools.ToScreen(paletteTra);
        GenBoardPlan.OnPaletteBoardClicked(paletteBoard);
    }
}
