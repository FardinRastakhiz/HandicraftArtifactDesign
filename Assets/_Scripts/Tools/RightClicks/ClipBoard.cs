///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ClipBoard>
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

public class ClipBoard {
    public static List<ShapeInstance> instances;
    public static int numberOfCopies;
    public static Vector3 mousePosition;
    public static bool isCut;
    protected static string sourcePlan = "";

    protected static void ResetInstances()
    {
        mousePosition = RightClick.mousePosition;
        if (instances==null)
        {
            instances = new List<ShapeInstance>();
            return;
        }
        for (int i = instances.Count-1; i >= 0; i--)
        {
            UIController.DestoryObject(instances[i].rectTransform.gameObject);
            instances.RemoveAt(i);
        }
        numberOfCopies = 0;
    }

    protected static void TakePrimitiveParameters(Primitive prim, ShapeInstance instance)
    {
        instance.id = prim.id;
        instance.size = prim.size;
        instance.transform2D = new Transform2D();
        instance.transform2D.position = prim.transform2D.position;
        instance.transform2D.rotation = prim.transform2D.rotation;
        instance.transform2D.size = prim.transform2D.size;
        instance.color = prim.color;
        instance.sourcePlan = sourcePlan;
        instance.sourceImage = "";
        instance.indx = prim.sourceShape;
        instance.order = prim.order;
        instance.shapeType = ShapeType.PRIMITIVE;
        GameObject newObj = new GameObject();
        instance.rectTransform = newObj.AddComponent<RectTransform>();
        RectTools.Copy(prim.GetComponent<RectTransform>(), instance.rectTransform);
    }

    protected static void TakePartParameters(Part part, ShapeInstance instance)
    {
        instance.id = part.id;
        instance.size = part.size;
        instance.transform2D = new Transform2D();
        instance.transform2D.position = part.transform2D.position;
        instance.transform2D.rotation = part.transform2D.rotation;
        instance.transform2D.size = part.transform2D.size;
        instance.color = part.color;
        instance.sourcePlan = sourcePlan;
        instance.sourceImage = part.sourceImage;
        instance.indx = part.index;
        instance.order = part.order;
        instance.shapeType = ShapeType.PART;
        GameObject newObj = new GameObject();
        instance.rectTransform = newObj.AddComponent<RectTransform>();
        RectTools.Copy(part.GetComponent<RectTransform>(), instance.rectTransform);
    }

    protected static void TakeBackgroundParameters(Background bg, ShapeInstance instance)
    {
        instance.id = bg.id;
        instance.size = bg.size;
        instance.transform2D = new Transform2D();
        instance.transform2D.position = bg.transform2D.position;
        instance.transform2D.rotation = bg.transform2D.rotation;
        instance.transform2D.size = bg.transform2D.size;
        instance.color = bg.color;
        instance.sourcePlan = sourcePlan;
        instance.sourceImage = "";
        instance.indx = bg.sourceShape;
        instance.imageType = bg.imageType;
        instance.order = bg.order;
        instance.shapeType = ShapeType.BACKGROUND;
        GameObject newObj = new GameObject();
        instance.rectTransform = newObj.AddComponent<RectTransform>();
        RectTools.Copy(bg.GetComponent<RectTransform>(), instance.rectTransform);
    }
}
