///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Clss Namespacea>
///   Class:          <GenPlanPrimitives>
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
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.EventSystems;

public class GenPlanPrimitives : GenPlanShapes
{
    public GenPlanPrimitives(BoardPlan activePlan)
    {
        parent = activePlan.board.transform.Find("Mask").Find("Grid");
        SetParents(activePlan);
        SetPrimitiveComponents.SetBoardPlanPrimitives(activePlan);
    }

    public override void Generate(BoardPlan activePlan)
    {
        parent = activePlan.board.transform.Find("Mask").Find("Grid");
        SetParents(activePlan);
        SetPrimitiveComponents.SetBoardPlanPrimitives(activePlan);
    }
    protected override void SetParents(BoardPlan activePlan)
    {
        for (int i = 0; i < activePlan.primitives.Count; i++)
        {
            activePlan.primitives[i].gameObject.transform.SetParent(parent);
        }
    }
}
