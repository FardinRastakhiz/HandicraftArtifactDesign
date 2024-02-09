using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreDownBoard : MonoBehaviour {
    [SerializeField]
    Board board;
    RectTransform rectTra;
    RectInstance savedTra;

    [SerializeField]
    GameObject controlBorders;

    void Start()
    {
        rectTra = board.gameObject.GetComponent<RectTransform>();
        savedTra = new RectInstance(rectTra);
    }

    public void Restore_Down_Board()
    {
        board.Set_BoardPlan_Index();
        RectTransform paletteRect = board.transform.parent.GetComponent<RectTransform>();
        rectTra = board.gameObject.GetComponent<RectTransform>();
        if (rectTra.rect.width == paletteRect.rect.width)
        {
            savedTra.GiveDataTo(rectTra);
            controlBorders.SetActive(true);
            board.transform.GetComponent<Window>().IsFullScreen = false;
            HideBoards.HideBehind(GenBoardPlan.nextFullScreen(board.plan.order));
        }
        else
        {
            savedTra.GetDataFrom(rectTra);
            rectTra.anchorMin = Vector2.zero;
            rectTra.anchorMax = Vector2.one;
            rectTra.sizeDelta = Vector2.zero;
            rectTra.localPosition = Vector3.zero;
            controlBorders.SetActive(false);
            board.transform.GetComponent<Window>().IsFullScreen = true;
            HideBoards.HideBehind(board.plan.order);
            //HideBoards.HideExcept(BoardPlans.boardPlans.IndexOf(board.plan));
        }
    }
}
