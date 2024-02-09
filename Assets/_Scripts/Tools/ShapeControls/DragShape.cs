///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <DragShape>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Linq;

public class DragShape : MonoBehaviour
{
    static bool nstartDrag;
    static Transform targetBoard;
    static Rect WorkFrame;
    static Vector3 mouseStart;
    static Vector3 rectStart;
    static Transform paletteWindow;
    static bool dragCopy;
    static Transform dragObject;
    public static void Drag_Copy_Shape()
    {
        dragCopy = true;
    }
    public static void Drag_Copy_Shape_Off()
    {
        dragCopy = false;
    }

    public static void Drag_Shape(string dragState)
    {
        if (ToolsUtility.toolState == ToolsState.SELECT)
        {
            if (dragCopy)
            {
                
            }
            switch (dragState)
            {
                case "Begin":
                    On_Shape_Begin_Drag();
                    if (DefaultShortcuts.ctrlActive)
                    {
                        SelectTools.ResetTotal();
                        SelectTools.SelectCustomShape(dragObject.GetComponent<Shape>());
                        dragObject.GetComponent<Shape>().order = dragObject.transform.GetSiblingIndex();
                        DuplicateShapes.Duplicate();
                        dragObject.transform.SetSiblingIndex(dragObject.GetComponent<Shape>().order);
                        dragObject = SelectTools.lastShapes.ElementAt(0).transform;
                    }
                    break;
                case "dragging":
                    On_Shape_Drag();
                    break;
                case "end":
                    On_Shape_EndDrag();
                    break;
                default:
                    break;
            }
        }
    }

    static void On_Shape_Begin_Drag()
    {
        if (paletteWindow==null)
            paletteWindow = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().paletteWindow;

        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            dragObject = EventSystem.current.currentSelectedGameObject.transform;
            //dragObject.position = mousePosition;
            mouseStart = mousePosition;
            rectStart = dragObject.position;
            if (dragObject.parent.name == "OuterParts" || dragObject.parent.parent.name == "OuterParts")
                WorkFrame = RectTools.ToScreen(dragObject.parent.parent.Find("WorkFrame").GetComponent<RectTransform>());
            else
                WorkFrame = RectTools.ToScreen(dragObject.parent.Find("DrawCanvas").GetComponent<RectTransform>());
        }
        else if (Input.GetMouseButton(1))
        {
            Rect paletteRect = RectTools.ToScreen(paletteWindow.GetComponent<RectTransform>());
            if (paletteRect.Contains(Input.mousePosition))
            {
                if (!nstartDrag)
                    nstartDrag = GetTargetBoard(Input.mousePosition, out targetBoard);
                if (nstartDrag)
                    targetBoard.GetComponentInChildren<OnGrid>().Begin_Drag();
            }
        }
    }

    static void On_Shape_Drag()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            //var dragObject = EventSystem.current.currentSelectedGameObject.transform;
            dragObject.position = WorkFrame.Contains(Input.mousePosition) ? rectStart + Input.mousePosition - mouseStart : dragObject.position;
        }
        else if (Input.GetMouseButton(1))
        {
            Rect paletteRect = RectTools.ToScreen(paletteWindow.GetComponent<RectTransform>());
            if (paletteRect.Contains(Input.mousePosition))
            {
                if (!nstartDrag)
                    nstartDrag = GetTargetBoard(Input.mousePosition, out targetBoard);
                if (nstartDrag)
                    targetBoard.GetComponentInChildren<OnGrid>().Drag();
            }
        }
    }



    static void On_Shape_EndDrag()
    {
        Rect paletteRect = RectTools.ToScreen(paletteWindow.GetComponent<RectTransform>());
        if (Input.GetMouseButtonUp(1))
        {
            if (nstartDrag)
            {
                targetBoard.GetComponentInChildren<OnGrid>().Drag_Exit();
                nstartDrag = false;
            }
            return;
        }

        if (paletteRect.Contains(Input.mousePosition))
        {
            bool inBoards = false;
            Transform targetTra = dragObject;// EventSystem.current.currentSelectedGameObject.transform;
            if (GetTargetBoard(Input.mousePosition, out targetBoard))
            {
                Transform GridTra = targetBoard.Find("Mask").Find("Grid");
                Shape targetShape = targetTra.GetComponent<Shape>();
                if (targetTra.parent != GridTra)
                {
                    RectTransform targetRectTra = targetTra.GetComponent<RectTransform>();
                    Vector3 lastScale = targetRectTra.localScale;
                    if (targetTra.parent.name == "Grid")
                    {
                        BoardPlan lastPlan = targetTra.GetComponentInParent<Board>().plan;
                        GenBoardPlan.RemoveShape(lastPlan, targetShape);
                        //lastPlan
                    }
                    targetTra.SetParent(targetBoard.Find("Mask").Find("Grid"));
                    targetRectTra.localScale = lastScale;
                    targetRectTra.position = Input.mousePosition;

                    BoardPlan targetPlan = targetBoard.GetComponent<Board>().plan;
                    targetTra.SetAsLastSibling();
                    GenBoardPlan.ResetOrders(targetPlan);
                    targetShape.order = targetTra.GetSiblingIndex();
                    targetTra.GetComponent<Shape>().id = targetPlan.shapeIDs.Count == 0 ?
                        1 : targetPlan.shapeIDs[targetPlan.shapeIDs.Count - 1] + 1;
                    if (targetShape.GetType() == typeof(Primitive))
                    {
                        GenBoardPlan.AddNewPrimitive(targetPlan, targetTra.GetComponent<Primitive>());
                        SetPrimitiveComponents.ResetTriggers(targetTra.GetComponent<EventTrigger>(), targetPlan.board);
                    }
                    else if (targetShape.GetType() == typeof(Part))
                    {
                        GenBoardPlan.AddNewPart(targetPlan, targetTra.GetComponent<Part>());
                        SetPartComponents.ResetTriggers(targetTra.GetComponent<EventTrigger>(), targetPlan.board);
                    }
                    else if (targetShape.GetType() == typeof(Background))
                    {
                        GenBoardPlan.AddNewBackground(targetPlan, targetTra.GetComponent<Background>());
                        SetBackgroundComponents.ResetTriggers(targetTra.GetComponent<EventTrigger>(), targetPlan.board);
                    }
                }
                inBoards = true;
            }

            if (!inBoards)
            {
                if (targetTra.parent.name == "Grid")
                {
                    BoardPlan lastPlan = targetTra.GetComponentInParent<Board>().plan;
                    GenBoardPlan.RemoveShape(lastPlan, targetTra.GetComponent<Shape>());
                    //lastPlan
                }

                Transform boardTra = paletteWindow.Find("OuterParts");
                if (targetTra.parent != boardTra)
                {
                    Vector3 lastScale = targetTra.localScale;
                    targetTra.SetParent(boardTra);
                    targetTra.localScale = lastScale;
                    targetTra.position = Input.mousePosition;
                }
            }
        }
        else
        {
            //put shape on border
        }
    }

    public static bool GetTargetBoard(Vector2 input, out Transform targetBoard)
    {
        Transform paletteWindow = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().paletteWindow;
        BoardPlans.ordersList.Sort();
        for (int i = BoardPlans.ordersList.Count-1; i >=0; i--)
        {
            Board next = BoardPlans.inOrders[BoardPlans.ordersList[i]].board;
            if (RectTools.ToScreen(next.GetComponent<RectTransform>()).Contains(Input.mousePosition))
            {
                targetBoard = next.transform;
                return true;
            }
        }
        targetBoard = paletteWindow;
        return false;
    }
}
