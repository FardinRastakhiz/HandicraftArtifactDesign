///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ToolsUtility>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ToolsUtility : MonoBehaviour {
    static Transforms tansforms;
    public static bool isDragOn;
    public static bool canMoveShape;
    public static ToolsState toolState;
    public static Transform highlightedShape;
    static GameObject lastTool;

    public static void SetToolsState(ToolsState state, GameObject stateButton)
    {
        SetToolsSprite(stateButton);
        SetState(state);
    }

    public static void SetState(ToolsState state)
    {
        toolState = state;
        switch (state)
        {
            case ToolsState.SELECT:
                SelectTools.Start(ref tansforms);
                canMoveShape = true;
                break;
            case ToolsState.MOVE:
                MoveTools.Start(ref tansforms);
                canMoveShape = false;
                break;
            case ToolsState.ROTATE:
                RotateTools.Start(ref tansforms);
                canMoveShape = false;
                break;
            case ToolsState.SCALE:
                ScaleTools.Start(ref tansforms);
                canMoveShape = false;
                break;
        }
    }

    static string SetToolsSprite(GameObject toolButton)
    {
        SpriteState spriteStates;
        if (lastTool != null)
        {
            lastTool.GetComponent<Image>().sprite = toolButton.GetComponent<Image>().sprite;
            spriteStates = lastTool.GetComponent<Button>().spriteState;
            spriteStates.highlightedSprite = toolButton.GetComponent<Button>().spriteState.highlightedSprite;
            lastTool.GetComponent<Button>().spriteState = spriteStates;
        }
        spriteStates = toolButton.GetComponent<Button>().spriteState;
        spriteStates.highlightedSprite = spriteStates.pressedSprite;
        toolButton.GetComponent<Image>().sprite = spriteStates.pressedSprite;
        toolButton.GetComponent<Button>().spriteState = spriteStates;
        lastTool = toolButton;
        return toolButton.name;
    }
    public static void On_Shape_Mouse_Over(Transform shape)
    {
        highlightedShape = shape;
    }

    public static void On_Shape_Mouse_Over_Exit()
    {
        highlightedShape = null;
    }

    public static void On_Shape_Click()
    {
        //GenBoardPlan.UpdateBoardPlan(highlightedShape.transform.parent);
        if (isDragOn && !canMoveShape)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            tansforms.On_Shape_Click();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //RightClick.OnShapeRightClick();
        }
    }


    public static void On_Shape_Begin_Drag()
    {
        tansforms.On_Shape_Begin_Drag();
    }

    public static void On_Free_Area_Click()
    {
        tansforms.On_Free_Area_Click();
    }
    public static void On_Free_Area_Begin_Drag(Transform board)
    {
        isDragOn = true;
        tansforms.On_Free_Area_Begin_Drag(board);
    }

    public static void On_Free_Area_Drag(BoardPlan plan)
    {
        tansforms.On_Free_Area_Drag(plan);
    }

    public static void On_Drag_Exit()
    {
        tansforms.On_Drag_Exit();
    }

}

public enum ToolsState { SELECT, MOVE, ROTATE, SCALE};