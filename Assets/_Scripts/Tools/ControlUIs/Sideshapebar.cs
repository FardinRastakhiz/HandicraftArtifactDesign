///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Sideshapebar>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public class Sideshapebar : MonoBehaviour {


    Vector3 sideShapesPos;

    private bool isHiding;
    float timer;


    static RectTransform PaletteBoard;
    static RectTransform sideShapes;

    Vector2 paletteBoardOffsetMin;
    Vector2 sideShapesOffsetMin;

    float paletteBoardTraget;
    float sideShapesTraget;


    void Awake()
    {
        if (sideShapes == null)
        {
            sideShapes = this.GetComponent<RectTransform>();

            PaletteBoard = GameObject.FindGameObjectWithTag("PaletteBoard").GetComponent<RectTransform>();
        }
        timer = 3.0f;

    }

    void FixedUpdate()
    {
        if (timer < 1.1f)
        {
            timer += Time.fixedDeltaTime;

            sideShapesPos = sideShapes.anchoredPosition;
            sideShapesPos.y = Mathf.Lerp(sideShapesPos.y, sideShapesTraget, timer);
            sideShapes.anchoredPosition = sideShapesPos;

            paletteBoardOffsetMin = PaletteBoard.offsetMin;
            paletteBoardOffsetMin.y = Mathf.Lerp(paletteBoardOffsetMin.y, paletteBoardTraget, timer);
            PaletteBoard.offsetMin = paletteBoardOffsetMin;
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
        sideShapesTraget = 0.0f;
        paletteBoardTraget = 65.0f;
    }

    private void Hide()
    {
        sideShapesTraget = -65.0f;
        paletteBoardTraget = 0.0f;
    }
}
