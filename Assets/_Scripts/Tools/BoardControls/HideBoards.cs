using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBoards : MonoBehaviour {



    public static void HideExcept(int index)
    {
        if (index == -1)
        {
            for (int i = 0; i < BoardPlans.boardPlans.Count; i++)
                BoardPlans.boardPlans[i].Hide(false);
            return;
        }
        for (int i = 0; i < BoardPlans.boardPlans.Count; i++)
        {
            BoardPlans.boardPlans[i].Hide(i != index);
        }
    }

    public static void activePlanClosed()
    {

    }
    public static void HideBehind(int fullScreenBoard)
    {
        BoardPlans.ordersList.Sort();
        for (int i = BoardPlans.ordersList.Count - 1; i >= 0; i--)
        {
            BoardPlans.inOrders[BoardPlans.ordersList[i]].Hide(BoardPlans.ordersList[i] < fullScreenBoard);
        }
    }
}
