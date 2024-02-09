///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Shortcuts>
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

public class DefaultShortcuts : MonoBehaviour
{
    public static bool ctrlActive;
    public static bool shiftActive;
    static bool shiftTool;

    static ToolsUI toolsButtons;

    public static bool allowed;
    static string lastTool;
	// Use this for initialization
	void Start () {
        if (UIController.tools != null)
            toolsButtons = UIController.tools.GetComponent<ToolsUI>();
        allowed = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (allowed)
            GetKeys();
	}

    void GetKeys()
    {
        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SelectTools.SelectAll();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                CopyTool.TakeShapes();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                CutTool.TakeShapes();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                PasteTool.DropShapes(RightClick.mousePosition);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                DuplicateShapes.Duplicate();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (!ScreenShot.takingShot && !ScreenShot.TakeCompleteShot)
                {
                    if (BoardPlans.ActiveIndex == -1)
                    {
                        StartCoroutine(GameObject.Find("ScreenShot").GetComponent<ScreenShot>().Take_All_BoardPlans());
                    }
                    else
                    {
                        Board board = BoardPlans.boardPlans[BoardPlans.ActiveIndex].board;
                        GameObject.Find("ScreenShot").GetComponent<ScreenShot>().Take_ScreenShot(board);
                    }
                }
            }
            ctrlActive = true;
        }
        else
        {
            ctrlActive = false;
        }
        if (BoardPlans.ActiveIndex != -1)
        {
            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
            {
                if (!shiftActive)
                {
                    Debug.Log("SelectTools.lastShapes.Count:" + SelectTools.lastShapes.Count);
                    if (SelectTools.lastShapes.Count > 0)
                    {
                        lastTool = ToolsUtility.toolState.ToString();
                        Debug.Log("lastToolaa: " + lastTool);
                        toolsButtons.CustomSelect(toolsButtons.transform.Find("SCALE").gameObject);
                        shiftTool = true;
                    }
                    else
                        BoardZoom.Interactible = true;
                }
                shiftActive = true;
            }
            else
            {
                if (shiftActive)
                {
                    if (shiftTool)
                    {
                        toolsButtons.CustomSelect(toolsButtons.transform.Find(lastTool).gameObject);
                        shiftTool = false;
                    }
                    else
                        BoardZoom.Interactible = false;
                }
                shiftActive = false;
            }
        }
        
    }
    public static List<Shortcut> Reset()
    {
        List<Shortcut> shortCuts = new List<Shortcut>();

        return shortCuts;
    }
}
