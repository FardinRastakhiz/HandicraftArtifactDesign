///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <UIController>
///   Description:    <Overal control over software shell>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:           Date:        Description:
///-----------------------------------------------------------------
///</symmary>

using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern int SetForegroundWindow(IntPtr hwnd);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    public Transform paletteWindow;

    public GameObject designBoards;

    public GameObject openWindow;

    public GameObject addNewTab;

    public GameObject createNewPlan;

    public GameObject deleteWarning;

    public GameObject planParts;

    [SerializeField]
    GameObject _tools;
    public static GameObject tools;

    void Awake()
    {
        tools = _tools;
    }
    public void Close_Software()
    {
        Application.Quit();
        //ExitSoftware.Exit();
	}

    public void SetAsActiveWindow()
    {
        SetForegroundWindow(GetActiveWindow());
    }

    int screenWidth = 1024;
    int screenHeight = 768;
    public void Restore_Down_Software()
    {
        SetAsActiveWindow();
        if (Screen.fullScreen)
            Screen.SetResolution(screenWidth, screenHeight, false);
        else
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        }
    }

    public void minimize_Software()
    {
        ShowWindow(GetActiveWindow(), 2);

    }

    public void Change_Title_Texture()
    {
        GameObject categoryButton = EventSystem.current.currentSelectedGameObject;
        Categorization.changeImage(categoryButton);
    }

    public void MoveScroll()
    {
        var mover = EventSystem.current.currentSelectedGameObject.transform;
        var name = mover.name;
        var scrollContent = mover.parent.Find("ScrollContent").GetComponent<UnityEngine.UI.ScrollRect>();

        switch (name)
        {
            case "left":
                scrollContent.horizontalScrollbar.value -= 0.1f;
                break;
            case "right":
                scrollContent.horizontalScrollbar.value += 0.1f;
                break;
            case "up":
                scrollContent.verticalScrollbar.value += 0.1f;
                break;
            case "down":
                scrollContent.verticalScrollbar.value -= 0.1f;
                break;
            default: 
                break;
        }
    }

    public void On_Shape_Click()
    {
        GameObject shape = EventSystem.current.currentSelectedGameObject;
    }

    void SetNewShapes()
    {

    }

    public void Create_New_Plan(Cupboards Category)
    {
        CreateNewPlan.Show(Category);
    }

    public static void DestoryObject(GameObject obj)
    {
        Destroy(obj);
    }

    public void LoadPlanImage(Plan plan)
    {
        StartCoroutine(LoadPImage(plan));
    }

    IEnumerator LoadPImage(Plan plan)
    {
        string imagePath = Application.dataPath + "/Resources/" + plan.sourceImage + ".png";
        string imageURL = "file://" + imagePath;
        WWW www = new WWW(imageURL);
        yield return www;
        Texture2D targetImg = new Texture2D(www.texture.width, www.texture.height, TextureFormat.RGB24, false);
        www.LoadImageIntoTexture(targetImg);
        plan.transform.Find("PlanImage").GetComponent<Image>().sprite = Sprite.Create(targetImg, new Rect(0, 0, targetImg.width, targetImg.height),
            new Vector2(0.5f, 0.5f), 1.0f);
    }
}