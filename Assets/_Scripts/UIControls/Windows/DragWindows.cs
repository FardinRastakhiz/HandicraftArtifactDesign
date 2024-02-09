using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWindows : MonoBehaviour {

    [SerializeField]
    Window window;

    RectTransform rectTra;
    Vector3 mouseStart;
    Vector3 rectStart;

    [SerializeField]
    RectTransform frameWork;

    public void On_Begin_Drag()
    {
        if (window.IsFullScreen)
            return;
        rectTra = window.GetComponent<RectTransform>();
        mouseStart = Input.mousePosition;
        rectStart = rectTra.position;
        if (frameWork == null)
        {
            if (rectTra.parent.Find("WorkFrame") != null)
                frameWork = rectTra.parent.Find("WorkFrame").GetComponent<RectTransform>();
            else
                frameWork = rectTra.parent.GetComponent<RectTransform>();
        }
    }
    public void On_Drag()
    {
        if (window.IsFullScreen)
            return;
        var WorkFrame = RectTools.ToScreen(frameWork);
        rectTra.position = WorkFrame.Contains(Input.mousePosition) ? rectStart + Input.mousePosition - mouseStart : rectTra.position;
    }
}
