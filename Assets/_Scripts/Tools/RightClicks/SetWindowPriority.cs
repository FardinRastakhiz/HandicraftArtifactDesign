using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWindowPriority : MonoBehaviour {
    public static void MoveForward()
    {
        BoardPlan activePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
        Transform paletteBoard = activePlan.board.transform.parent;
        activePlan.board.transform.SetAsLastSibling();
        paletteBoard.Find("ShowButtons").SetAsLastSibling();
        GenBoardPlan.ResetBoardOrders();
        SetTotalBoardsPriority();
    }

    public static void MoveBackward()
    {
        BoardPlan activePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
        activePlan.board.transform.SetAsFirstSibling();
        Transform paletteBoard = activePlan.board.transform.parent;
        paletteBoard.Find("OuterParts").SetAsFirstSibling();
        paletteBoard.Find("WorkFrame").SetAsFirstSibling();
        GenBoardPlan.ResetBoardOrders();
        SetTotalBoardsPriority();
    }

    public static bool OneStepForward()
    {
        BoardPlan activePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
        Transform paletteBoard = activePlan.board.transform.parent;
        activePlan.board.transform.SetSiblingIndex(activePlan.order + 1);
        paletteBoard.Find("ShowButtons").SetAsLastSibling();
        GenBoardPlan.ResetBoardOrders();
        SetTotalBoardsPriority();
        if (activePlan.order + 1 >= paletteBoard.transform.childCount)
            return false;
        return true;
    }
    public static bool OneStepBackward()
    {
        BoardPlan activePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
        Transform paletteBoard = activePlan.board.transform.parent;
        activePlan.board.transform.SetSiblingIndex(activePlan.order - 1);
        paletteBoard.Find("OuterParts").SetAsFirstSibling();
        paletteBoard.Find("WorkFrame").SetAsFirstSibling();
        GenBoardPlan.ResetBoardOrders();
        SetTotalBoardsPriority();
        if (activePlan.order -1 <= 1)
            return false;
        return true;
    }

    public static void SetTotalBoardsPriority()
    {
        if (BoardPlans.ordersList.Count == 0)
            return;
        BoardPlans.ordersList.Sort();
        int size = BoardPlans.ordersList.Count;
        for (int i = 0; i < size; i++)
        {
            BoardPlans.inOrders[BoardPlans.ordersList[i]].board.transform.SetAsLastSibling();
        }
        BoardPlans.inOrders[BoardPlans.ordersList[0]].board.transform.parent.Find("ShowButtons").SetAsLastSibling();
    }
}
