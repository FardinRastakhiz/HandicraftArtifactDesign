///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ToolsUI>
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
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class ToolsUI : MonoBehaviour
{
    static Dictionary<string, int> indx;
    static List<Button> buttons;
    static Fardin.ColorTools.ColorTerminal colorTerminal;
    static Image colorPalette;
    static Fardin.ColorTools.ColorPick colorPick;
    public static void Initialize()
    {
        indx = new Dictionary<string, int>();
        buttons = new List<Button>();
        if (UIController.tools == null)
            return;
        Transform tools = UIController.tools.transform;
        foreach (Transform item in tools)
        {
            indx.Add(item.name, buttons.Count);
            buttons.Add(item.GetComponent<Button>());
        }
        colorPalette = tools.Find("ColorPalette").Find("Image").GetComponent<Image>();
        colorPick = tools.Find("PickTools").GetComponent<Fardin.ColorTools.ColorPick>();
        colorTerminal = GameObject.FindGameObjectWithTag("Canvas").transform.Find("ColorTools").GetComponent<Fardin.ColorTools.ColorTerminal>();
        colorPick.OnPickColor += On_Color_Pick_Change;
    }
    
    public void Select()
    {
        GameObject ToolButton = EventSystem.current.currentSelectedGameObject;
        CustomSelect(ToolButton);
    }

    public void CustomSelect(GameObject ToolButton)
    {
        string name = ToolButton.name;
        DeactiveButtons();
        SetActiveButton(name);

        /*if (!GenBoardPlan.isBoardPlan())
        {
            ToolsUtility.SetToolsState(ToolsState.SELECT, ToolButton);
            return;
        }*/

        switch (name)
        {
            case "SELECT":
                ToolsUtility.SetToolsState(ToolsState.SELECT, ToolButton);
                break;
            case "MOVE":
                ToolsUtility.SetToolsState(ToolsState.MOVE, ToolButton);
                break;
            case "ROTATE":
                ToolsUtility.SetToolsState(ToolsState.ROTATE, ToolButton);
                break;
            case "SCALE":
                ToolsUtility.SetToolsState(ToolsState.SCALE, ToolButton);
                break;
        }
    }

    static void DeactiveButtons()
    {
        foreach (Button item in buttons)
	    {
            ColorBlock colors = item.colors;
            colors.normalColor = new Color(255, 255, 255);
            colors.highlightedColor = new Color(0, 255, 0);
            item.colors = colors;
	    }
    }
    static void SetActiveButton(string name)
    {
        ColorBlock colors = buttons[indx[name]].colors;
        colors.normalColor = new Color(0, 255, 255);
        colors.highlightedColor = new Color(0, 0, 255);
        buttons[indx[name]].colors = colors;
    }


    public void Copy()
    {
        CopyTool.TakeShapes();
    }

    public void Cut()
    {
        CutTool.TakeShapes();
    }

    public void Paste()
    {
        PasteTool.DropShapes(RightClick.mousePosition);
    }


    public void ColorPickerSet(Color color)
    {

    }

    public void Mirror()
    {
        CustomSelect(transform.Find("SELECT").gameObject);
        MirrorTools.HorizontalMirror();
    }

    protected static void On_Color_Pick_Change(object o, Fardin.ColorTools.OnPickColorHandler e)
    {
        if (colorTerminal.gameObject.activeSelf)
            Fardin.ColorTools.FillColorForm.ByRGBA(e.color,colorTerminal.colorForm);
        else
        {
            colorPalette.color = e.color;
            ColorTools.SetSelectionsColor(e.color);
        }
    }
}
