///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ExitSoftware>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using System.Collections.Generic;
using UnityEngine;

public class ExitSoftware: MonoBehaviour {

    static bool isQuiting;
    static GameObject quitWarning;
    static int openBoards;
    static bool forceQuit;
    public static bool Exit()
    {
        if (forceQuit)
            return true;
        openBoards = BoardPlans.boardPlans.Count;
        if (openBoards != 0)
        {
            quitWarning = GameObject.FindGameObjectWithTag("Canvas").transform.parent.Find("QuitWarning").gameObject;
            quitWarning.transform.SetAsLastSibling();
            quitWarning.SetActive(true);
        }
        else
            return true;
        return false;

    }

    void Update()
    {
        if (isQuiting)
        {
            if (!Board.closePlan)
            {
                if (BoardPlans.boardPlans.Count == 0)
                {
                    Application.Quit();
                    isQuiting = false;
                    return;
                }
                BoardPlans.boardPlans[0].board.Close_Board();
            }
        }
    }

    public void On_Save_And_Quit()
    {
        isQuiting = true;
        quitWarning.SetActive(false);
    }

    public void On_Without_Save_Quit()
    {
        forceQuit = true;
        Application.Quit();
        isQuiting = false;
        quitWarning.SetActive(false);
    }

    public void On_Cancel_Quit()
    {
        isQuiting = false;
        quitWarning.SetActive(false);
    }

    
}
