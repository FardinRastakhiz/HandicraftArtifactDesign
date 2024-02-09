using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenPlanShapes
{
    protected Transform parent;
    public virtual void Generate(BoardPlan activePlan) { }
    protected virtual void SetParents(BoardPlan activePlan) { }
}
