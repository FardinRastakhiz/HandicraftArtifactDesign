///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <CopyShape>
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

public class CopyShape : MonoBehaviour
{
    static Transform paletteWindow;
    public static Transform parent;
    static bool isInBoard;
    static BoardPlan activePlan;
    public static void On_Source_Shape_Click()
    {
        ToolsUI tools = UIController.tools.GetComponent<ToolsUI>();
        tools.CustomSelect(tools.transform.Find("SELECT").gameObject);

        GameObject shape = EventSystem.current.currentSelectedGameObject;
        CreateUI(CreatePrimitive(shape), shape.GetComponent<RectTransform>());
    }

    public static void On_Source_Background_Click()
    {
        ToolsUI tools = UIController.tools.GetComponent<ToolsUI>();
        tools.CustomSelect(tools.transform.Find("SELECT").gameObject);

        GameObject backGround = EventSystem.current.currentSelectedGameObject;
        GameObject bgObj = CreateBackground(backGround);
        CreateUI(bgObj, backGround.GetComponent<RectTransform>());
        bgObj.transform.SetAsLastSibling();
        if (isInBoard) 
            GenBoardPlan.ResetOrders(activePlan);
        else
            bgObj.GetComponent<Background>().order = bgObj.transform.GetSiblingIndex();
        bgObj.GetComponent<RectTransform>().sizeDelta = new Vector2(512,512);
    }

    public static void On_Sidebar_Click()
    {
        ToolsUI tools = UIController.tools.GetComponent<ToolsUI>();
        tools.CustomSelect(tools.transform.Find("SELECT").gameObject);

        GameObject shape = EventSystem.current.currentSelectedGameObject;
        if (shape.name == "Sidebar")
        {
            CreateUI(CreateSidePart(shape), shape.GetComponent<RectTransform>());
        }
    }


    static GameObject CreatePrimitive(GameObject shape)
    {
        SetParent();
        GameObject newObj = new GameObject();
        newObj.name = "primitive";
        newObj.transform.SetParent(parent);
        newObj.AddComponent<Primitive>();
        Primitive prim = newObj.GetComponent<Primitive>();
        prim.color = Color.white;
        prim.transform2D = new Transform2D();
        prim.transform2D.position = Vector2.zero;
        prim.transform2D.rotation = 0;
        prim.transform2D.size = 1;
        UnityEngine.UI.Image image = shape.GetComponent<UnityEngine.UI.Image>();
        prim.size = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
        prim.id = 0;
        prim.sourceShape = shape.transform.GetSiblingIndex();
        SetPrimitiveComponents.SetSinglePrimitive(prim, parent.parent.parent);
        prim.transform.SetAsLastSibling();
        if (isInBoard)
        {
            prim.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
            prim.id = activePlan.shapeIDs.Count == 0 ? 1 : activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
            GenBoardPlan.ResetOrders(activePlan);
            prim.order = prim.transform.GetSiblingIndex();
            GenBoardPlan.AddNewPrimitive(activePlan, newObj.GetComponent<Primitive>());
        }
        return newObj;
    }

    static GameObject CreateBackground(GameObject backGround)
    {
        SetParent();
        GameObject newObj = new GameObject();

        newObj.name = "background";
        newObj.transform.SetParent(parent);
        newObj.AddComponent<Background>();
        Background bg = newObj.GetComponent<Background>();
        bg.color = Color.white;
        bg.transform2D = new Transform2D();
        bg.transform2D.position = Vector2.zero;
        bg.transform2D.rotation = 0;
        bg.transform2D.size = 1;

        UnityEngine.UI.Image image = backGround.GetComponent<UnityEngine.UI.Image>();
        bg.size = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
        bg.id = 0;
        bg.sourceShape = backGround.transform.GetSiblingIndex();
        SetBackgroundComponents.SetSingleBackground(bg, parent.parent.parent);
        if (isInBoard)
        {
            bg.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
            bg.id = activePlan.shapeIDs.Count == 0 ? 1 : activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
            bg.transform.SetAsLastSibling();
            GenBoardPlan.ResetOrders(activePlan);
            bg.order = bg.transform.GetSiblingIndex();
            GenBoardPlan.AddNewBackground(activePlan, bg);
        }
        return newObj;
    }

    static GameObject CreateSidePart(GameObject shape)
    {
        Sidebar sidebar = shape.GetComponent<Sidebar>();
        GameObject newObj = new GameObject();
        SetParent();
        newObj.transform.SetParent(parent);
        UnityEngine.UI.Image image = shape.GetComponent<UnityEngine.UI.Image>();
        switch (sidebar.shapeType)
        {
            case ShapeType.PART:
                newObj.name = "part";
                newObj.AddComponent<Part>();
				Part part = newObj.GetComponent<Part>();
				SetShapeParameters(part, sidebar);
				part.sourceImage = sidebar.sourceImage;
				part.index = sidebar.indx;
				part.size = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
				part.id = 0;
				SetPartComponents.SetSinglePart(part, parent.parent.parent);
                if (isInBoard)
                {
                    part.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
                    part.id = activePlan.shapeIDs.Count == 0 ? 1 : activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
                    part.transform.SetAsLastSibling();
                    GenBoardPlan.ResetOrders(activePlan);
                    part.order = part.transform.GetSiblingIndex();
					GenBoardPlan.AddNewPart(activePlan, part);
                }
                break;
            case ShapeType.PRIMITIVE:
                newObj.name = "primitive";
				newObj.AddComponent<Primitive>();
				Primitive prim = newObj.GetComponent<Primitive>();
				SetShapeParameters(prim, sidebar);
				prim.sourceShape = sidebar.indx;
				prim.size = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
				prim.id = 0;
				SetPrimitiveComponents.SetSinglePrimitive(prim, parent.parent.parent);
                if (isInBoard)
                {
                    prim.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
                    prim.id = activePlan.shapeIDs[activePlan.shapeIDs.Count - 1] + 1;
                    prim.transform.SetAsLastSibling();
                    GenBoardPlan.ResetOrders(activePlan);
                    prim.order = prim.transform.GetSiblingIndex();
					GenBoardPlan.AddNewPrimitive(activePlan, prim);
                }
                break;
        }
        
        return newObj;
    }

    static void SetShapeParameters(Shape shape, Sidebar sideshape)
    {
        shape.color = Color.white;
        shape.transform2D = new Transform2D();
        shape.transform2D.position = sideshape.transform2D.position;
        shape.transform2D.rotation = 0;
        shape.transform2D.size = 1;
        //shape.colorID = LoadBoardPlan.GetID("Colors");
        //shape.transformID = LoadBoardPlan.GetID("Transforms");
    }
    static void SetParent()
    {
        paletteWindow = GameObject.FindGameObjectWithTag("PaletteBoard").transform;//controller.paletteWindow;
        if (GenBoardPlan.isBoardPlan())
        {
            isInBoard = true;
            activePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
            parent = GenBoardPlan.ActiveBoard().transform.Find("Mask").Find("Grid");
        }
        else
        {
            isInBoard = false;
            parent = paletteWindow.Find("OuterParts");
        }
    }

    public static void CreateUI(GameObject shapeInstance, RectTransform ShapeRect)
    {
        shapeInstance.name = "Instance";
        RectTransform newShapeRect = shapeInstance.GetComponent<RectTransform>();
        RectTransform paletteRect = paletteWindow.GetComponent<RectTransform>();
        shapeInstance.transform.SetParent(parent);
        shapeInstance.transform.SetAsLastSibling();

        newShapeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ShapeRect.rect.width);
        newShapeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ShapeRect.rect.height);
        float xRandom = 0;
        float yRandom = 0;
        if (isInBoard)
        {
            Vector3 pos1 = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0) - parent.transform.position;
            pos1 = new Vector3(pos1.x / parent.transform.lossyScale.x, pos1.y / parent.transform.lossyScale.y);
            pos1.x = Mathf.Abs(pos1.x) > 500 ? Mathf.Sign(pos1.x) * 500 : pos1.x;
            pos1.y = Mathf.Abs(pos1.y) > 500 ? Mathf.Sign(pos1.y) * 500 : pos1.y;
            xRandom = pos1.x + UnityEngine.Random.Range(-15, 15);
            yRandom = pos1.y + UnityEngine.Random.Range(-15, 15);

        }
        else
        {
            xRandom = UnityEngine.Random.Range(0, paletteRect.rect.width / 3);
            yRandom = UnityEngine.Random.Range(-paletteRect.rect.height / 3, paletteRect.rect.height / 3);
        }
        newShapeRect.localPosition = new Vector3(xRandom, yRandom, 0);
        newShapeRect.localScale = new Vector3(1, 1, 1);
    }
}
