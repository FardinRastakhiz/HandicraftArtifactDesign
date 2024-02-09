using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowsMenu : MonoBehaviour {

    [SerializeField]
    GameObject[] windows;
    GameObject[] allCheckBoxes;
    public void OnStartWindow()
    {
        allCheckBoxes = new GameObject[windows.Length];
        for (int i = 0; i < windows.Length; i++)
        {
            allCheckBoxes[i] = transform.Find(windows[i].name).Find("CheckMark").gameObject;
            if (windows[i].activeSelf)
            {
                allCheckBoxes[i].SetActive(true);
            }
            else
            {
                allCheckBoxes[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        
        for (int i = 0; i < allCheckBoxes.Length; i++)
        {
            if (windows[i].activeSelf)
            {
                if (!allCheckBoxes[i].activeSelf)
                    allCheckBoxes[i].SetActive(true);
            }
            else
            {
                if (allCheckBoxes[i].activeSelf)
                    allCheckBoxes[i].SetActive(false);
            }
        }
    }

    GameObject checkBox;
    public void On_Tools_Click(GameObject toolbar)
    {
        checkBox = EventSystem.current.currentSelectedGameObject.transform.Find("CheckMark").gameObject;
        SetWindowState(toolbar);
    }

    public void On_SourceShapes_Click(GameObject SourceShapes)
    {
        checkBox = EventSystem.current.currentSelectedGameObject.transform.Find("CheckMark").gameObject;
        checkBox.SetActive(!checkBox.activeSelf);
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<TopScrollbar>().On_TopScroll_Active(SourceShapes.GetComponent<RectTransform>());
    }

    public void On_PlanParts_Click(GameObject PlanParts)
    {
        checkBox = EventSystem.current.currentSelectedGameObject.transform.Find("CheckMark").gameObject;
        checkBox.SetActive(!checkBox.activeSelf);
        PlanParts.GetComponent<Sideshapebar>().ShowOrHide();
    }

    public void On_PlanCategories_Click(GameObject PlanCategories)
    {
        checkBox = EventSystem.current.currentSelectedGameObject.transform.Find("CheckMark").gameObject;
        checkBox.SetActive(!checkBox.activeSelf);
        PlanCategories.GetComponent<Planbar>().ShowOrHide();
    }

    public void On_ColorTools_Click(GameObject ColorTools)
    {
        checkBox = EventSystem.current.currentSelectedGameObject.transform.Find("CheckMark").gameObject;
        SetWindowState(ColorTools);
        Fardin.ColorTools.ColorTerminal colorTerminal = ColorTools.GetComponent<Fardin.ColorTools.ColorTerminal>();
        colorTerminal.changedColor -= elementColorTools.On_Color_Change;
        colorTerminal.changedColor -= SetCanvasColor.On_Color_Change;
        //GameObject tools = GameObject.FindGameObjectWithTag("Tools");
        if (UIController.tools != null)
        {
            UIController.tools.transform.Find("ColorPick").GetComponent<elementColorTools>().OnColorTools();
        }
        else
        {
            colorTerminal.changedColor += elementColorTools.On_Color_Change;
        }
    }


    void SetWindowState(GameObject window)
    {
        if (window.activeSelf)
        {
            window.SetActive(false);
            checkBox.SetActive(false);
        }
        else
        {
            window.SetActive(true);
            checkBox.SetActive(true);
        }
    }


}
