///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <RotateTools> <RotateComponents>
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

public class RotateTools : Transforms
{
    static bool isActive;
    static Vector2 centerPoint;
    static Vector2 startPos;
    static GameObject rotateArea;
    static Vector3 startArea;
    static RotateTools thisObject;
    public static void Start(ref Transforms transforms)
    {

        if (thisObject == null)
            thisObject = new RotateTools();
        if (transforms.GetType() == thisObject.GetType()||SelectTools.lastShapes.Count==0)
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
        rotateArea.transform.SetSiblingIndex(selectedList[selectedList.Count - 1].transform.GetSiblingIndex() + 1);
        for (int i = 0; i < selectedList.Count; i++)
            selectedList[i].gameObject.transform.SetParent(rotateArea.transform);

        rotateArea.transform.Find("border").SetAsLastSibling();
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
        rotateArea.transform.SetAsLastSibling();
        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        for (int i = 0; i < selectedList.Count; i++)
            selectedList[i].transform.SetSiblingIndex(selectedList[i].order);
        isActive = false;
        RotateComponents.Destroy(ref rotateArea);
    }

    public override void On_Shape_Click()
    {

    }

    public override void On_Shape_Begin_Drag()
    {
        StartRotating();
    }

    public override void On_Free_Area_Click()
    {

    }

    public override void On_Free_Area_Begin_Drag(Transform board)
    {
        StartRotating();
    }

    public override void On_Free_Area_Drag(BoardPlan plan)
    {
        if (isActive)
        {
            Vector2 endPos = Input.mousePosition;
            Vector2 displace = endPos - startPos;
            //float radius = (endPos - centerPoint).magnitude;
            displace.x = (endPos.y<centerPoint.y)?displace.x:-displace.x;
            displace.y = (endPos.x>=centerPoint.x)?displace.y:-displace.y;
            RotateSelectedObject(displace.x + displace.y);
        }
    }

    public override void On_Drag_Exit()
    {

    }
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
        if (rotateArea == null)
            rotateArea = RotateComponents.createObject();
        RectTransform moveRect = rotateArea.GetComponent<RectTransform>();
        Transform parent = null;
        foreach (var item in SelectTools.lastShapes)
        {
            parent = item.transform.parent;
            break;
        }
        moveRect.SetParent(parent);
        moveRect.localScale = new Vector3(1, 1, 1);
        Vector3 position = new Vector3((extermums.z + extermums.x) / 2, (extermums.y + extermums.w) / 2, 0);
        moveRect.localPosition = position;
        var worldCorners = new Vector3[4];
        moveRect.GetWorldCorners(worldCorners);
        centerPoint = new Vector2((worldCorners[2].x + worldCorners[0].x)/2.0f,(worldCorners[2].y + worldCorners[0].y)/2.0f);
        moveRect.sizeDelta = new Vector2(Mathf.Abs(extermums.z - extermums.x),
            Mathf.Abs(extermums.w - extermums.y));
    }

    static void StartRotating()
    {
        startPos = Input.mousePosition;
        if (RectTransformUtility.RectangleContainsScreenPoint(rotateArea.GetComponent<RectTransform>(),
            startPos, rotateArea.GetComponentInParent<Canvas>().GetComponent<Camera>()))
        {
            isActive = true;
            startArea = rotateArea.transform.localEulerAngles;
        }
        else
        {
            isActive = false;
        }
    }

    static void RotateSelectedObject(float rotation)
    {
        //foreach (var item in SelectTools.lastShapes)
        //{
        //    item.gameObject.GetComponent<RectTransform>().localEulerAngles = shapeStartAngles[item.id] + new Vector3(0,0,rotation);
        //}
        rotateArea.transform.localEulerAngles = startArea + new Vector3(0, 0, rotation);
        startArea = rotateArea.transform.localEulerAngles;
        startPos = Input.mousePosition;
    }
}

public class RotateComponents : MonoBehaviour
{
    static GameObject rotateArea;
    public static GameObject createObject()
    {
        GameObject prefab = ShapeCenter.rotateArea;
        rotateArea = Instantiate(prefab as Object) as GameObject;
        return rotateArea;
    }

    public static void Destroy(ref GameObject obj)
    {
        Destroy(obj);
    }
}
