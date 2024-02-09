///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <StartProgram>
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

public class StartProgram : MonoBehaviour {

    Rect paletteBoard;
    GameObject window;
    float screenHeight;
    float paletteHeight;
    RectTransform paletteTra;
    void Awake()
    {
        screenHeight = Screen.height;
        Application.wantsToQuit += ExitSoftware.Exit;
        Screen.fullScreen = false;
        UIController uiController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
        uiController.SetAsActiveWindow();
        paletteTra = GameObject.FindGameObjectWithTag("PaletteBoard").GetComponent<RectTransform>();
        paletteBoard = Fardin.UITools.RectTools.ToScreen(paletteTra);
        new BoardPlans();
        BoardPlans.ActiveIndex = -1;
        SQLiteExecute.SetDataPath();
        SelectTools.InitializeProperties();
        ToolsUI.Initialize();
        GenSourceShaps.Generate();
        GenSourceBackgrounds.Generate();
        GenboardCanvas.Generate();
        GenPlans.Start();
        ToolsUtility.SetToolsState(ToolsState.SELECT, UIController.tools.transform.Find("SELECT").gameObject);
        window = GameObject.FindGameObjectWithTag("Windows");
	}

    void Update()
    {
        if (screenHeight != Screen.height || paletteTra.rect.height != paletteHeight)
        {
            paletteBoard = Fardin.UITools.RectTools.ToScreen(paletteTra);
            paletteHeight = paletteTra.rect.height;
            screenHeight = Screen.height;
        }

        if (Screen.fullScreen)
        {
            if (!window.activeSelf)
            {
                window.SetActive(true);
            }
        }
        else
        {
            if (window.activeSelf)
            {
                window.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (SelectTools.lastShapes.Count>0)
            {
                DeleteShapes.Delete();
            }
        }

        RightClick.UpdateRightClick(paletteBoard);
    }

}

