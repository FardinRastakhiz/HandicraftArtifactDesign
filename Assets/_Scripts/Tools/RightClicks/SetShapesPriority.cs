///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <SetShapesPriority>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SetShapesPriority : MonoBehaviour {
    public static void MoveForward()
    {
        List<Shape> selectedShapes = new List<Shape>();
        var selOrders = SelectTools.lastShapes.OrderBy(x => x.order);
        selectedShapes = selOrders.ToList();
        for (int i = 0; i < selectedShapes.Count; i++)
        {
            selectedShapes[i].transform.SetAsLastSibling();
        }

        if (BoardPlans.ActiveIndex == -1)
            return;
        BoardPlan plan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
        GenBoardPlan.ResetOrders(plan);
    }

    public static void MoveBackward()
    {
        Transform parent = null;
        List<Shape> selectedShapes = new List<Shape>();
        var selOrders = SelectTools.lastShapes.OrderBy(x => x.order);
        selectedShapes = selOrders.ToList();
        if (parent == null)
            parent = selectedShapes[0].transform.parent;
        for (int i = selectedShapes.Count - 1; i >= 0; i--)
        {
            selectedShapes[i].transform.SetAsFirstSibling();
        }

        if (parent.name == "Grid")
            parent.Find("DrawCanvas").SetAsFirstSibling();

        if (BoardPlans.ActiveIndex == -1)
            return;
        BoardPlan plan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
        GenBoardPlan.ResetOrders(plan);
    }

    public static bool OneStepForward()
    {
        if (BoardPlans.ActiveIndex == -1)
            return false;
        BoardPlan plan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
        GenBoardPlan.ResetOrders(plan);
        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                   orderby item.order
                                   select item).ToList();
        int tail = selectedList[0].transform.parent.childCount-1;
        int i = 0;
        for (i = selectedList.Count()-1; i >=0 ; i--)
        {
            if(selectedList[i].order==tail){
                tail--;
            }
            else
                break;
        }
        if (i < 0)
            return false;
        for (int j = i; j >= 0; j--)
		{
            selectedList[j].transform.SetSiblingIndex(selectedList[j].order + 1);
        }
        GenBoardPlan.ResetOrders(plan);
        return true;
    }
    public static bool OneStepBackward()
    {
        if (BoardPlans.ActiveIndex == -1)
            return false;
        BoardPlan plan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
        GenBoardPlan.ResetOrders(plan);
        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        int head = 1;
        int size = selectedList.Count();
        int i = 0;
        for (i = 0; i < size; i++)
        {
            if (selectedList[i].order == head)
            {
                head++;
            }
            else
                break;
        }
        if (i >= size)
            return false;
        for (int j = i; j < size; j++)
        {
            selectedList[j].transform.SetSiblingIndex(selectedList[j].order - 1);
        }
        GenBoardPlan.ResetOrders(plan);
        return true;
    }
    public static void SetTotalShapesPriority(BoardPlan plan)
    {
        plan.indexInOrder.Sort();
        int size = plan.indexInOrder.Count;
        for (int i = 0; i < size; i++)
        {
            plan.orders[plan.indexInOrder[i]].transform.SetAsLastSibling();
        }
        GenBoardPlan.ResetOrders(plan);
    }
}
