///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Clss Namespacea>
///   Class:          <GenPlanParts>
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
public class GenPlanParts : GenPlanShapes
{
    public GenPlanParts(BoardPlan activePlan)
    {
        parent = activePlan.board.transform.Find("Mask").Find("Grid");
        SetParents(activePlan);
        SetPartComponents.SetactivePlanParts(activePlan);
    }
    public override void Generate(BoardPlan activePlan)
    {
        parent = activePlan.board.transform.Find("Mask").Find("Grid");
        SetParents(activePlan);
        SetPartComponents.SetactivePlanParts(activePlan);
    }
    protected override void SetParents(BoardPlan activePlan)
    {
        for (int i = 0; i < activePlan.parts.Count; i++)
        {
            activePlan.parts[i].gameObject.transform.SetParent(parent);
        }
    }

}
