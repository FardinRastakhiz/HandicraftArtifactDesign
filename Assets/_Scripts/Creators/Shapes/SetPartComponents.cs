using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class SetPartComponents : SetShapeComponents
{
    static UIController controller;
    public static void SetactivePlanParts(BoardPlan activePlan)
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
        for (int i = 0; i < activePlan.parts.Count; i++)
        {
            SetSinglePart(activePlan.parts[i], activePlan.board.transform);
        }
    }

    public static void SetSinglePart(Part part, Transform boardTra)
    {
        if (controller == null)
            controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
        AddComponents(part);
        Board board = boardTra.GetComponent<Board>();
        SetComponents(part, board);
    }
}