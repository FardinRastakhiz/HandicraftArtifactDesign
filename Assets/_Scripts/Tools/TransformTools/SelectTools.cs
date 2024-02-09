///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <SelectTools> <SelectComponents> <SelectContainer>
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
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class SelectTools : Transforms {
    public static Transform currentBoard;
    public static HashSet<Shape> lastShapes;
    static HashSet<GameObject> lastObjects;

    public static HashSet<Shape> newShapes;
    static HashSet<GameObject> newObjects;
    static SelectTools thisObject;
    static int activeBoard = 0;
    static Fardin.ColorTools.ColorPick colorPick;
    public static int ActiveBoard
    {
        get
        {
            return activeBoard;
        }
        set
        {
            if (activeBoard != value)
            {
                activeBoard = value;
                if (activeBoard!=-1)
                    currentBoard = BoardPlans.boardPlans[activeBoard].board.transform;
                else
                    currentBoard = null;
                SelectContainer.Start(lastShapes);
                SelectContainer.GetSelects(lastShapes, activeBoard);
                ResetTotal();
                SelectCustomShapes(lastShapes);
            }
        }
    }

    public static void Start(ref Transforms transforms)
    {
        if (thisObject == null)
            thisObject = new SelectTools();
        InitializeProperties();

        if (transforms != null)
            transforms.ResetTools();

        transforms = thisObject;
        SelectContainer.Start(lastShapes);


        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        for (int i = 0; i < selectedList.Count; i++)
        {
            selectedList[i].gameObject.transform.SetSiblingIndex(selectedList[i].order);
        }
        foreach (var item in lastObjects)
        {
            item.SetActive(true);
        }
    }


    public override void ResetTools()
    {
        foreach (var item in lastObjects)
        {
            if (item == null)
            {
                lastObjects.Remove(item);
                continue;
            }
            item.SetActive(false);
        }
        string parentName = "";
        foreach (var item in lastShapes)
        {
            if (item == null)
            {
                lastShapes.Remove(item);
                continue;
            }
            if (parentName =="")
                parentName = item.gameObject.transform.parent.name;
            if (parentName != "Grid" && parentName != "OuterParts")
                item.gameObject.transform.SetParent(item.gameObject.transform.parent.parent);
        }
    }

    

    public override void On_Shape_Click()
    {
        /*
        if (!GameObject.FindGameObjectWithTag("Tools"))
        {
            On_Deselect();
            return;
        }
        ColorPicker colorPicker = GameObject.FindGameObjectWithTag("Tools").transform.Find("ColorPick").GetComponent<ColorPicker>();
        if (colorPicker.mState == ColorPicker.ESTATE.Hidden)
            On_Select();*/

        On_Select();
    }

    

    public override void On_Shape_Begin_Drag()
    {
        On_Select();
    }

    public override void On_Free_Area_Click()
    {
        if (!UIController.tools)
        {
            On_Deselect();
            return;
        }
        /*ColorPicker colorPicker = GameObject.FindGameObjectWithTag("Tools").transform.Find("ColorPick").GetComponent<ColorPicker>();
        if (colorPicker.mState == ColorPicker.ESTATE.Hidden)
            On_Deselect();*/
        On_Deselect();
    }

    public override void On_Free_Area_Begin_Drag(Transform board)
    {
        On_Begin_Area_Select(board);
    }

    public override void On_Free_Area_Drag(BoardPlan plan)
    {
        On_Area_Select(plan);
    }

    public override void On_Drag_Exit()
    {
        if (Input.GetMouseButtonUp(0))
        {
            On_End_Area_Select();
        }
    }
    void On_Select()
    {
        InitializeProperties();
        if (Input.GetMouseButton(0)||Input.GetMouseButtonUp(0))
        {
            if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                ResetTotal();
            }
            Shape selected = EventSystem.current.currentSelectedGameObject.GetComponent<Shape>();
            //colorPick.CutomChange(selected.gameObject.GetComponent<Image>().color);
            lastShapes.Add(selected);
            lastObjects.Add(Sets(selected));
            if (selected.GetType()==typeof(Background))
                selected.transform.Find("borderControl").gameObject.SetActive(true);
        }
    }
    public static void SelectAll()
    {
        if (BoardPlans.ActiveIndex != -1)
        {
            ResetTotal();
            BoardPlan plan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
            int size = plan.parts.Count;
            for (int i = 0; i < size; i++)
            {
                lastShapes.Add(plan.parts[i]);
                lastObjects.Add(Sets(plan.parts[i]));
            }
            size = plan.primitives.Count;
            for (int i = 0; i < size; i++)
            {
                lastShapes.Add(plan.primitives[i]);
                lastObjects.Add(Sets(plan.primitives[i]));
            }
            size = plan.backgrounds.Count;
            for (int i = 0; i < size; i++)
            {
                lastShapes.Add(plan.backgrounds[i]);
                lastObjects.Add(Sets(plan.backgrounds[i]));
            }
        }
        else
        {
            //...
        }
    }

    void CheckSelectBoard(Transform selecBoard )
    {
        if (selecBoard != currentBoard)
        {
            currentBoard = selecBoard;
            ResetNew();
            ResetTotal();
        }
    }
    void On_Deselect()
    {
        InitializeProperties();
        ResetTotal();
    }
    public static void InitializeProperties()
    {
        if (lastShapes == null)
            lastShapes = new HashSet<Shape>();
        if (lastObjects == null)
            lastObjects = new HashSet<GameObject>();
        if (newShapes == null)
            newShapes = new HashSet<Shape>();
        if (newObjects == null)
            newObjects = new HashSet<GameObject>();
        if (currentBoard == null && GenBoardPlan.isBoardPlan())
        {
            currentBoard = GenBoardPlan.ActiveBoard().transform;
        }
        if (colorPick == null)
        {
            colorPick = UIController.tools.transform.Find("PickTools").GetComponent<Fardin.ColorTools.ColorPick>();
        }
    }


    public static void RefineSelection(Transform shape)
    {
        if (lastShapes.Count > 0)
        {
            if (!lastShapes.Contains(shape.GetComponent<Shape>()))
            {
                ResetTotal();
                lastShapes.Add(shape.GetComponent<Shape>());
            }
        }
        else
        {
            lastShapes.Add(shape.GetComponent<Shape>());
        }
    }

    RectTransform Grid;
    GameObject selectArea;
    Vector3 mouseStart;
    Vector2 localRectStart;

    void On_Begin_Area_Select(Transform board)
    {
        InitializeProperties();
        CheckSelectBoard(board);
        if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            ResetTotal();
        }

        mouseStart = Input.mousePosition;
        Grid = board.Find("Mask").Find("Grid").GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Grid, mouseStart
            , Grid.GetComponentInParent<Canvas>().GetComponent<Camera>(), out localRectStart);
        SelectComponents.CreateObject(out selectArea);
        selectArea.transform.SetParent(Grid);
    }
    void On_Area_Select(BoardPlan plan)
    {
        if (selectArea==null)
            On_Begin_Area_Select(plan.board.transform);
        Vector2 localRectEnd;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Grid, Input.mousePosition
            , Grid.GetComponentInParent<Canvas>().GetComponent<Camera>(), out localRectEnd);
        RectTransform areaRect = selectArea.GetComponent<RectTransform>();
        areaRect.sizeDelta = new Vector2(Mathf.Abs(Input.mousePosition.x - mouseStart.x),
            Mathf.Abs(Input.mousePosition.y - mouseStart.y));
        areaRect.localPosition = new Vector3(Mathf.Min(localRectStart.x, localRectEnd.x) + areaRect.sizeDelta.x / (2 * Grid.lossyScale.x),
            Mathf.Min(localRectStart.y, localRectEnd.y) + areaRect.sizeDelta.y / (2 * Grid.lossyScale.y));
        Rect area = areaRect.rect;
        area.center = areaRect.TransformPoint(area.center);
        area.size = areaRect.TransformVector(area.size);
            //new Rect(Mathf.Min(localRectStart.x, localRectEnd.x), Mathf.Min(localRectStart.y, localRectEnd.y),
            //Mathf.Abs(localRectEnd.x - localRectStart.x), Mathf.Abs(localRectEnd.y - localRectStart.y));

        ResetNew();
        
        for (int i = 0; i < plan.parts.Count; i++)
        {
            if (area.Contains(plan.parts[i].gameObject.GetComponent<RectTransform>().position))
            {
                newShapes.Add(plan.parts[i]);
                newObjects.Add(Sets(plan.parts[i]));
            }
        }
        for (int i = 0; i < plan.primitives.Count; i++)
        {
            if (area.Contains(plan.primitives[i].gameObject.GetComponent<RectTransform>().position))
            {
                newShapes.Add(plan.primitives[i]);
                newObjects.Add(Sets(plan.primitives[i]));
            }
        }
        for (int i = 0; i < plan.backgrounds.Count; i++)
        {
            if (area.Contains(plan.backgrounds[i].gameObject.GetComponent<RectTransform>().position))
            {
                newShapes.Add(plan.backgrounds[i]);
                newObjects.Add(Sets(plan.backgrounds[i]));
            }
        }
    }

    void On_End_Area_Select()
    {
        if (selectArea != null)
            SelectComponents.TerminateObject(selectArea);
        foreach (var item in newShapes)
            lastShapes.Add(item);
        foreach (var item in newObjects)
            lastObjects.Add(item);

        newObjects.ExceptWith(lastObjects);
        newShapes.ExceptWith(lastShapes);
    }

    void ResetNew()
    {
        foreach (var item in newShapes)
        {
            if (!lastShapes.Contains(item))
            {
                item.selected = false;
            }
        }
        foreach (var item in newObjects)
        {
            if (!lastObjects.Contains(item))
            {
                GameObject oldObj = item;
                SelectComponents.TerminateObject(oldObj);
                //newObjects.Remove(item);
            }
        }
        newShapes = new HashSet<Shape>();
        newObjects = new HashSet<GameObject>();

    }

    public static bool ResetTotal()
    {
        try
        {
            //if (!GameObject.FindGameObjectWithTag("Tools"))
            //{
            //    ToolsUtility.SetState(ToolsState.SELECT);
            //    return ResetHashSets();
            //}
            ToolsUtility.SetToolsState(ToolsState.SELECT,
                UIController.tools.transform.Find("SELECT").gameObject);
            return ResetHashSets();
        }
        catch (System.Exception)
        {
            return false;
        }
        
    }

    public static bool ResetHashSets()
    {
        try
        {
            bool completeDone = true;
            //if (lastShapes.Count==1)
            //    foreach (var item in lastShapes)
            //        if (item.GetType()==typeof(Background))
            //            item.transform.Find("borderControl").gameObject.SetActive(false);

            foreach (var item in lastShapes)
            {
                item.selected = false;
                if (item.GetType() == typeof(Background))
                    item.transform.Find("borderControl").gameObject.SetActive(false);
            }
            foreach (var item in lastObjects)
            {
                GameObject oldObj = item;
                completeDone = SelectComponents.TerminateObject(oldObj);
                //lastObjects.Remove(item);
            }
            lastShapes = new HashSet<Shape>();
            lastObjects = new HashSet<GameObject>();
            return completeDone;
        }
        catch (System.Exception)
        {
            return false;
        }
    }


    public static void SelectCustomShape(Shape customShape)
    {
        lastShapes.Add(customShape);
        lastObjects.Add(Sets(customShape));
    }
    public static void SelectCustomShapes(HashSet<Shape> customShapes)
    {
        foreach (var item in customShapes)
            SelectCustomShape(item);
    }

    static GameObject Sets(Shape selected)
    {
        //selected.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        selected.selected = true;
        GameObject newObj = new GameObject("SelectObject");
        newObj.transform.SetParent(selected.gameObject.transform);
        SelectComponents.Set(newObj);
        return newObj;
    }
}

public class SelectComponents: MonoBehaviour
{
    public static void CreateObject(out GameObject obj)
    {
        obj = Instantiate((Object)ShapeCenter.areaShape) as GameObject;
    }
    public static bool TerminateObject(GameObject obj)
    {
        Destroy(obj);
        return true;
    }
    public static void Set(GameObject selectObj){
        AddComponents(selectObj);
        SetComponents(selectObj);
    }
    static void AddComponents(GameObject selectObj)
    {
        selectObj.AddComponent<RectTransform>();
        selectObj.AddComponent<Image>();
    }
    static void SetComponents(GameObject obj)
    {
        SetRectTransform(obj);
        SetImage(obj.GetComponent<Image>());
    }

    static void SetRectTransform(GameObject obj)
    {
        var parent = obj.transform.parent;
        obj.transform.SetParent(parent.parent);
        var rectTra = obj.GetComponent<RectTransform>();
        var parentRect = parent.GetComponent<RectTransform>();
        RectTools.Copy(parentRect, rectTra);
        obj.transform.SetParent(parent);
        //rectTra.sizeDelta = new Vector2(parentRect.rect.width + 0, parentRect.rect.height + 0);
        rectTra.sizeDelta = new Vector2(0, 0);
        rectTra.anchorMin = Vector2.zero;
        rectTra.anchorMax = Vector2.one;
    }

    static void SetImage(Image img)
    {
        img.sprite = ShapeCenter.border;
        img.color = new Color(0, 1.0f, 1.0f, 1.0f);
        img.type = Image.Type.Tiled;
    }
}

public class SelectContainer
{
    static Dictionary<int, HashSet<Shape>> allSelects;
    static int lastActiveBoard;

    public static void Start(HashSet<Shape> Selects)
    {
        if (allSelects == null)
        {
            allSelects = new Dictionary<int, HashSet<Shape>>();
            allSelects.Add(-1, Selects);
            lastActiveBoard = -1;
        }
    }

    public static void GetSelects(HashSet<Shape> Selects, int activeBoard)
    {
        Debug.Log(activeBoard + " : " + lastActiveBoard);
        allSelects[lastActiveBoard] = Selects;
        Selects = allSelects[activeBoard];
        lastActiveBoard = activeBoard;
    }
    public static void UpdateSelects(HashSet<Shape> Selects, int activeBoard)
    {
        allSelects[activeBoard] = Selects;
        lastActiveBoard = activeBoard;
    }
    public static void AddNewSelects(HashSet<Shape> Selects, int activeBoard)
    {
        if (allSelects.ContainsKey(activeBoard))
        {
            UpdateSelects(Selects, activeBoard);
            return;
        }
        allSelects.Add(activeBoard, Selects);
        lastActiveBoard = activeBoard;
    }

    public static void RemoveSelects(int activeBoard)
    {
        if (allSelects.ContainsKey(activeBoard))
        {
            allSelects.Remove(activeBoard);
            lastActiveBoard = -1;
        }
    }
}