///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Planbar>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public class Planbar : MonoBehaviour {
    private bool isHiding;
    static RectTransform planTabs;
    static RectTransform sourceShapes;
    static RectTransform sideShapes;
    static RectTransform PaletteBoard;


    Vector3 planbarPos;

    Vector2 paletteBoardOffsetMax;
    Vector2 sourceShapesOffsetMax;
    Vector2 sideShapesOffsetMax;
    float timer;
    float planTabsTarget;
    float paletteBoardTraget;
    float sourceShapesTraget;
    float sideShapesTraget;

    void Awake()
    {
        if (planTabs == null)
        {
            planTabs = this.GetComponent<RectTransform>();
            PaletteBoard = GameObject.FindGameObjectWithTag("PaletteBoard").GetComponent<RectTransform>();
            sourceShapes = PaletteBoard.parent.Find("SourceShapes").GetComponent<RectTransform>();
            sideShapes = PaletteBoard.parent.Find("PlanParts").GetComponent<RectTransform>();
        }

        timer = 3.0f;
    }

    void FixedUpdate()
    {
        if (timer < 1.1f)
        {
            timer += Time.fixedDeltaTime;

            planbarPos = planTabs.anchoredPosition;
            paletteBoardOffsetMax = PaletteBoard.offsetMax;
            sourceShapesOffsetMax = sourceShapes.offsetMax;
            sideShapesOffsetMax = sideShapes.offsetMax;

            planbarPos.x = Mathf.Lerp(planbarPos.x, planTabsTarget, timer);
            paletteBoardOffsetMax.x = Mathf.Lerp(paletteBoardOffsetMax.x, paletteBoardTraget, timer);
            sourceShapesOffsetMax.x = Mathf.Lerp(sourceShapesOffsetMax.x, sourceShapesTraget, timer);
            sideShapesOffsetMax.x = Mathf.Lerp(sideShapesOffsetMax.x, sideShapesTraget, timer);

            planTabs.anchoredPosition = planbarPos;
            PaletteBoard.offsetMax = paletteBoardOffsetMax;
            sourceShapes.offsetMax = sourceShapesOffsetMax;
            sideShapes.offsetMax = sideShapesOffsetMax;
            //PaletteBoard.up = 1.0f;
        }
        else
        {
            if (isHiding && gameObject.activeSelf)
                gameObject.SetActive(false);
        }
    }

    public void ShowOrHide()
    {
        timer = 0;
        if (isHiding)
        {
            Show();
            gameObject.SetActive(true);
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
        planTabsTarget = -85.7f;
        paletteBoardTraget = -171.327f;
        sourceShapesTraget = -171.327f;
        sideShapesTraget = -171.327f;
    }

    private void Hide()
    {
        planTabsTarget = 86.5f;
        paletteBoardTraget = -0.0f;
        sourceShapesTraget = -0.0f;
        sideShapesTraget = -0.0f;
    }
}