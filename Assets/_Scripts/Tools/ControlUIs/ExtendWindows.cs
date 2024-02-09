///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ExtendWindows>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExtendWindows : MonoBehaviour {

    [SerializeField]
    Sprite horizontalSprite;
    [SerializeField]
    Sprite diagonalSprite;
    [SerializeField]
    RectTransform CursorObj;
    bool activeBorder;
    [SerializeField]
    float minThickNess = 100.0f;
    //RectTransform borderRect;
    string borderName;

    [SerializeField]
    public RectTransform window;

    //RectTransform workFrame;

    Vector2 offsetMin;
    Vector2 offsetMax;
    Vector2 startOffsetMin;
    Vector2 startOffsetMax;
    Vector2 mouseStart;
    Board targetBoard;
    Transform paletteWindow;
    Rect paletteRect;
    void Update()
    {
        if (activeBorder)
        {
            CursorObj.position = Input.mousePosition;
            /*if (!RectTools.ToScreen(borderRect).Contains(Input.mousePosition))
            {
                CursorObj.gameObject.SetActive(false);
                Cursor.visible = true;
                activeBorder = false;
            }*/
	    }
    }

    public void On_Border_Over(GameObject borderObj)
    {
        if (!Input.GetMouseButton(0))
        {
            borderName = borderObj.name;
            //borderRect = borderObj.GetComponent<RectTransform>();
            //workFrame = window.parent.GetComponent<RectTransform>();
            activeBorder = true;
            Cursor.visible = false;
            CursorObj.gameObject.SetActive(true);
            SetCursor();
        }
    }

    public void On_Border_Exit()
    {
        if (!Input.GetMouseButton(0))
        {
            activeBorder = false;
            Cursor.visible = true;
            CursorObj.gameObject.SetActive(false);
        }
    }


    public void On_Border_Begin_Drag()
    {
        if (Input.GetMouseButton(0))
        {
            startOffsetMin = window.offsetMin;
            startOffsetMax = window.offsetMax;
            offsetMin = startOffsetMin;
            offsetMax = startOffsetMax;
            mouseStart = Input.mousePosition;
        }
    }

    public void On_Border_Drag()
    {
        if (Input.GetMouseButton(0))
        {
            //if (RectTools.ToScreen(workFrame).Contains(Input.mousePosition))
            switch (borderName)
            {
                case "Left":
                    offsetMin.x = startOffsetMin.x + (Input.mousePosition.x - mouseStart.x) / window.lossyScale.x;
                    offsetMin.x = offsetMin.x > offsetMax.x - minThickNess ? offsetMax.x - minThickNess : offsetMin.x;
                    window.offsetMin = offsetMin;

                    break;
                case "Right":
                    offsetMax.x = startOffsetMax.x + (Input.mousePosition.x - mouseStart.x) / window.lossyScale.x;
                    offsetMax.x = offsetMax.x < offsetMin.x + minThickNess ? offsetMin.x + minThickNess : offsetMax.x;
                    window.offsetMax = offsetMax;
                    break;
                case "Bottom":
                    offsetMin.y = startOffsetMin.y + (Input.mousePosition.y - mouseStart.y) / window.lossyScale.y;
                    offsetMin.y = offsetMin.y > offsetMax.y - minThickNess ? offsetMax.y - minThickNess : offsetMin.y;
                    window.offsetMin = offsetMin;
                    break;
                case "Top":
                    offsetMax.y = startOffsetMax.y + (Input.mousePosition.y - mouseStart.y) / window.lossyScale.y;
                    offsetMax.y = offsetMax.y < offsetMin.y + minThickNess ? offsetMin.y + minThickNess : offsetMax.y;
                    window.offsetMax = offsetMax;
                    break;
                case "BottomRight":
                    offsetMax.x = startOffsetMax.x + (Input.mousePosition.x - mouseStart.x) / window.lossyScale.x;
                    offsetMin.y = startOffsetMin.y + (Input.mousePosition.y - mouseStart.y) / window.lossyScale.y;
                    offsetMax.x = offsetMax.x < offsetMin.x + minThickNess ? offsetMin.x + minThickNess : offsetMax.x;
                    offsetMin.y = offsetMin.y > offsetMax.y - minThickNess ? offsetMax.y - minThickNess : offsetMin.y;
                    window.offsetMin = offsetMin;
                    window.offsetMax = offsetMax;
                    break;
                case "BottomLeft":
                    offsetMin.x = startOffsetMin.x + (Input.mousePosition.x - mouseStart.x) / window.lossyScale.x;
                    offsetMin.y = startOffsetMin.y + (Input.mousePosition.y - mouseStart.y) / window.lossyScale.y;
                    offsetMin.x = offsetMin.x > offsetMax.x - minThickNess ? offsetMax.x - minThickNess : offsetMin.x;
                    offsetMin.y = offsetMin.y > offsetMax.y - minThickNess ? offsetMax.y - minThickNess : offsetMin.y;
                    window.offsetMin = offsetMin;
                    break;
                case "TopRight":
                    offsetMax.x = startOffsetMax.x + (Input.mousePosition.x - mouseStart.x) / window.lossyScale.x;
                    offsetMax.y = startOffsetMax.y + (Input.mousePosition.y - mouseStart.y) / window.lossyScale.y;
                    offsetMax.x = offsetMax.x < offsetMin.x + minThickNess ? offsetMin.x + minThickNess : offsetMax.x;
                    offsetMax.y = offsetMax.y < offsetMin.y + minThickNess ? offsetMin.y + minThickNess : offsetMax.y;
                    window.offsetMax = offsetMax;
                    break;
                case "TopLeft":
                    offsetMin.x = startOffsetMin.x + (Input.mousePosition.x - mouseStart.x) / window.lossyScale.x;
                    offsetMax.y = startOffsetMax.y + (Input.mousePosition.y - mouseStart.y) / window.lossyScale.y;
                    offsetMin.x = offsetMin.x > offsetMax.x - minThickNess ? offsetMax.x - minThickNess : offsetMin.x;
                    offsetMax.y = offsetMax.y < offsetMin.y + minThickNess ? offsetMin.y + minThickNess : offsetMax.y;
                    window.offsetMin = offsetMin;
                    window.offsetMax = offsetMax;
                    break;
                default:
                    break;
            }
        }
    }


    public void On_Border_Drag_End()
    {
        activeBorder = false;
        Cursor.visible = true;
        CursorObj.gameObject.SetActive(false);
    }

    public void On_Scroll_Mouse()
    {
        Transform dynamicShape = transform.parent;
        if (dynamicShape.parent.name=="Grid")
            ZoomBoard.Zoom(dynamicShape);
    }

    void SetCursor()
    {
        //Cursor.visible = false;
        switch (borderName)
        {
            case "Left":
            case "Right":
                CursorObj.gameObject.GetComponent<Image>().sprite = horizontalSprite;
                Vector3 angle = CursorObj.localEulerAngles;
                angle.z = 0;
                CursorObj.localEulerAngles = angle;
                break;
            case "Bottom":
            case "Top":
                CursorObj.gameObject.GetComponent<Image>().sprite = horizontalSprite;
                angle = CursorObj.localEulerAngles;
                angle.z = 90;
                CursorObj.localEulerAngles = angle;
                break;
            case "BottomRight":
            case "TopLeft":
                CursorObj.gameObject.GetComponent<Image>().sprite = diagonalSprite;
                angle = CursorObj.localEulerAngles;
                angle.z = 90;
                CursorObj.localEulerAngles = angle;
                break;
            case "TopRight":
            case "BottomLeft":
                CursorObj.gameObject.GetComponent<Image>().sprite = diagonalSprite;
                angle = CursorObj.localEulerAngles;
                angle.z = 0;
                CursorObj.localEulerAngles = angle;
                break;
        }
        float sc = CursorObj.parent.lossyScale.x;
        CursorObj.localScale = new Vector3(1.0f / sc, 1.0f / sc, 1.0f / sc);
    }



    void SetBottom()
    {

    }

    void SetLeft()
    {

    }

    void SetRight()
    {

    }
}
