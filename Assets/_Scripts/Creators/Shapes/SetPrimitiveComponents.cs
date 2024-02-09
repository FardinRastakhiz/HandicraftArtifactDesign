using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class SetPrimitiveComponents : SetShapeComponents
{
    static ShapeCenter shapeObj;

    public static void SetExternals()
    {
        if (shapeObj == null)
        {
            shapeObj = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShapeCenter>();
        }
    }

    public static void SetBoardPlanPrimitives(BoardPlan activePlan)
    {
        SetExternals();
        for (int i = 0; i < activePlan.primitives.Count; i++)
        {
            SetSinglePrimitive(activePlan.primitives[i], activePlan.board.transform);
        }
    }

    public static void SetSinglePrimitive(Primitive prim, Transform boardTra)
    {
        SetExternals();
        AddComponents(prim);
        Board board = boardTra.GetComponent<Board>();
        SetComponents(prim, board);
    }
}