using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlans : MonoBehaviour
{
    public static int activeIndex;
    public static List<BoardPlan> boardPlans;
    public static Dictionary<int, BoardPlan> inOrders;
    public static List<int> ordersList;

    public BoardPlans()
    {
        boardPlans = new List<BoardPlan>();
        inOrders = new Dictionary<int, BoardPlan>();
        ordersList = new List<int>();
    }


    public BoardPlan ActiveBoardPlan
    {
        get { return boardPlans[ActiveIndex]; }
        set { ActiveIndex = boardPlans.IndexOf(value); }
    }

    public static int ActiveIndex
    {
        get { return activeIndex; }
        set
        {
            activeIndex = value;
            SelectTools.ActiveBoard = activeIndex;
        }
    }

    public int BoardPlanIndex(BoardPlan plan)
    {
        return boardPlans.IndexOf(plan);
    }
}

public class BoardIndex
{

}
