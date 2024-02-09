///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <DuplicateShapes>
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
using System.Linq;
public class DuplicateShapes : MonoBehaviour {
    static Transform parent;
    public static HashSet<Shape> duplicatedShapes;
    static bool isInBoard;
    static BoardPlan activePlan;
    public static void Duplicate()
    {
        duplicatedShapes = new HashSet<Shape>();
        isInBoard = false;
        if (BoardPlans.ActiveIndex!=-1)
        {
            activePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
            isInBoard = true;
            ToolsUI tools = UIController.tools.GetComponent<ToolsUI>();
            tools.CustomSelect(tools.transform.Find("SELECT").gameObject);
        }

        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();

        for (int i = 0; i < selectedList.Count; i++)
        {
            GameObject newItem = CreateDuplicate(selectedList[i]);
            CreateUI(newItem, selectedList[i].GetComponent<RectTransform>());
            duplicatedShapes.Add(newItem.GetComponent<Shape>());
        }

        SelectTools.ResetTotal();
        SelectTools.SelectCustomShapes(duplicatedShapes);
    }
    public static void TransformDuplicate()
    {
        duplicatedShapes = new HashSet<Shape>();
        isInBoard = false;
        if (BoardPlans.ActiveIndex != -1)
        {
            activePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
            isInBoard = true;
        }
        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        for (int i = 0; i < selectedList.Count; i++)
        {
            GameObject newItem = CreateDuplicate(selectedList[i]);
            CreateUI(newItem, selectedList[i].GetComponent<RectTransform>());
            duplicatedShapes.Add(newItem.GetComponent<Shape>());
            newItem.GetComponent<RectTransform>().position -= new Vector3(20, 20, 0);
            newItem.GetComponent<Shape>().transform2D.position -= new Vector2(20, 20);
        }
        SelectTools.ResetHashSets();
        SelectTools.SelectCustomShapes(duplicatedShapes);
    }

    static GameObject CreateDuplicate(Shape shape)
    {
        GameObject newObj = new GameObject();
        //controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
        parent = shape.transform.parent;
        newObj.transform.SetParent(parent);
        UnityEngine.UI.Image image = shape.GetComponent<UnityEngine.UI.Image>();
        if (shape.GetType()==typeof(Part))
        {
            newObj.name = "part";
            newObj.AddComponent<Part>();
            Part part = newObj.GetComponent<Part>();
            SetShapeParameters(part, shape);
            part.sourceImage = (shape as Part).sourceImage;
            part.index = (shape as Part).index;
            part.size = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
            part.id = 0;
            SetPartComponents.SetSinglePart(part, parent.parent.parent);
            if (isInBoard)
            {
                part.id = activePlan.shapeIDs.Count == 0 ? 1 : activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
                part.transform.SetAsLastSibling();
                GenBoardPlan.ResetOrders(activePlan);
                part.order = part.transform.GetSiblingIndex();
                GenBoardPlan.AddNewPart(activePlan, part);
            }
        }
        else if (shape.GetType()==typeof(Primitive))
        {
            newObj.name = "primitive";
            newObj.AddComponent<Primitive>();
            Primitive prim = newObj.GetComponent<Primitive>();
            SetShapeParameters(prim, shape);
            prim.sourceShape = (shape as Primitive).sourceShape;
            prim.size = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
            prim.id = 0;
            SetPrimitiveComponents.SetSinglePrimitive(prim, parent.parent.parent);
            if (isInBoard)
            {
                prim.id = activePlan.shapeIDs.Count == 0 ? 1 : activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
                prim.transform.SetAsLastSibling();
                GenBoardPlan.ResetOrders(activePlan);
                prim.order = prim.transform.GetSiblingIndex();
                GenBoardPlan.AddNewPrimitive(activePlan, prim);
            }
        }
        else if (shape.GetType() == typeof(Background))
        {

            newObj.name = "background";
            newObj.AddComponent<Background>();
            Background bg = newObj.GetComponent<Background>();
            SetShapeParameters(bg, shape);
            bg.sourceShape = (shape as Background).sourceShape;
            bg.size = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
            bg.id = 0;
            SetBackgroundComponents.SetSingleBackground(bg, parent.parent.parent);
            if (isInBoard)
            {
                bg.id = activePlan.shapeIDs.Count == 0 ? 1 : activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
                bg.transform.SetAsLastSibling();
                GenBoardPlan.ResetOrders(activePlan);
                bg.order = bg.transform.GetSiblingIndex();
                GenBoardPlan.AddNewBackground(activePlan, bg);
            }
        } 
        return newObj;
    }


    static void SetShapeParameters(Shape copy, Shape original)
    {
        copy.color = original.color;
        copy.transform2D = new Transform2D();
        copy.transform2D.position = original.transform2D.position+new Vector2(20,20);
        copy.transform2D.rotation = original.transform2D.rotation;
        copy.transform2D.size = original.transform2D.size;
        //shape.colorID = LoadBoardPlan.GetID("Colors");
        //shape.transformID = LoadBoardPlan.GetID("Transforms");
    }


    public static void CreateUI(GameObject shapeInstance, RectTransform ShapeRect)
    {
        shapeInstance.name = "Instance";
        RectTransform newShapeRect = shapeInstance.GetComponent<RectTransform>();

        shapeInstance.transform.SetParent(ShapeRect.parent);
        newShapeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ShapeRect.rect.width);
        newShapeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ShapeRect.rect.height);
        //float xRandom = UnityEngine.Random.Range(0, paletteRect.rect.width / 3);
        //float yRandom = UnityEngine.Random.Range(-paletteRect.rect.height / 3, paletteRect.rect.height / 3);
        newShapeRect.position = ShapeRect.position + new Vector3(20, 20,0);
        newShapeRect.localScale = ShapeRect.localScale;
        newShapeRect.rotation = ShapeRect.rotation;
    }
}
