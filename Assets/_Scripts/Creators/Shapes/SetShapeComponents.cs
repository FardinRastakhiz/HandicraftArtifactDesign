using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetShapeComponents : MonoBehaviour {
    protected static void AddComponents(Shape shape)
    {
        shape.gameObject.AddComponent<RectTransform>();
        shape.gameObject.AddComponent<UnityEngine.UI.Image>();
        shape.gameObject.AddComponent<UnityEngine.UI.Button>();
        shape.gameObject.AddComponent<UnityEngine.UI.LayoutElement>();
        shape.gameObject.AddComponent<EventTrigger>();
        shape.gameObject.AddComponent<AlphaCheck>();
    }
    protected static void SetComponents(Shape shape, Board board)
    {
        SetRectTransform(shape);
        SetImage(shape);
        SetButton(shape.gameObject.GetComponent<UnityEngine.UI.Button>());
        SetEventTriggers(shape.gameObject.GetComponent<EventTrigger>(), board);
        SetLayoutElement(shape.gameObject.GetComponent<UnityEngine.UI.LayoutElement>());
        SetAlphaCheck(shape.gameObject.GetComponent<AlphaCheck>());
    }
    protected static void SetRectTransform(Shape shape)
    {
        RectTransform rectTra = shape.gameObject.GetComponent<RectTransform>();
        rectTra.anchoredPosition = new Vector3(shape.transform2D.position.x, shape.transform2D.position.y);
        rectTra.localScale = new Vector3(shape.transform2D.size, Mathf.Abs(shape.transform2D.size), Mathf.Abs(shape.transform2D.size));
        rectTra.sizeDelta = shape.transform2D.scale;
        Vector3 angles = rectTra.localEulerAngles;
        angles.z = shape.transform2D.rotation;
        rectTra.localEulerAngles = angles;
    }
    protected static void SetImage(Shape shape)
    {
        UnityEngine.UI.Image image = shape.gameObject.GetComponent<UnityEngine.UI.Image>();
        if (shape.GetType()==typeof(Primitive))
            image.sprite = ShapeCenter.sourceShapes[((Primitive)shape).sourceShape];
        else if (shape.GetType() == typeof(Part))
        {
            image.sprite = Resources.LoadAll<Sprite>(((Part)shape).sourceImage)[((Part)shape).index];
        }
        else if (shape.GetType() == typeof(Background))
        {
            image.sprite = ShapeCenter.backgrounds[((Background)shape).sourceShape].image;
            image.type = ShapeCenter.backgrounds[((Background)shape).sourceShape].type;
        }
        //image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, shape.size.x);
        //image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, shape.size.y);
        image.color = shape.color;
    }
    protected static void SetButton(UnityEngine.UI.Button button)
    {

    }
    protected static void SetEventTriggers(EventTrigger trigger, Board board)
    {

        Rect paletteBoard = Fardin.UITools.RectTools.ToScreen(GameObject.FindGameObjectWithTag("PaletteBoard").GetComponent<RectTransform>());
        List<EventTrigger.Entry> entries = trigger.triggers;
        entries = new List<EventTrigger.Entry>();
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener((eventData) => { DragShape.Drag_Shape("Begin"); });
        entry.callback.AddListener((eventData) => { ToolsUtility.On_Shape_Begin_Drag(); });
        entry.callback.AddListener((eventData) => { GenBoardPlan.OnPaletteBoardClicked(paletteBoard); });
        
        entries.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((eventData) => { DragShape.Drag_Shape("dragging"); });
        entries.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((eventData) => { DragShape.Drag_Shape("end"); });
        entry.callback.AddListener((eventData) => { GenBoardPlan.OnPaletteBoardClicked(paletteBoard); });
        entries.Add(entry);


        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { ToolsUtility.On_Shape_Mouse_Over(trigger.transform); });
        entries.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { ToolsUtility.On_Shape_Mouse_Over_Exit(); });
        entries.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { ToolsUtility.On_Shape_Click(); });
        entry.callback.AddListener((eventData) => { GenBoardPlan.OnPaletteBoardClicked(paletteBoard); });
        entries.Add(entry);

        /*
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Scroll;
        entry.callback.AddListener((eventData) => { board.Zoom(); });
        entries.Add(entry);*/

        trigger.triggers = entries;
        SetBoardTriggers(trigger, board);
    }
    protected static void SetBoardTriggers(EventTrigger trigger, Board board)
    {
        if (board != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Scroll;
            entry.callback.AddListener((eventData) => { ZoomBoard.Zoom(trigger.transform); });
            trigger.triggers.Add(entry);

        }
    }
    public static void ResetTriggers(EventTrigger trigger, Board board)
    {
        GameObject tra = trigger.gameObject;
        tra.GetComponent<EventTrigger>().triggers = new List<EventTrigger.Entry>();
        SetEventTriggers(tra.GetComponent<EventTrigger>(), board);
    }
    protected static void SetLayoutElement(UnityEngine.UI.LayoutElement element)
    {
        //element.preferredHeight = 165;
        //element.preferredWidth = 165;
    }

    protected static void SetAlphaCheck(AlphaCheck alphaCheck)
    {
        alphaCheck.AlphaThreshold = 0.5f;
    }
}
