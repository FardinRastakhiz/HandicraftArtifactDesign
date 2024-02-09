using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCanvas : MonoBehaviour {

    public static void ChangeBoardCanvas(int canvasTex)
    {
        if (BoardPlans.ActiveIndex == -1)
            return;
        Transform boardCanvas = BoardPlans.boardPlans[BoardPlans.ActiveIndex].board.transform.Find("Mask").Find("Grid").Find("DrawCanvas");
        boardCanvas.GetComponent<Image>().sprite = ShapeCenter.boardCanvas[canvasTex];
        BoardPlans.boardPlans[BoardPlans.ActiveIndex].Canvas = canvasTex;
    }
}
