using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCanvasColor : MonoBehaviour {

    Color canvasColor;
    Button button;
    static Fardin.ColorTools.ColorTerminal colorTerminal;
    static Image colorPalette;
    static Transform boardCanvas;
    BoardPlan activePlan;
	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        colorTerminal = GameObject.FindGameObjectWithTag("Canvas").transform.Find("ColorTools").GetComponent<Fardin.ColorTools.ColorTerminal>();
        colorPalette = transform.parent.Find("ColorPalette").Find("Image").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (BoardPlans.ActiveIndex == -1 || Board.closePlan)
        {
            if (button.interactable)
            {
                button.interactable = false; 
                boardCanvas = null;
            }
        }
        else
        {
            try
            {
                if (!button.interactable || activePlan != BoardPlans.boardPlans[BoardPlans.ActiveIndex])
                {
                    button.interactable = true;
                    activePlan = BoardPlans.boardPlans[BoardPlans.ActiveIndex];
                    boardCanvas = activePlan.board.transform.Find("Mask").Find("Grid").Find("DrawCanvas");
                }
            }
            catch (System.NullReferenceException)
            {
                Debug.Log("SetCanvasColor : " + typeof(System.NullReferenceException));
            }
            catch (System.Exception)
            {
                Debug.Log("SetCanvasColor : " + typeof(System.Exception));
            }
        }
	}

    public void OnColorTools()
    {
        try
        {
            colorTerminal.starterColor = boardCanvas.GetComponent<Image>().color;
            colorTerminal.gameObject.SetActive(true);
            colorTerminal.transform.Find("Drag").Find("Text").GetComponent<Text>().text = "رنگ پس زمینه";
            colorTerminal.changedColor -= elementColorTools.On_Color_Change;
            colorTerminal.changedColor -= On_Color_Change;
            colorTerminal.changedColor += On_Color_Change;
        }
        catch (System.Exception)
        {
            Debug.Log("SetCanvasColor:OnColorTools");
        }
    }
    public static void On_Color_Change(object o, Fardin.ColorTools.OnChangeColorHandler e)
    {
        try
        {
            colorPalette.color = e.form.RGB;
            if (BoardPlans.ActiveIndex == -1)
                return;
            Color c = colorPalette.color;
            c.a = 1.0f;
            boardCanvas.GetComponent<Image>().color = c;
            BoardPlans.boardPlans[BoardPlans.ActiveIndex].CanvasColor = c;
        }
        catch (System.Exception)
        {
            Debug.Log("SetCanvasColor:On_Color_Change");
        }
    }
}
