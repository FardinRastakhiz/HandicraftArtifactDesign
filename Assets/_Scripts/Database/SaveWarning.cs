///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <SaveWarning>
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

public class SaveWarning : MonoBehaviour {

    public static Board board;
    static GameObject filter;
    static GameObject thisObject;
    public static void Show(Board targetBoard)
    {
        board = targetBoard;
        thisObject = GameObject.FindGameObjectWithTag("Canvas").transform.Find("SaveChangesWarning").gameObject;
        thisObject.SetActive(true);
        thisObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        filter = thisObject.transform.parent.Find("ClickFilter").gameObject;
        filter.SetActive(true);
    }
    public void On_Save_Click()
    {
        if (board.plan.isOriginal)
        {
            SelectTools.SelectAll();
            CopyTool.TakeShapes();
            CreateNewPlan.CopyShow(board.plan.plan.transform.parent.parent.parent.GetComponent<Cupboards>(), board);
            CloseWindow();
        }
        else
        {
            GameObject.Find("ScreenShot").GetComponent<ScreenShot>().ForceTakeScreenShot(board);
            CloseWindow();
        }
    }

    public void On_Dont_Save_Click()
    {
        if (Board.close)
        {
            board.Close_Without_Save();
            Board.close = false;
        }
        CloseWindow();
    }

    public void On_Cancel_Close()
    {
        Board.close = false;
        CloseWindow();
    }
    void CloseWindow()
    {
        filter.SetActive(false);
        thisObject.SetActive(false);
    }
}
