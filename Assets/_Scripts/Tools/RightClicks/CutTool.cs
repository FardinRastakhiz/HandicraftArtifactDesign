///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <CutTool>
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

public class CutTool : ClipBoard
{
    public static void TakeShapes()//(int curPlan, HashSet<Shape> shapes)
    {
        isCut = true;
        if (BoardPlans.ActiveIndex != -1)
        {
            sourcePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex].name;
            ToolsUI tools = UIController.tools.GetComponent<ToolsUI>();
            tools.CustomSelect(tools.transform.Find("SELECT").gameObject);
            GenBoardPlan.ResetOrders(BoardPlans.boardPlans[BoardPlans.ActiveIndex]);
        }
        ResetInstances();
        foreach (var item in SelectTools.lastShapes)
        {
            ShapeInstance instance = new ShapeInstance();
            if (item.GetType() == typeof(Primitive))
            {
                TakePrimitiveParameters(item as Primitive, instance);
            }
            else if (item.GetType() == typeof(Part))
            {
                TakePartParameters(item as Part, instance);
            }
            else
            {
                TakeBackgroundParameters(item as Background, instance);
            }
            instances.Add(instance);
        }
        DeleteShapes.Delete();
    }
}
