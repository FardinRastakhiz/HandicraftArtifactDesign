using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraButtonsColors : MonoBehaviour {
    [SerializeField]
    UnityEngine.UI.Image[] buttons;

    float start = 0.0f;
    float space = 0.0f;

	// Use this for initialization
	void Start () {
        space = 0.65f / buttons.Length;
	}
	
	 //Update is called once per frame
    void Update () {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].color = Color.HSVToRGB(Eval(start + space * i), 1.0f, 0.5f);
        }
        start += Time.deltaTime*0.1f;
        start = start > 1.0f ? 0.0f : start;
    }

    float Eval(float hue)
    {
        return hue > 1.0f ? hue - 1.0f : hue;
    }
}
