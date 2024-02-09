///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <GenBoardPlan>
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

public class GenBoardPlan {

    
    public static bool isBoardPlan()
    {
        return BoardPlans.ActiveIndex != -1;
    }

    public static void SetBoardPlanIndex(BoardPlan plan)
    {
        BoardPlans.ActiveIndex = BoardPlans.boardPlans.IndexOf(plan);
        //boardPlans[ActiveIndex].board.transform.SetAsLastSibling();
        //boardPlans[ActiveIndex].board.transform.parent.Find("ShowButtons").SetAsLastSibling();
    }
    public static Board ActiveBoard()
    {
        if (BoardPlans.ActiveIndex == -1)
            return null;
        return BoardPlans.boardPlans[BoardPlans.ActiveIndex].board;
    }
    public static BoardPlan Generate(Plan plan)
    {
        //if ((boardPlans.Count < 2))
        //{
        for (int i = 0; i < BoardPlans.boardPlans.Count; i++)
            {
                if ((BoardPlans.boardPlans[i].name == plan.name))
                    return BoardPlans.boardPlans[i];
            }
            int indx = TakeBoardPlan(plan);
            BoardPlans.boardPlans[indx].plan = plan;
            CreateDesignBoard(indx);
            TakeDesignElements(indx);
            CreateUIs();
            ResetOrders(BoardPlans.boardPlans[indx]);
            SelectContainer.AddNewSelects(new HashSet<Shape>(), indx);
            BoardPlans.ActiveIndex = indx;
            return BoardPlans.boardPlans[indx];
        //}
    }

    public static bool Close(int i)
    {
        //SaveBoardPlan.Save(boardPlans[i]);
        HideBoards.HideBehind(nextFullScreen(BoardPlans.boardPlans[i].order));

        BoardPlans.inOrders.Remove(BoardPlans.boardPlans[i].order);
        BoardPlans.ordersList.Remove(BoardPlans.boardPlans[i].order);

        BoardPlans.boardPlans.RemoveAt(i);
        BoardPlans.ActiveIndex = -1;
        //if (boardPlans.Count > 0)
        //    boardPlans[0].board.Index = 0;
        return true;
    }

    public static bool SavePlan(BoardPlan plan)
    {
        ResetOrders(plan);
        SaveBoardPlan.Save(plan);
        return true;
    }
    public static int nextFullScreen(int lastFull)
    {
        BoardPlans.ordersList.Sort();
        for (int i = BoardPlans.ordersList.IndexOf(lastFull) - 1; i >= 0; i--)
        {
            if (BoardPlans.inOrders[BoardPlans.ordersList[i]].board.transform.GetComponent<Window>().IsFullScreen)
            {
                return BoardPlans.ordersList[i];
            }
        }
        return 0;
    }
    
    static int TakeBoardPlan(Plan plan)
    {
        BoardPlans.boardPlans.Add(LoadBoardPlan.Load(plan.name));
        return BoardPlans.boardPlans.Count - 1;
    }
    static void CreateDesignBoard(int i)
    {
        GameObject newBoard = (GameObject)MonoBehaviour.Instantiate(
            GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().designBoards);
        newBoard.name = "Design Board";
        newBoard.transform.SetParent(GameObject.FindGameObjectWithTag("PaletteBoard").transform);
        newBoard.transform.parent.Find("ShowButtons").SetAsLastSibling();
        newBoard.transform.Find("ControlWindow").Find("Drag").Find("Text").
            GetComponent<UnityEngine.UI.Text>().text = BoardPlans.boardPlans[i].name;
        BoardPlans.boardPlans[i].board = newBoard.GetComponent<Board>();
        BoardPlans.boardPlans[i].board.UIObject = newBoard;
        //BoardPlans.boardPlans[i].board.Index = i;
        BoardPlans.boardPlans[i].board.SetBoardRect();
        BoardPlans.boardPlans[i].board.plan = BoardPlans.boardPlans[i];
        BoardPlans.boardPlans[i].transform.SetParent(newBoard.transform);
        UnityEngine.UI.Image boardCanvas = newBoard.transform.Find("Mask").Find("Grid").Find("DrawCanvas").GetComponent<UnityEngine.UI.Image>();
        boardCanvas.sprite = ShapeCenter.boardCanvas[BoardPlans.boardPlans[i].Canvas];
        boardCanvas.color = BoardPlans.boardPlans[i].CanvasColor;

        BoardPlans.ordersList.Sort();
        BoardPlans.boardPlans[i].order = BoardPlans.ordersList.Count == 0 ? 1 : BoardPlans.ordersList[BoardPlans.ordersList.Count - 1] + 1;

        BoardPlans.ordersList.Add(BoardPlans.boardPlans[i].order);
        BoardPlans.inOrders.Add(BoardPlans.boardPlans[i].order, BoardPlans.boardPlans[i]);

    }

    static void TakeDesignElements(int i)
    {
        new GenPlanPrimitives(BoardPlans.boardPlans[i]);
        new GenPlanParts(BoardPlans.boardPlans[i]);
        new GenPlanBackgrounds(BoardPlans.boardPlans[i]);
        GenSidebar.Generate(BoardPlans.boardPlans[i]);
        SetShapesPriority.SetTotalShapesPriority(BoardPlans.boardPlans[i]);
    }

    static void CreateUIs()
    {
        new PlanComponents();
        for (int i = 0; i < BoardPlans.boardPlans.Count; i++)
        {
            //BoardPlans.boardPlans[i].gameObject = planComponents.createShapeObject(plans[i].name, plans[i].sourceImage);
        }
    }

    public static void AddNewPart(BoardPlan plan, Part part)
    {
        plan.indexInOrder.Add(part.order);
        plan.orders.Add(part.order, part);
        plan.shapeIDs.Add(part.id);
        plan.parts.Add(part);
        //SaveBoardPlan.AddNewPart(plan.name + "_parts", part, true);

    }

    public static void AddNewPrimitive(BoardPlan plan, Primitive primitive)
    {
        //int size = plan.indexInOrder.Count;
        //primitive.order = plan.indexInOrder.Count == 0 ? 1 : plan.indexInOrder[size - 1] + 1;
        //primitive.transform.SetAsLastSibling();
        plan.indexInOrder.Add(primitive.order);
        plan.orders.Add(primitive.order, primitive);
        plan.shapeIDs.Add(primitive.id);
        plan.primitives.Add(primitive);
        //SaveBoardPlan.AddNewPrimitive(plan.name + "_primitives", primitive, true);
    }
    public static void AddNewBackground(BoardPlan plan, Background background)
    {
        //int size = plan.indexInOrder.Count;
        //background.order = plan.indexInOrder.Count == 0 ? 1 : plan.indexInOrder[size-1] + 1;
        plan.indexInOrder.Add(background.order);
        plan.orders.Add(background.order, background);
        plan.backgrounds.Add(background);
        plan.shapeIDs.Add(background.id);
        //SaveBoardPlan.AddNewBackground(plan.name + "_primitives", background, true);
    }
    public static void RemoveShape(BoardPlan plan, Shape shape)
    {
        plan.shapeIDs.Remove(shape.id);
        plan.indexInOrder.Sort();
        plan.orders.Remove(shape.order);
        plan.indexInOrder.Remove(shape.order);
        if (shape.GetType()==typeof(Part))
        {
            removePart(plan, (Part)shape);
        }
        else if (shape.GetType() == typeof(Primitive))
        {
            removePrimitive(plan, (Primitive)shape);
        }
    }
    static void removePart(BoardPlan plan, Part part)
    {
        plan.parts.Remove(part);
        ResetOrders(plan);
        SaveBoardPlan.ReSaveParts(plan.name + "_parts", plan.parts);
    }
    static void removePrimitive(BoardPlan plan, Primitive primitive)
    {
        plan.primitives.Remove(primitive);
        ResetOrders(plan);
        SaveBoardPlan.ReSavePrimitives(plan.name + "_primitives", plan.primitives);
    }


    public static void ResetOrders(BoardPlan plan)
    {
        plan.orders = new Dictionary<int, Shape>();
        plan.indexInOrder = new List<int>();
        for (int i = plan.parts.Count - 1; i >= 0; i--)
        {
            plan.parts[i].order = plan.parts[i].transform.GetSiblingIndex();
            plan.indexInOrder.Add(plan.parts[i].order);
            plan.orders.Add(plan.parts[i].order, plan.parts[i]);
        }
        for (int i = plan.primitives.Count - 1; i >= 0; i--)
        {
            plan.primitives[i].order = plan.primitives[i].transform.GetSiblingIndex();
            plan.indexInOrder.Add(plan.primitives[i].order);
            plan.orders.Add(plan.primitives[i].order, plan.primitives[i]);
        }
        for (int i = plan.backgrounds.Count - 1; i >= 0; i--)
        {
            plan.backgrounds[i].order = plan.backgrounds[i].transform.GetSiblingIndex();
            plan.indexInOrder.Add(plan.backgrounds[i].order);
            plan.orders.Add(plan.backgrounds[i].order, plan.backgrounds[i]);
        }
        plan.indexInOrder.Sort();
    }

    public static void UpdateActiveBoard(Rect paletteBoard)
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (paletteBoard.Contains(Input.mousePosition))
            {
                for (int i = BoardPlans.boardPlans.Count - 1; i >= 0; i--)
                {
                    Rect rect = RectTools.ToScreen(
                        BoardPlans.boardPlans[i].board.GetComponent<RectTransform>());
                    if (rect.Contains(Input.mousePosition))
                    {
                        BoardPlans.ActiveIndex = i;
                        //BoardPlans.boardPlans[ActiveIndex].board.transform.SetAsLastSibling();
                        //BoardPlans.boardPlans[ActiveIndex].board.transform.parent.Find("ShowButtons").SetAsLastSibling();
                        return;
                    }
                }
                BoardPlans.ActiveIndex = -1;
            }
        }
    }

    public static void OnPaletteBoardClicked(Rect paletteBoard)
    {
        if (paletteBoard.Contains(Input.mousePosition))
        {
            for (int i = BoardPlans.boardPlans.Count - 1; i >= 0; i--)
            {
                Rect rect = RectTools.ToScreen(
                    BoardPlans.boardPlans[i].board.GetComponent<RectTransform>());
                if (rect.Contains(Input.mousePosition))
                {
                    BoardPlans.ActiveIndex = i;
                    //BoardPlans.boardPlans[ActiveIndex].board.transform.SetAsLastSibling();
                    //BoardPlans.boardPlans[ActiveIndex].board.transform.parent.Find("ShowButtons").SetAsLastSibling();
                    return;
                }
            }
            BoardPlans.ActiveIndex = -1;
        }
    }

    public static void ResetBoardOrders()
    {
        BoardPlans.ordersList = new List<int>();
        BoardPlans.inOrders = new Dictionary<int, BoardPlan>();
        for (int i = BoardPlans.boardPlans.Count-1; i >=0; i--)
        {
            BoardPlans.boardPlans[i].order = BoardPlans.boardPlans[i].board.transform.GetSiblingIndex();
            BoardPlans.inOrders.Add(BoardPlans.boardPlans[i].order, BoardPlans.boardPlans[i]);
            BoardPlans.ordersList.Add(BoardPlans.boardPlans[i].order);
        }
    }

    //public static void UpdateBoardPlan(Transform curBoard)
    //{
    //    if (boardPlans.Count == 0 || curBoard.name == "OuterParts")
    //        ActiveIndex = 2;
    //    else if (curBoard.name == "Grid")
    //        ActiveIndex = boardPlans.IndexOf(curBoard.parent.parent.GetComponent<Board>().plan);

    //}
}