///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ScaleTools> <ScaleComponents>
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

public class ScaleTools : Transforms
{
    static GameObject scaleArea;
    static bool isActive;
    static Vector4 extermums;
    static Vector2 startPos;
    static Vector2 startSize;
    static Vector2 startMin;
    static ScaleTools thisObject;
    static RectTransform scaleRect;
    static Vector2 areaPoint;
    static Dictionary<string, Vector2> itemsStartSize;
    static Dictionary<string, Vector2> itemsStartPos;
    public static void Start(ref Transforms transforms)
    {
        if (thisObject == null)
            thisObject = new ScaleTools();
        if (transforms.GetType() == thisObject.GetType() || SelectTools.lastShapes.Count == 0)
            return;
        if (transforms != null)
            transforms.ResetTools();
        transforms = thisObject;
        GetExtermums();
        DrawBorder();
        //Vector2 halfParentSize = new Vector2(scaleArea.GetComponent<RectTransform>().rect.width,
        //    scaleArea.GetComponent<RectTransform>().rect.height) * 0.5f;
        //Vector2 itemPos = Vector2.zero;
        //Vector2 halfItemSize = Vector2.zero;
        itemsStartSize = new Dictionary<string, Vector2>();
        itemsStartPos = new Dictionary<string, Vector2>();


        foreach (var item in SelectTools.lastShapes)
            item.order = item.gameObject.transform.GetSiblingIndex();
        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        scaleArea.transform.SetSiblingIndex(selectedList[selectedList.Count - 1].transform.GetSiblingIndex() + 1);
        for (int i = 0; i < selectedList.Count; i++)
            selectedList[i].gameObject.transform.SetParent(scaleArea.transform);

        scaleArea.transform.Find("border").SetAsLastSibling();

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
        scaleArea.transform.SetAsLastSibling();
        List<Shape> selectedList = (from item in SelectTools.lastShapes
                                    orderby item.order
                                    select item).ToList();
        for (int i = 0; i < selectedList.Count; i++)
            selectedList[i].transform.SetSiblingIndex(selectedList[i].order);
        isActive = false;
        ScaleComponents.Destroy(ref scaleArea);
    }
    public override void On_Shape_Click()
    {

    }

    public override void On_Shape_Begin_Drag()
    {
        StartScaling();
    }

    public override void On_Free_Area_Click()
    {

    }

    public override void On_Free_Area_Begin_Drag(Transform board)
    {
        if (Input.GetMouseButton(0))
        {
            StartScaling();
        }
    }

    public override void On_Free_Area_Drag(BoardPlan plan)
    {
        if (isActive && Input.GetMouseButton(0))
        {
            Vector2 endPos = Input.mousePosition;
            Vector2 displace = endPos - startPos;
            //Vector2 projected = (displace.x * areaPoint.x + displace.y * areaPoint.y) * areaPoint;
            ScaleSelectedObject((displace.x * areaPoint.x + displace.y * areaPoint.y));
        }
    }

    public override void On_Drag_Exit()
    {
        Shape myShape = null;
        foreach (var item in SelectTools.lastShapes)
	    {
            myShape = item;
            break;
	    }

        if (isActive&&Input.GetMouseButtonUp(0))
        {

            Vector2 endPos = Input.mousePosition;
            Vector2 displace = endPos - startPos;
            //float dotProduct = (displace.x * areaPoint.x + displace.y * areaPoint.y);
            //Vector2 projected = dotProduct * areaPoint;
            if (!myShape)
                return;
            if (myShape.GetComponentInParent<Board>().plan==BoardPlans.boardPlans[BoardPlans.ActiveIndex])
                ScaleSelectedObject((displace.x * areaPoint.x + displace.y * areaPoint.y));
        }
    }
    void StartScaling()
    {
        startPos = Input.mousePosition;
        scaleRect = scaleArea.GetComponent<RectTransform>();
        if (RectTransformUtility.RectangleContainsScreenPoint(scaleRect,
            startPos, scaleArea.GetComponentInParent<Canvas>().GetComponent<Camera>()))
        {
            isActive = true;
            startSize = new Vector2(scaleRect.rect.width, scaleRect.rect.height);
            areaPoint = new Vector2(startPos.x - scaleRect.position.x, startPos.y - scaleRect.position.y).normalized;
            itemsStartSize = new Dictionary<string, Vector2>();
            itemsStartPos = new Dictionary<string, Vector2>();
            foreach (var item in SelectTools.lastShapes)
            {

                itemsStartSize.Add(item.GetType() + "_" + item.id, item.GetComponent<RectTransform>().sizeDelta);
                itemsStartPos.Add(item.GetType() + "_" + item.id, item.GetComponent<RectTransform>().anchoredPosition);
            }
        }
        else
        {
            isActive = false;
        }
    }

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
        if (scaleArea == null)
            scaleArea = ScaleComponents.createObject();
        RectTransform scaleRect = scaleArea.GetComponent<RectTransform>();
        Transform parent = null;
        foreach (var item in SelectTools.lastShapes)
        {
            parent = item.transform.parent;
            break;
        }
        scaleRect.SetParent(parent);
        scaleRect.localScale = new Vector3(1, 1, 1);
        Vector3 position = new Vector3((extermums.z + extermums.x) / 2, (extermums.y + extermums.w) / 2, 0);
        scaleRect.localPosition = position;
        scaleRect.sizeDelta = new Vector2(Mathf.Abs(extermums.z - extermums.x),
            Mathf.Abs(extermums.w - extermums.y));
    }

    static void ScaleSelectedObject(float size)
    {
        Vector2 newSize = startSize + startSize.normalized* size;
        if (newSize.x <= 20 || newSize.y <= 20)
            return;
        scaleRect.sizeDelta = newSize;
        float r = scaleRect.sizeDelta.x / startSize.x;
        foreach (var item in SelectTools.lastShapes)
        {
            RectTransform itemRect = item.GetComponent<RectTransform>();
            itemRect.anchoredPosition = itemsStartPos[item.GetType() + "_" + item.id] * r;
            itemRect.sizeDelta = itemsStartSize[item.GetType() + "_" + item.id] * r;
        }
    }
}

public class ScaleComponents : MonoBehaviour
{
    static GameObject scaleArea;
    public static GameObject createObject()
    {
        GameObject prefab = ShapeCenter.scaleArea;
        scaleArea = Instantiate(prefab as Object) as GameObject;
        return scaleArea;
    }

    public static void Destroy(ref GameObject obj)
    {
        Destroy(obj);
    }
}