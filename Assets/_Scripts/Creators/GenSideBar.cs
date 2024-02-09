///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Clss Namespacea>
///   Class:          <GenSidebar> <DrawSideBar>
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
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenSidebar : MonoBehaviour
{
    static List<Sidebar> sidebars;

    public static void Generate(BoardPlan plan)
    {
        if (sidebars == null)
            sidebars = new List<Sidebar>();
        int startindex = sidebars.Count;
        TakeParts(plan);
        TakePrimitives(plan);
        DrawSideBar.Draw(sidebars.GetRange(startindex, sidebars.Count - startindex));
    }

    public static bool removeExcess(string closedPlan)
    {
        for (int i = sidebars.Count-1; i >=0; i--)
        {
            if (sidebars[i].planName==closedPlan)
            {
                Destroy(sidebars[i].gameObject);
                sidebars.RemoveAt(i);
            }
        }
        return true;
    }
    
    static void TakeParts(BoardPlan plan)
    {
        HashSet<KeyValuePair<int, string>> sideList = new HashSet<KeyValuePair<int, string>>();
        KeyValuePair<int, string> pair;
        for (int i = 0; i < plan.parts.Count; i++)
        {
            pair = new KeyValuePair<int, string>(plan.parts[i].index, plan.parts[i].sourceImage);
            if (!sideList.Contains(pair))
            {
                sideList.Add(pair);
                AddShapeToSidebar(plan.parts[i], plan.name);
            }
            /*
            GameObject newObj = new GameObject("Sidebar");
            newObj.AddComponent<Sidebar>();
            Sidebar sidebar = newObj.GetComponent<Sidebar>();
            sidebar.TakeParameters(plan.parts[i]);
            sidebars.Add(sidebar);
             */
        }
        sideList.Clear();
    }

    static void TakePrimitives(BoardPlan plan)
    {
        HashSet<int> sideList = new HashSet<int>();
        for (int i = 0; i < plan.primitives.Count; i++)
        {
            if (!sideList.Contains(plan.primitives[i].sourceShape))
            {
                sideList.Add(plan.primitives[i].sourceShape);
                AddShapeToSidebar(plan.primitives[i], plan.name);
            }
            /*
            GameObject newObj = new GameObject("Sidebar");
            newObj.AddComponent<Sidebar>();
            Sidebar sidebar = newObj.GetComponent<Sidebar>();
            sidebar.TakeParameters(plan.primitives[i]);
            sidebars.Add(sidebar);
             */
        }
    }

    static void AddShapeToSidebar(Shape shape, string planNme)
    {
        GameObject newObj = new GameObject("Sidebar");
        newObj.AddComponent<Sidebar>();
        Sidebar sidebar = newObj.GetComponent<Sidebar>();
        sidebar.TakeParameters(shape);
        sidebar.planName = planNme;
        sidebars.Add(sidebar);
    }

    public static void RemoveElement()
    {

    }

    public static void AddElement()
    {

    }
}

public class DrawSideBar
{
    static int size;
    static Transform parent;
    static RectTransform scrollContent;
    public static void Draw(List<Sidebar> sideBars)
    {
        size = sideBars.Count;
        scrollContent = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().
            planParts.transform.Find("ScrollContent").GetComponent<RectTransform>();
        parent = scrollContent.Find("Contents");
        for (int i = 0; i < size; i++)
        {
            sideBars[i].transform.SetParent(parent);
            AddComponents(sideBars[i].gameObject);
            SetComponents(sideBars[i]);
        }
    }

    static void AddComponents(GameObject sideObj)
    {
        sideObj.AddComponent<RectTransform>();
        sideObj.AddComponent<Button>();
        sideObj.AddComponent<Image>();
        sideObj.AddComponent<LayoutElement>();
        sideObj.AddComponent<UnityEngine.UI.Outline>();
        //sideObj.AddComponent<EventTrigger>();
    }

    static void SetComponents(Sidebar sidebar)
    {
        SetRectTransform(sidebar.gameObject.GetComponent<RectTransform>());
        SetButton(sidebar.gameObject.GetComponent<Button>());
        SetImage(sidebar.gameObject.GetComponent<Image>(), sidebar);
        SetLayoutElement(sidebar.gameObject.GetComponent<LayoutElement>());
        SetOutline(sidebar.GetComponent<UnityEngine.UI.Outline>());
    }

    static void SetRectTransform(RectTransform tra)
    {
        tra.localScale = new Vector3(1, 1, 1);
    }

    static void SetButton(Button button)
    {
        button.onClick.AddListener(CopyShape.On_Sidebar_Click);
    }

    static void SetOutline(UnityEngine.UI.Outline outline)
    {
        Color col = outline.effectColor;
        col.a = 1.0f;
        outline.effectColor = col;
    }

    static void SetImage(Image img, Sidebar sidebar)
    {
        switch (sidebar.shapeType){
            case ShapeType.PART:
                img.sprite = Resources.LoadAll<Sprite>(sidebar.sourceImage)[sidebar.indx];
                break;
            case ShapeType.PRIMITIVE:
                img.sprite = ShapeCenter.sourceShapes[sidebar.indx];
                break;
            case ShapeType.BACKGROUND:
                img.sprite = ShapeCenter.backgrounds[sidebar.indx].image;
                img.type = ShapeCenter.backgrounds[sidebar.indx].type;
                break;
        }
        //img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sidebar.size.x);
        //img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sidebar.size.y);
    }
    static void SetLayoutElement(LayoutElement element)
    {
        element.preferredHeight = scrollContent.rect.height - 2;
        element.preferredWidth = scrollContent.rect.height - 2;
    }
    static void SetEventTrigger(EventTrigger trig)
    {
        /*List<EventTrigger.Entry> entris = trig.delegates;
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener(creat like click then Drag);
        entris.Add(entry);
        trig.delegates = entris;*/
    }
}