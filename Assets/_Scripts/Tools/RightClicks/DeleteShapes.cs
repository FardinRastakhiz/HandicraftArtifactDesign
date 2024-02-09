///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <DeleteShapes>
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

public class DeleteShapes : MonoBehaviour {
    public static void Delete()
    {
        if (SelectTools.currentBoard == null)
        {
            Debug.Log("aaa");
            DeleteParentless();
            return;
        }
        BoardPlan cPlan = SelectTools.currentBoard.GetComponent<Board>().plan;
        foreach (var item in SelectTools.lastShapes)
        {
            cPlan.indexInOrder.Remove(item.order);
            cPlan.orders.Remove(item.order);
            if (item.GetType() == typeof(Part))
            {
                Debug.Log("bbb" + cPlan);
                cPlan.parts.Remove((Part)item);
            }
            else if (item.GetType() == typeof(Primitive))
            {
                Debug.Log("ccc");
                cPlan.primitives.Remove((Primitive)item);
            }
            else if (item.GetType() == typeof(Background))
            {
                Debug.Log("ddd");
                cPlan.backgrounds.Remove((Background)item);
            }
            Destroy(item.gameObject);
        }
        Debug.Log("eee");
        SelectTools.ResetTotal();
    }

    static void DeleteParentless()
    {
        foreach (var item in SelectTools.lastShapes)
        {
            Destroy(item.gameObject);
        }
        SelectTools.ResetTotal();
    }

}