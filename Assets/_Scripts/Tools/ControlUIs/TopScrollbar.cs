using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopScrollbar : MonoBehaviour {
    [SerializeField]
    List<RectTransform> topScrollRects;
    List<Vector3> Positions;
    float[] targetY;
    float[] allYs;
    bool[] isHiding;
    bool[] isHidden;
    int lastActiveBar = 0;

    [SerializeField]
    RectTransform PaletteBoard;


    float timer;
    int size = 0;
    Vector3 posModifier = Vector3.zero;

    void Awake()
    {
        size = topScrollRects.Count;
        allYs = new float[size];
        targetY = new float[size];
        isHiding = new bool[size];
        isHidden = new bool[size];
        Positions = new List<Vector3>();
        for (int i = 0; i < size; i++)
        {
            topScrollRects[i].anchoredPosition = new Vector2(topScrollRects[i].anchoredPosition.x, 32.5f);
            Positions.Add(topScrollRects[i].anchoredPosition);
            targetY[i] = 32.5f;
            isHiding[i] = true;
            isHidden[i] = true;
        }
        PaletteBoard.offsetMax = new Vector2(PaletteBoard.offsetMax.x, 0.0f);
        timer = 3.0f;

        isHiding[0] = false;
        ShowOrHide(0);
    }


    public void On_TopScroll_Active(RectTransform targetRect)
    {
        ShowOrHide(topScrollRects.IndexOf(targetRect));
    }


    void FixedUpdate()
    {
        if (timer < 1.1f)
        {
            timer += Time.fixedDeltaTime;

            for (int i = 0; i < size; i++)
            {
                
                posModifier = topScrollRects[i].anchoredPosition;
                posModifier.y = Mathf.Lerp(posModifier.y, targetY[i], timer);
                allYs[i] = posModifier.y;
                topScrollRects[i].anchoredPosition = posModifier;
            }
            PaletteBoard.offsetMax = new Vector3(PaletteBoard.offsetMax.x,Mathf.Min(allYs) - 32.5f);
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                if (isHiding[i])
                    if (!isHidden[i])
                    {
                        topScrollRects[i].gameObject.SetActive(false);
                        isHidden[i] = true;
                    }
            }
        }
    }

    private void ShowOrHide(int indx)
    {
        timer = 0;
        if (isHiding[indx])
        {
            Show(indx);
            topScrollRects[indx].gameObject.SetActive(true);
            isHidden[indx] = false;
            isHiding[indx] = false;
            if (indx != lastActiveBar)
                isHiding[lastActiveBar]= true;
            lastActiveBar = indx;
        }
        else
        {
            Hide(indx);
            isHiding[indx] = true;
        }
    }

    private void Show(int indx)
    {
        targetY[indx] = -32.5f;
        if (indx != lastActiveBar)
            targetY[lastActiveBar] = 32.5f;
    }

    private void Hide(int indx)
    {
        targetY[indx] = 32.5f;
    }
}
