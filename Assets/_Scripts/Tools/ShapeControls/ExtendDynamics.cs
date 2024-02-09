///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ExtendDynamics> 
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExtendDynamics : MonoBehaviour
{

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

    Background ownerShape;

    [SerializeField]
    public RectTransform window;
    [SerializeField]
    public RectTransform windowParent;

    //RectTransform workFrame;

    Vector2 offsetMin;
    Vector2 offsetMax;
    Vector2 startOffsetMin;
    Vector2 startOffsetMax;
    Vector2 mouseStart;
    OnGrid targetGrid;
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

    bool IsActive()
    {
        if (ToolsUtility.toolState == ToolsState.SELECT)
        {
            if (SelectTools.lastShapes.Count==0)
                return true;
            if (ownerShape==null)
                ownerShape = transform.parent.GetComponent<Background>();
            else if (SelectTools.lastShapes.Count == 1 && SelectTools.lastShapes.Contains(ownerShape))
                return true;
        }
        return false;
    }

    public void On_Border_Over(GameObject borderObj)
    {
        if (!Input.GetMouseButton(0) && IsActive())
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
        if (Input.GetMouseButton(0) && IsActive())
        {
            windowParent = window.parent.GetComponent<RectTransform>();
            startOffsetMin = window.offsetMin;
            startOffsetMax = window.offsetMax;
            offsetMin = startOffsetMin;
            offsetMax = startOffsetMax;
            mouseStart = Input.mousePosition;
            rotation = Mathf.Deg2Rad*transform.parent.GetComponent<RectTransform>().localEulerAngles.z;
        }
        else if (Input.GetMouseButton(1))
        {
            if (transform.parent.parent.name != "Grid")
                return;

            if (paletteWindow == null)
                paletteWindow = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().paletteWindow;
            paletteRect = RectTools.ToScreen(paletteWindow.GetComponent<RectTransform>());
            targetGrid = transform.GetComponentInParent<OnGrid>();
            targetGrid.Begin_Drag();
        }
    }
    //Vector2 mouseTransfer;
    float rotation;
    public void On_Border_Drag()
    {
        if (Input.GetMouseButton(0) && IsActive())
        {
            float cosine = Mathf.Cos(rotation);
            float sine = Mathf.Sin(rotation);
            xSign = Mathf.Sign(window.localScale.x);
            Vector2 mouseTransfer1 = new Vector2(Input.mousePosition.x - mouseStart.x, Input.mousePosition.y - mouseStart.y);
            Vector2 mouseTransfer2 = new Vector2(mouseTransfer1.x * cosine + mouseTransfer1.y * sine,
                mouseTransfer1.y * cosine - mouseTransfer1.x * sine);
            Vector2 posDeviation = Vector2.zero;

            switch (borderName)
            {
                case "Left":
                    posDeviation = new Vector2(mouseTransfer2.x * cosine, mouseTransfer2.x * sine)  * 0.5f;
                    MoveBorder(mouseTransfer2.x, 0, 0, 0, posDeviation);
                    break;
                case "Right":
                    posDeviation = new Vector2(mouseTransfer2.x * cosine, mouseTransfer2.x * sine)  * 0.5f;
                    MoveBorder(0, mouseTransfer2.x, 0, 0, posDeviation);
                    break;
                case "Bottom":
                    posDeviation = new Vector2(-mouseTransfer2.y * sine , mouseTransfer2.y * cosine) * 0.5f;
                    MoveBorder(0, 0, mouseTransfer2.y, 0, posDeviation);
                    break;
                case "Top":
                    posDeviation = new Vector2(-mouseTransfer2.y * sine, mouseTransfer2.y * cosine) * 0.5f;
                    MoveBorder(0, 0, 0, mouseTransfer2.y, posDeviation);
                    break;
                case "BottomRight":
                    posDeviation = new Vector2(mouseTransfer2.x * cosine - mouseTransfer2.y * sine,
                        mouseTransfer2.x * sine + mouseTransfer2.y * cosine) * 0.5f;
                    MoveBorder(0, mouseTransfer2.x, mouseTransfer2.y, 0, posDeviation);
                    break;
                case "BottomLeft":
                    posDeviation = new Vector2(mouseTransfer2.x * cosine - mouseTransfer2.y * sine,
                        mouseTransfer2.x * sine + mouseTransfer2.y * cosine) * 0.5f;
                    MoveBorder(mouseTransfer2.x, 0, mouseTransfer2.y, 0, posDeviation);
                    break;
                case "TopRight":
                    posDeviation = new Vector2(mouseTransfer2.x * cosine - mouseTransfer2.y * sine,
                        mouseTransfer2.x * sine + mouseTransfer2.y * cosine) * 0.5f;
                    MoveBorder(0, mouseTransfer2.x, 0, mouseTransfer2.y, posDeviation);
                    break;
                case "TopLeft":
                    posDeviation = new Vector2(mouseTransfer2.x * cosine - mouseTransfer2.y * sine,
                        mouseTransfer2.x * sine + mouseTransfer2.y * cosine) * 0.5f;
                    MoveBorder(mouseTransfer2.x, 0, 0, mouseTransfer2.y, posDeviation);
                    break;
                default:
                    break;
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if (paletteRect.Contains(Input.mousePosition))
            {
                targetGrid.Drag();
            }
        }
    }

    float xSign = 0.0f;
    void MoveBorder(float xMin, float xMax, float yMin, float yMax, Vector2 posDeviation)
    {
        offsetMin.x = startOffsetMin.x + xMin / window.lossyScale.x;
        offsetMax.x = startOffsetMax.x + xMax / window.lossyScale.x;
        offsetMin.y = startOffsetMin.y + yMin / window.lossyScale.y;
        offsetMax.y = startOffsetMax.y + yMax / window.lossyScale.y;

        Vector2 deviationn = (new Vector2(xMin + xMax, (yMin + yMax) * xSign) * -0.5f + new Vector2(posDeviation.x, posDeviation.y) * xSign);

        bool resetCheck = (offsetMax.y < offsetMin.y + minThickNess)||(offsetMax.x < offsetMin.x + minThickNess)||
            offsetMin.x > offsetMax.x - minThickNess||offsetMin.y > offsetMax.y - minThickNess;
        if (resetCheck)
        {
            offsetMin = window.offsetMin;
            offsetMax = window.offsetMax;
            deviationn = Vector2.zero;
        }
        window.offsetMin = offsetMin;
        window.offsetMax = offsetMax;
        window.anchoredPosition += deviationn / window.lossyScale.x;
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
        if (dynamicShape.parent.name == "Grid")
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
