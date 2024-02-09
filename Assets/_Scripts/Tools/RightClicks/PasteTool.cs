///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <PasteTool>
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

public class PasteTool : ClipBoard
{
    static Transform parent;
    public static Vector3 newMousePosition;
    static Vector3 translation;
    static BoardPlan activePlan;
    static int lastInOrder;


    public static void DropShapes(Vector3 startPos)
    {
        if (ClipBoard.instances == null)
            return;
        if (BoardPlans.ActiveIndex == -1)
        {
            //throw new System.ArgumentException("Can't Copy Shapes Here.");
            ToolsUI tools = UIController.tools.GetComponent<ToolsUI>();
            tools.CustomSelect(tools.transform.Find("SELECT").gameObject);
            return;
        }
        newMousePosition = RightClick.mousePosition;
        Vector3 deviation = Vector3.one * 5;
        numberOfCopies++;
        translation = newMousePosition - startPos + numberOfCopies*deviation;
       
        activePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
        activePlan.indexInOrder.Sort();
        lastInOrder = activePlan.indexInOrder.Count == 0 ? 0 : activePlan.indexInOrder[activePlan.indexInOrder.Count - 1];

        parent = activePlan.board.transform.Find("Mask").Find("Grid");
        GenBoardPlan.ResetOrders(activePlan);
        HashSet<Shape> newShapes = new HashSet<Shape>();

        for (int i = ClipBoard.instances.Count-1; i >=0 ; i--)
        {
            switch (ClipBoard.instances[i].shapeType)
            {
                case ShapeType.PART:
                    GameObject newPart = GeneratePart(ClipBoard.instances[i]);
                    newShapes.Add(newPart.GetComponent<Part>());
                    CreateUI(newPart,ClipBoard.instances[i].rectTransform);
                    break;
                case ShapeType.PRIMITIVE:
                    GameObject newPrim = GeneratePrimitive(ClipBoard.instances[i]);
                    newShapes.Add(newPrim.GetComponent<Primitive>());
                    CreateUI(newPrim, ClipBoard.instances[i].rectTransform);
                    break;
                case ShapeType.BACKGROUND:
                    GameObject newBg = GenerateBackgrounds(ClipBoard.instances[i]);
                    newShapes.Add(newBg.GetComponent<Background>());
                    CreateUI(newBg, ClipBoard.instances[i].rectTransform);
                    break;
            }
        }
        SetShapesPriority.SetTotalShapesPriority(activePlan);
        SelectTools.ResetTotal();
        SelectTools.SelectCustomShapes(newShapes);
    }

    static GameObject GeneratePart(ShapeInstance instance)
    {
        GameObject newObj = new GameObject();
        newObj.name = "part";
        newObj.AddComponent<Part>();
        Part part = newObj.GetComponent<Part>();
        part.color = instance.color;
        part.transform2D = new Transform2D();
        part.transform2D.position = instance.transform2D.position + new Vector2(translation.x, translation.y); 
        part.transform2D.rotation = instance.transform2D.rotation;
        part.transform2D.size = instance.transform2D.size;

        part.sourceImage = instance.sourceImage;
        part.index = instance.indx;
        part.size = instance.size;
        part.order = lastInOrder + instance.order;
        part.id = activePlan.shapeIDs.Count == 0 ? 1 : activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
        SetPartComponents.SetSinglePart(part, activePlan.board.transform);
        GenBoardPlan.AddNewPart(activePlan, part);

        return newObj;
    }

    static GameObject GeneratePrimitive(ShapeInstance instance)
    {
        GameObject newObj = new GameObject();
        newObj.name = "primitive";
        newObj.AddComponent<Primitive>();
        Primitive prim = newObj.GetComponent<Primitive>();
        prim.color = instance.color;
        prim.transform2D = new Transform2D();
        prim.transform2D.position = instance.transform2D.position + new Vector2(translation.x, translation.y);
        prim.transform2D.rotation = instance.transform2D.rotation;
        prim.transform2D.size = instance.transform2D.size;
        prim.size = instance.size;
        prim.sourceShape = instance.indx;
        prim.order = lastInOrder + instance.order;
        prim.id = activePlan.shapeIDs.Count == 0 ? 1 : activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
        SetPrimitiveComponents.SetSinglePrimitive(prim, activePlan.board.transform);
        GenBoardPlan.AddNewPrimitive(activePlan, prim);
        return newObj;
    }
    static GameObject GenerateBackgrounds(ShapeInstance instance)
    {
        GameObject newObj = new GameObject();
        newObj.name = "background";
        newObj.AddComponent<Background>();
        Background bg = newObj.GetComponent<Background>();
        bg.color = instance.color;
        bg.transform2D = new Transform2D();
        bg.transform2D.position = instance.transform2D.position + new Vector2(translation.x, translation.y);
        bg.transform2D.rotation = instance.transform2D.rotation;
        bg.transform2D.size = instance.transform2D.size;
        bg.size = instance.size;
        bg.sourceShape = instance.indx;
        bg.order = lastInOrder + instance.order;
        bg.id = activePlan.shapeIDs.Count == 0 ? 1 : activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
        SetBackgroundComponents.SetSingleBackground(bg, activePlan.board.transform);
        GenBoardPlan.AddNewBackground(activePlan, bg);
        return newObj;
    }

    public static void CreateUI(GameObject shapeInstance, RectTransform ShapeRect)
    {
        shapeInstance.name = "Instance";
        RectTransform newShapeRect = shapeInstance.GetComponent<RectTransform>();
        shapeInstance.transform.SetParent(parent);
        newShapeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ShapeRect.rect.width);
        newShapeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ShapeRect.rect.height);
        newShapeRect.localPosition = ShapeRect.localPosition + translation;
        newShapeRect.localScale = ShapeRect.localScale;
        newShapeRect.rotation = ShapeRect.rotation;
    }
}
