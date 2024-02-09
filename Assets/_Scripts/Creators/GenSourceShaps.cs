///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Clss Namespacea>
///   Class:          <GenSourceShaps> <SSComps> <SourceShapeSubInfo>
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
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenSourceShaps {
    static List<SourceShape> sourceShaps;
    public static void Generate()
    {
        sourceShaps = new List<SourceShape>();
        SourceShape sourceShape = new SourceShape();
        SSComps.GetParent(GameObject.Find("SourceShapes"));
        for (int i = 0; i < ShapeCenter.sourceShapes.Length; i++)
        {
            sourceShape.index = i;
            sourceShape.name = ShapeCenter.sourceShapes[i].name;
            sourceShape.image = ShapeCenter.sourceShapes[i];
            sourceShape.gameObject = SSComps.createShapeObject(sourceShape);
            sourceShape.gameObject.GetComponent<Button>().onClick.AddListener(CopyShape.On_Source_Shape_Click);
            SubInfo.AddSubInfo(sourceShape.gameObject.transform);
            sourceShaps.Add(sourceShape);
        }
    }
}

class SSComps
{
    static Transform parent;
    static RectTransform scrollContent;

    public static void GetParent(GameObject SourceParent)
    {
        scrollContent = SourceParent.transform.Find("ScrollContent").GetComponent<RectTransform>();
        parent = scrollContent.Find("Contents");
    }

    public static GameObject createShapeObject(SourceShape prim)
    {
        GameObject newShape = new GameObject(prim.name);
        newShape.transform.parent = parent;
        AddComponents(ref newShape);
        SetComponents(ref newShape, prim);
        return newShape;
    }

    static void AddComponents(ref GameObject shape)
    {
        shape.AddComponent<RectTransform>();
        shape.AddComponent<CanvasRenderer>();
        shape.AddComponent<Image>();
        shape.AddComponent<Button>();
        shape.AddComponent<LayoutElement>();
        shape.AddComponent<Outline>();
    }

    static void SetComponents(ref GameObject shape,SourceShape prim)
    {
        SetRectTransform(shape.GetComponent<RectTransform>());
        SetImage(shape.GetComponent<Image>(), prim.image);
        //SetButton(shape.GetComponent<Button>());
        SetLayoutElement(shape.GetComponent<LayoutElement>());
        SetOutline(shape.GetComponent<Outline>());
    }
    static void SetRectTransform(RectTransform rectTransform)
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
    }
    static void SetOutline(Outline outline)
    {
        Color col = outline.effectColor;
        col.a = 1.0f;
        outline.effectColor = col;
    }

    static void SetImage(Image image, Sprite spr)
    {
        image.sprite = spr;
    }

    /*static void SetButton(Button button)
    {
        button.onClick.AddListener(CopyShape.On_Source_Shape_Click);
    }*/

    static void SetLayoutElement(LayoutElement element)
    {
        element.preferredHeight = scrollContent.rect.height-2;
        element.preferredWidth = scrollContent.rect.height-2;
    }

    static void ondone()
    {

    }
}


class SubInfo : MonoBehaviour
{
    public static void AddSubInfo(Transform sourceShapeTra)
    {
        GameObject subInfo = Instantiate(ShapeCenter.planSub) as GameObject;
        subInfo.transform.SetParent(sourceShapeTra);
        Text infoText = subInfo.transform.Find("Text").GetComponent<Text>();
        infoText.text = sourceShapeTra.name;
        infoText.fontSize = 10;
        SetRectTransform(subInfo.GetComponent<RectTransform>());
        SetImage(subInfo.GetComponent<Image>());
    }

    static void SetRectTransform(RectTransform rectTra)
    {
        rectTra.localScale = Vector3.one;
        rectTra.offsetMin = new Vector2(0, 0);
        rectTra.offsetMax = new Vector2(0, 0);
        Vector3 pos = rectTra.anchoredPosition;
        pos.y = 7;
        rectTra.anchoredPosition = pos;
        rectTra.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 14);
    }

    static void SetImage(Image img)
    {
        Color col = img.color;
        col.a = 0.5f;
        img.color = col;
    }
}