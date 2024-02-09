///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Toolbar>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public class Toolbar : MonoBehaviour {


    static RectTransform toolbar;
    Vector3 toolbarPos;
    float toolbarTarget;

    private bool isHiding;
    float timer;


    //static RectTransform sourceShapes;
    static RectTransform PaletteBoard;
    static RectTransform sideShapes;

    Vector2 paletteBoardOffsetMin;
    Vector2 sideShapesOffsetMin;
    //Vector2 sourceShapesOffsetMax;

    float paletteBoardTraget;
    float sourceShapeTarget;
    float sideShapesTraget;


    void Awake()
    {
        if (toolbar == null)
        {
            toolbar = this.GetComponent<RectTransform>();

            PaletteBoard = GameObject.FindGameObjectWithTag("PaletteBoard").GetComponent<RectTransform>();
            sideShapes = PaletteBoard.parent.Find("PlanParts").GetComponent<RectTransform>();



            toolbarPos = toolbar.anchoredPosition;
            paletteBoardOffsetMin = PaletteBoard.offsetMin;
            sideShapesOffsetMin = sideShapes.offsetMin;
        }

        timer = 3.0f;
    }

    void FixedUpdate()
    {
        if (timer < 2)
        {
            timer += Time.fixedDeltaTime;
            toolbarPos = toolbar.anchoredPosition;
            toolbarPos.x = Mathf.Lerp(toolbarPos.x, toolbarTarget, timer);
            toolbar.anchoredPosition = toolbarPos;


            paletteBoardOffsetMin = PaletteBoard.offsetMin;
            sideShapesOffsetMin = sideShapes.offsetMin;
            paletteBoardOffsetMin.x = Mathf.Lerp(paletteBoardOffsetMin.x, paletteBoardTraget, timer);
            sideShapesOffsetMin.x = Mathf.Lerp(sideShapesOffsetMin.x, sideShapesTraget, timer);

            PaletteBoard.offsetMin = paletteBoardOffsetMin;
            sideShapes.offsetMin = sideShapesOffsetMin;
        }
    }

    public void ShowOrHide()
    {
        timer = 0;
        if (isHiding)
        {
            Show();
            isHiding = false;
        }
        else
        {
            Hide();
            isHiding = true;
        }
    }

    private void Show()
    {
        toolbarTarget = 21.3f;

        paletteBoardTraget = 42.8f;
        sideShapesTraget = 42.8f;
    }

    private void Hide()
    {
        toolbarTarget = -21.5f;

        paletteBoardTraget = -0.5f;
        sideShapesTraget = -0.5f;
    }
}
