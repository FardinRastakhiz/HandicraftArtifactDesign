using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackgroundComponents : SetShapeComponents
{
    public static void SetBoardPlanBackgrounds(BoardPlan activePlan)
    {
        for (int i = 0; i < activePlan.backgrounds.Count; i++)
        {
            SetSingleBackground(activePlan.backgrounds[i], activePlan.board.transform);
        }
    }
    public static void SetSingleBackground(Background bg, Transform boardTra)
    {
        AddComponents(bg);
        Board board = boardTra.GetComponent<Board>();
        SetComponents(bg, board);
        AddBorderControl(bg);
        //bg.GetComponent<RectTransform>().sizeDelta = bg.transform2D.scale;
    }

    static void AddBorderControl(Background bg)
    {
        if (bg.transform.Find("borderControl")!=null)
            return;
        GameObject borderControl = Instantiate(ShapeCenter.borderControl);
        borderControl.name = "borderControl";
        borderControl.transform.SetParent(bg.transform);
        borderControl.transform.localEulerAngles = Vector3.zero;
        borderControl.GetComponent<ExtendDynamics>().window = bg.GetComponent<RectTransform>();
        RectTransform borderRect = borderControl.GetComponent<RectTransform>();
        borderRect.offsetMin = Vector2.zero;
        borderRect.offsetMax = Vector2.one;
        borderRect.localScale = Vector3.one;
        borderControl.SetActive(false);
    }
}
