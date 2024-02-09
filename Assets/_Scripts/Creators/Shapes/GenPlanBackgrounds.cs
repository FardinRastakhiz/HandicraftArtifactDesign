using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenPlanBackgrounds : GenPlanShapes
{
    public GenPlanBackgrounds(BoardPlan activePlan)
    {
        parent = activePlan.board.transform.Find("Mask").Find("Grid");
        SetParents(activePlan);
        SetBackgroundComponents.SetBoardPlanBackgrounds(activePlan);
    }
    public override void Generate(BoardPlan activePlan)
    {
        parent = activePlan.board.transform.Find("Mask").Find("Grid");
        SetParents(activePlan);
        SetBackgroundComponents.SetBoardPlanBackgrounds(activePlan);
    }
    protected override void SetParents(BoardPlan activePlan)
    {
        for (int i = 0; i < activePlan.backgrounds.Count; i++)
        {
            activePlan.backgrounds[i].gameObject.transform.SetParent(parent);
        }
    }

    public void SetBorderHandles()
    {

    }
}
