///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Board>
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
using System.IO;
using System.Linq;

public class Board : MonoBehaviour {
    public BoardPlan plan;
    public GameObject UIObject;
    public static bool close;
    public static bool closePlan;
    RectTransform rectTra;
    Vector3 mouseStart;
    Vector2 localRectStart;
    Vector3 rectStart;
    ScreenShot screenShot;




    public void Close_Board()
    {
        if (screenShot==null)
        {
            screenShot = GameObject.Find("ScreenShot").GetComponent<ScreenShot>();
        }
        close = true;
        closePlan = true;
        SaveWarning.Show(this);
    }

    public void Close_Without_Save()
    {
        StartCoroutine(Close_Active_Plan());
    }
    public IEnumerator Close_Active_Plan()
    {
        int currentIndex = BoardPlans.boardPlans.IndexOf(plan);
        Debug.Log(currentIndex + " : " + plan.name + " : " + BoardPlans.boardPlans.Contains(plan));
        BoardPlans.boardPlans[currentIndex].board.transform.SetAsLastSibling();
        Debug.Log("0");
        SelectContainer.RemoveSelects(currentIndex);
        Debug.Log("01");
        yield return SelectTools.ResetTotal();
        Debug.Log("002");
        yield return GenSidebar.removeExcess(plan.name);
        Debug.Log("0003");
        yield return GenBoardPlan.Close(currentIndex);
        Debug.Log("00004");
        yield return new WaitForEndOfFrame();
        BoardPlans.ActiveIndex = -1;
        Debug.Log("000005");
        Board.closePlan = false;
        Destroy(this.gameObject);
    }

    public void SetBoardRect()
    {
        rectTra = gameObject.GetComponent<RectTransform>();
        rectTra.anchorMin = new Vector2(0, 1);
        rectTra.anchorMax = new Vector2(0, 1);
        rectTra.anchoredPosition = (new Vector2(rectTra.sizeDelta.x, -rectTra.sizeDelta.y)) * 0.5f;
        rectTra.localScale = new Vector3(1, 1, 1);
    }


    public void Set_BoardPlan_Index()
    {
        GenBoardPlan.SetBoardPlanIndex(plan);
    }


    public void zoom()
    {
        Set_BoardPlan_Index();
        ZoomBoard.Zoom(EventSystem.current.currentSelectedGameObject.transform);
    }
}
