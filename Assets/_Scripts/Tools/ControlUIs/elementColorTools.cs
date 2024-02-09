using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class elementColorTools : MonoBehaviour {

    Color elementsColor;
    Button button;
    static Fardin.ColorTools.ColorTerminal colorTerminal;
    static Image colorPalette;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        colorTerminal = GameObject.FindGameObjectWithTag("Canvas").transform.Find("ColorTools").GetComponent<Fardin.ColorTools.ColorTerminal>();
        colorPalette = transform.parent.Find("ColorPalette").Find("Image").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        try
        {
            if (SelectTools.lastShapes.Count == 0)
            {
                if (button.interactable)
                {
                    button.interactable = false;
                }
            }
            else
            {
                if (!button.interactable)
                {
                    button.interactable = true;
                }
            }
        }
        catch(System.NullReferenceException)
        {
            Debug.Log("elementColorTools : " + typeof(System.NullReferenceException));
        }
        catch (System.Exception)
        {
            Debug.Log("elementColorTools : " + typeof(System.Exception));
        }
        
	}

    public void OnColorTools()
    {
        colorTerminal.starterColor = colorPalette.color;
        colorTerminal.gameObject.SetActive(true);
        colorTerminal.transform.Find("Drag").Find("Text").GetComponent<Text>().text = "رنگ المان ها";
        colorTerminal.changedColor -= SetCanvasColor.On_Color_Change;
        colorTerminal.changedColor -= On_Color_Change;
        colorTerminal.changedColor += On_Color_Change;
        //if (!colorTerminal.gameObject.activeSelf)
        //{
        //}
    }
    public static void On_Color_Change(object o, Fardin.ColorTools.OnChangeColorHandler e)
    {
        colorPalette.color = e.form.RGB;
        ColorTools.SetSelectionsColor(e.form.RGB);
    }
}
