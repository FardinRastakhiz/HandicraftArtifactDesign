using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileMenu : MonoBehaviour {
    [SerializeField]
    UnityEngine.UI.Button openButton;
    [SerializeField]
    UnityEngine.UI.Button saveButton;
    [SerializeField]
    UnityEngine.UI.Button saveAsButton;
    [SerializeField]
    UnityEngine.UI.Button saveAllButton;
    [SerializeField]
    UnityEngine.UI.Button newPlan;
    [SerializeField]
    UnityEngine.UI.Button exportButton;
    [SerializeField]
    UnityEngine.UI.Button exitButton;



    public void On_File_Start()
    {
        if (BoardPlans.ActiveIndex == -1)
        {
            if (BoardPlans.boardPlans.Count==0)
                saveAllButton.interactable = false;
            else
                saveAllButton.interactable = true;
            saveAsButton.interactable = false;
            saveButton.interactable = false;
            exportButton.interactable = false;
        }
        else
        {
            saveAllButton.interactable = true;
            saveAsButton.interactable = true;
            saveButton.interactable = true;
            exportButton.interactable = true;
        }
    }

    public void Exit_Software()
    {
        Application.Quit();
        //ExitSoftware.Exit();
        transform.parent.GetComponent<Menubar>().On_Free_Space();
    }

    public void Save_BoardPlan()
    {
        if (BoardPlans.ActiveIndex!=-1)
        {
            Board board = BoardPlans.boardPlans[BoardPlans.ActiveIndex].board;
            GameObject.Find("ScreenShot").GetComponent<ScreenShot>().Take_ScreenShot(board);
        }
    }

    public void Save_As_New_BoardPlan()
    {

    }

    public void Save_All_BoardPlan()
    {
        if (!ScreenShot.takingShot && !ScreenShot.TakeCompleteShot)
        {
            Debug.Log("this Happening");
            StartCoroutine(GameObject.Find("ScreenShot").GetComponent<ScreenShot>().Take_All_BoardPlans());
        }
    }

    
}
