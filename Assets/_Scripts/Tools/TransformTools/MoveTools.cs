///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <MoveTools> <MoveComponents>
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

public class MoveTools : Transforms
{
    static Vector2 startPos;
    static Rect workingRect;
    static Vector2 moveBorder;
    static MoveTools thisObject;

    public static void Start(ref Transforms transforms)
    {
        if (thisObject == null)
            thisObject = new MoveTools();
        if (transforms.GetType() == thisObject.GetType() || SelectTools.lastShapes.Count == 0)
            return;
        if (transforms != null)
            transforms.ResetTools();
        transforms = thisObject;


        
        GetExtermums();
        DrawBorder();
        
        foreach (var item in SelectTools.lastShapes)
            item.order = item.gameObject.transform.GetSiblingIndex();
        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        moveArea.transform.SetSiblingIndex(selectedList[selectedList.Count - 1].transform.GetSiblingIndex()+1);
        for (int i = 0; i < selectedList.Count; i++)
            selectedList[i].gameObject.transform.SetParent(moveArea.transform);

        moveArea.transform.Find("border").SetAsLastSibling();



        //Debug.Log("SelectTools.lastShapes.Count:" + SelectTools.lastShapes.Count);
        //foreach (var item in SelectTools.lastShapes)
        //{
        //    Debug.Log("item:" + item.name);
        //}
    }

    public override void ResetTools()
    {
        string parentName = "";
        foreach (var item in SelectTools.lastShapes)
        {
            if (parentName == "")
                parentName = item.gameObject.transform.parent.name;
            if (parentName != "Grid" && parentName != "OuterParts")
            {
                item.gameObject.transform.SetParent(item.gameObject.transform.parent.parent);
                item.transform.SetAsLastSibling();
            }
        }
        moveArea.transform.SetAsLastSibling();
        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        for (int i = 0; i < selectedList.Count; i++)
            selectedList[i].transform.SetSiblingIndex(selectedList[i].order);
        active = false;
        MoveComponents.Destroy(ref moveArea);
    }

    public override void On_Shape_Click()
    {

    }

    public override void On_Shape_Begin_Drag()
    {
        StartMoving();
    }

    public override void On_Free_Area_Click()
    {

    }

    public override void On_Free_Area_Begin_Drag(Transform board)
    {
        if (DefaultShortcuts.ctrlActive)
            Duplicate();
        moveArea.transform.Find("border").SetAsLastSibling();
        StartMoving();
    }

    public override void On_Free_Area_Drag(BoardPlan plan)
    {
        if (active)
        {
            Vector2 endPos = Input.mousePosition;
            Vector2 displace = endPos - startPos;
            MoveSelectedObject(new Vector3(displace.x,displace.y));

        }
    }

    public override void On_Drag_Exit()
    {

    }

    static void Duplicate()
    {
        string parentName = "";
        foreach (var item in SelectTools.lastShapes)
        {
            if (parentName == "")
                parentName = item.gameObject.transform.parent.name;
            if (parentName != "Grid" && parentName != "OuterParts")
            {
                item.gameObject.transform.SetParent(item.gameObject.transform.parent.parent);
                item.transform.SetAsLastSibling();
            }
        }
        moveArea.transform.SetAsLastSibling();

        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        for (int i = 0; i < selectedList.Count; i++)
            selectedList[i].transform.SetSiblingIndex(selectedList[i].order);
        DuplicateShapes.TransformDuplicate();
        foreach (var item in SelectTools.lastShapes)
            item.order = item.gameObject.transform.GetSiblingIndex();
        selectedList = new List<Shape>();
        selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        moveArea.transform.SetSiblingIndex(selectedList[selectedList.Count - 1].transform.GetSiblingIndex() + 1);
        for (int i = 0; i < selectedList.Count; i++)
        {
            selectedList[i].transform.SetParent(moveArea.transform);
            selectedList[i].transform.Find("SelectObject").gameObject.SetActive(false);
        }
    }
    static void StartMoving()
    {
        if (!moveArea)
            return;
        startPos = Input.mousePosition;
        if (RectTransformUtility.RectangleContainsScreenPoint(moveArea.GetComponent<RectTransform>(),
            startPos, moveArea.GetComponentInParent<Canvas>().GetComponent<Camera>()))
         {
            active = true;
            startArea = moveArea.transform.position;
            workingRect = RectTools.ToScreen(moveArea.transform.parent.Find("DrawCanvas").GetComponent<RectTransform>());
            Rect moveRect = RectTools.ToScreen(moveArea.GetComponent<RectTransform>());
            moveBorder = new Vector2(moveRect.width / 2, moveRect.height / 2);
         }
        else
        {
            active = false;
        }
    }

    static void MoveSelectedObject(Vector3 displace)
    {
        Vector3 newPos = moveArea.transform.position;

        newPos.x = (workingRect.Contains(new Vector3(startArea.x + displace.x + Mathf.Sign(displace.x) * moveBorder.x, startArea.y, startArea.z))) ?
            startArea.x + displace.x : workingRect.x + workingRect.width / 2.0f + Mathf.Sign(displace.x) * (workingRect.width / 2.0f - moveBorder.x);
        newPos.y = (workingRect.Contains(new Vector3(startArea.x, startArea.y + displace.y + Mathf.Sign(displace.y) * moveBorder.y, startArea.z))) ?
            startArea.y + displace.y : workingRect.y + workingRect.height / 2.0f + Mathf.Sign(displace.y) * (workingRect.height / 2.0f - moveBorder.y);

        moveArea.transform.position = newPos;

    }
    static GameObject moveArea;
    static Vector3 startArea;
    static Vector4 extermums;
    public static void GetExtermums()
    {
        int size = SelectTools.lastShapes.Count;
        if (size == 0)
            return;
        float[] minXValues = new float[size];
        float[] minYValues = new float[size];
        float[] maxXValues = new float[size];
        float[] maxYValues = new float[size];
        int i = 0;
        float X_Added = 0;
        float Y_Added = 0;
        foreach (var item in SelectTools.lastShapes)
        {
            RectTransform rectTra = item.gameObject.GetComponent<RectTransform>();
            X_Added = rectTra.rect.width * Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * rectTra.localEulerAngles.z)) * rectTra.localScale.y +
                rectTra.rect.height * Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * rectTra.localEulerAngles.z)) * rectTra.localScale.y;
            Y_Added = rectTra.rect.width * Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * rectTra.localEulerAngles.z)) * rectTra.localScale.y +
                rectTra.rect.height * Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * rectTra.localEulerAngles.z)) * rectTra.localScale.y;
            minXValues[i] = rectTra.localPosition.x - X_Added / 2;
            minYValues[i] = rectTra.localPosition.y - Y_Added / 2;
            maxXValues[i] = rectTra.localPosition.x + X_Added / 2;
            maxYValues[i] = rectTra.localPosition.y + Y_Added / 2;
            i++;
        }
        extermums = new Vector4();
        extermums.x = Mathf.Min(minXValues);
        extermums.y = Mathf.Min(minYValues);
        extermums.z = Mathf.Max(maxXValues);
        extermums.w = Mathf.Max(maxYValues);
    }
    public static void DrawBorder()
    {
        if (moveArea == null)
            moveArea = MoveComponents.createObject();
        RectTransform moveRect = moveArea.GetComponent<RectTransform>();
        Transform parent = null;
        foreach (var item in SelectTools.lastShapes)
        {
            parent = item.transform.parent;
            break;
        }
        moveRect.SetParent(parent);
        moveRect.localScale = new Vector3(1, 1, 1);
        Vector3 position = new Vector3((extermums.z+extermums.x)/2,(extermums.y+extermums.w)/2,0);
        moveRect.localPosition = position;
        moveRect.sizeDelta = new Vector2(Mathf.Abs(extermums.z - extermums.x),
            Mathf.Abs(extermums.w - extermums.y));
    }
}

public class MoveComponents: MonoBehaviour
{
    static GameObject moveArea;
    public static GameObject createObject()
    {
        GameObject prefab = ShapeCenter.moveArea;
        moveArea = Instantiate(prefab as Object) as GameObject;
        return moveArea;
    }
    public static void Destroy(ref GameObject obj)
    {
        Destroy(obj);
    }

}
